"use client";

import {
  ConfirmationModal,
  EditPageHeader,
  PageLoader,
  AdminCard,
  Input,
  Select,
  Button,
  TagInput,
  ImageUploadCard,
  DateTimeInputs,
} from "@/components";
import { apiService } from "@/services/api";
import { News, User } from "@/types";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { toast } from "sonner";
import {
  UserIcon,
  Square3Stack3DIcon,
  DocumentTextIcon,
  PhotoIcon,
} from "@heroicons/react/24/outline";

export default function AdminEditNoticiaPage() {
  const router = useRouter();
  const params = useParams();

  const [news, setNews] = useState<News | null>(null);
  const [users, setUsers] = useState<User[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [newsResponse, usersResponse] = await Promise.all([
          apiService.getNewsItem(params.id as string),
          apiService.getUsers(),
        ]);

        // Map backend 'autor' (Portuguese) to 'author' (English/User interface)
        const newsData = newsResponse as any;
        if (newsData.autor) {
          newsResponse.author = {
            id: newsData.autor.id,
            name: newsData.autor.nome,
            lastName: newsData.autor.sobrenome,
            email: "", // Placeholder or map if available
            avatar: "", // Placeholder
            role: "administrador" // Placeholder
          } as User;
        }

        setNews(newsResponse);
        setUsers(usersResponse);
      } catch (error) {
        console.error(error);
        toast.error("Erro ao carregar dados");
      } finally {
        setIsLoading(false);
      }
    };
    fetchData();
  }, [params.id]);

  const handleGoBack = () => router.push("/admin/conteudo");

  if (isLoading) return <PageLoader />;
  if (!news)
    return (
      <div className="flex flex-col items-center justify-center h-[60vh]">
        <h1 className="text-2xl font-bold text-gray-500">
          Notícia não encontrada
        </h1>
        <Button onClick={handleGoBack} className="mt-4">
          Voltar para Conteúdos
        </Button>
      </div>
    );

  const handleSave = async () => {
    if (!news) return;
    setIsSaving(true);
    try {
      // Format date to UTC for PostgreSQL compatibility
      let formattedDate = news.date;
      if (formattedDate && !formattedDate.endsWith('Z')) {
        // Remove any milliseconds and add Z for UTC
        formattedDate = formattedDate.includes('T')
          ? `${formattedDate.split('.')[0]}Z`
          : `${formattedDate}T00:00:00Z`;
      }

      // Create payload with only necessary fields (exclude author object to avoid nested date issues)
      // Ensure createdAt and updatedAt are present and valid UTC strings
      const ensureUtc = (dateStr?: string) => {
        if (!dateStr) return new Date().toISOString(); // Fallback to now
        if (dateStr.endsWith('Z')) return dateStr;
        return dateStr.includes('T') ? `${dateStr.split('.')[0]}Z` : `${dateStr}T00:00:00Z`;
      };

      const newsAny = news as any;

      const payload = {
        id: news.id,
        title: news.title,
        description: news.description,
        content: news.content,
        readTime: news.readTime,
        image: news.image,
        tags: news.tags,
        date: formattedDate,
        category: news.category,
        icon: news.icon,
        gradient: news.gradient,
        readMoreLink: news.readMoreLink,
        authorId: news.author?.id,
        // Backend requires these fields to avoid Unspecified Kind error (DateTime.MinValue)
        createdAt: ensureUtc(newsAny.createdAt),
        updatedAt: new Date().toISOString() // Always update modification time
      };

      await apiService.updateNews(news.id, payload as any);
      toast.success("Notícia salva com sucesso!");
    } catch (error) {
      console.error(error);
      toast.error(`Erro ao salvar notícia: ${error}`);
    } finally {
      setIsSaving(false);
    }
  };

  const handleDelete = async () => {
    if (!news) return;
    setIsDeleting(true);
    try {
      await apiService.deleteNews(news.id);
      toast.success("Notícia excluída com sucesso!");
      router.push("/admin/conteudo");
    } catch (error) {
      console.error(error);
      toast.error(`Erro ao excluir notícia: ${error}`);
    } finally {
      setIsDeleting(false);
      setDeleteModalOpen(false);
    }
  };

  const handleOpenDeleteModal = () => setDeleteModalOpen(true);
  const handleCloseDeleteModal = () => setDeleteModalOpen(false);

  const handleTitleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!news) return;
    setNews({ ...news, title: e.target.value });
  };

  const handleDescriptionChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!news) return;
    setNews({ ...news, description: e.target.value });
  };

  const handleContentChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!news) return;
    setNews({ ...news, content: e.target.value });
  };

  const handleCategoryChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    if (!news) return;
    setNews({ ...news, category: e.target.value });
  };

  const handleAuthorChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    if (!news) return;
    const selectedAuthorId = e.target.value;
    const selectedAuthor = users.find(u => u.id === selectedAuthorId);

    if (selectedAuthor) {
      setNews({ ...news, author: selectedAuthor });
    } else {
      // Handle unselection if necessary, though typical for required fields
      const { author, ...rest } = news;
      setNews(rest as News);
    }
  };

  const handleReadTimeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!news) return;
    setNews({ ...news, readTime: parseInt(e.target.value) || 0 });
  };

  const handleSetImage = (url: string) => {
    if (!news) return;
    setNews({ ...news, image: url });
  };

  const handleRemoveImage = () => {
    if (!news) return;
    setNews({ ...news, image: "" });
  };

  const handleAddTag = (tag: string) => {
    if (!news) return;
    setNews({ ...news, tags: [...(news.tags || []), tag] });
  };

  const handleRemoveTag = (tag: string) => {
    if (!news) return;
    setNews({ ...news, tags: news.tags?.filter((t) => t !== tag) || [] });
  };

  const handleDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!news) return;
    const newDate = e.target.value;
    const currentTime = news.date.includes('T') ? news.date.split('T')[1].substring(0, 5) : '00:00';
    // Preserve existing time or default to 00:00
    setNews({ ...news, date: `${newDate}T${currentTime}:00` });
  };

  const handleTimeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!news) return;
    const newTime = e.target.value;
    const currentDate = news.date.split('T')[0];
    setNews({ ...news, date: `${currentDate}T${newTime}:00` });
  };

  return (
    <div className="pb-20">
      <EditPageHeader
        title="Editar Notícia"
        id={news.id}
        onBack={handleGoBack}
        onSave={handleSave}
        onDelete={handleOpenDeleteModal}
        showDelete={true}
        loadingSave={isSaving}
        loadingDelete={isDeleting}
        status={{
          text: news.category,
          colorClass: "bg-blue-100 text-blue-700 font-bold",
        }}
      />

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Coluna Principal */}
          <div className="lg:col-span-2 space-y-6">
            <AdminCard
              icon={<DocumentTextIcon className="w-5 h-5" />}
              title="Conteúdo da Notícia"
            >
              <div className="space-y-6">
                <Input
                  label="Título da Notícia"
                  placeholder="Ex: Novo laboratório de IA é inaugurado na FEI"
                  value={news.title}
                  onChange={handleTitleChange}
                  className="text-lg font-bold"
                />

                <Input
                  label="Descrição Curta (Resumo)"
                  placeholder="Um breve resumo que aparece na listagem..."
                  value={news.description}
                  onChange={handleDescriptionChange}
                  multiline={true}
                  rows={3}
                />

                <Input
                  label="Conteúdo Completo"
                  placeholder="Escreva aqui o corpo da notícia..."
                  value={news.content || ""}
                  onChange={handleContentChange}
                  multiline={true}
                  rows={15}
                />
              </div>
            </AdminCard>
          </div>

          {/* Coluna Lateral */}
          <div className="space-y-6">
            <AdminCard
              icon={<Square3Stack3DIcon className="w-5 h-5" />}
              title="Classificação"
            >
              <div className="space-y-4">
                <Select
                  label="Categoria"
                  value={news.category}
                  onChange={handleCategoryChange}
                  options={[
                    { label: "Acadêmico", value: "Acadêmico" },
                    { label: "Evento", value: "Evento" },
                    { label: "DACC", value: "DACC" },
                    { label: "Competição", value: "Competição" },
                    { label: "Outros", value: "Outros" },
                  ]}
                />
                <div>
                  <TagInput
                    label="Tags da Notícia"
                    tags={news.tags || []}
                    onAddTag={handleAddTag}
                    onRemoveTag={handleRemoveTag}
                  />
                </div>
              </div>
            </AdminCard>

            <AdminCard
              icon={<UserIcon className="w-5 h-5" />}
              title="Publicação"
            >
              <div className="space-y-4">
                <Select
                  label="Autor"
                  value={news.author?.id || ""}
                  onChange={handleAuthorChange}
                  options={[
                    { label: "Selecione um autor", value: "" },
                    ...users.map((user) => ({
                      label:
                        `${user.name} ${user.lastName || ""} ${user.ra ? `(RA: ${user.ra})` : ""}`.trim(),
                      value: user.id,
                    })),
                  ]}
                />
                <DateTimeInputs
                  dateLabel="Data de Publicação"
                  timeLabel="Horário"
                  dateValue={news.date}
                  timeValue={news.date.includes('T') ? news.date.split('T')[1].substring(0, 5) : ''}
                  onDateChange={handleDateChange}
                  onTimeChange={handleTimeChange}
                />
                <Input
                  label="Tempo de Leitura (minutos)"
                  type="number"
                  value={news.readTime || 0}
                  onChange={handleReadTimeChange}
                />
              </div>
            </AdminCard>

            <ImageUploadCard
              title="Imagem de Capa"
              description="Clique para alterar a imagem de capa."
              icon={<PhotoIcon className="w-10 h-10" />}
              image={news.image}
              onSetImage={handleSetImage}
              onRemoveImage={handleRemoveImage}
              galleryTitle="Gerenciar Imagem de Capa"
              galleryDescription="Esta imagem aparecerá no topo da notícia e nos cards da listagem."
              showModal={true}
            />
          </div>
        </div>
      </div>

      <ConfirmationModal
        isOpen={deleteModalOpen}
        onClose={handleCloseDeleteModal}
        onConfirm={handleDelete}
        title="Excluir Notícia"
        message={`Tem certeza que deseja excluir a notícia "${news.title}"? Esta ação não pode ser desfeita.`}
        confirmLabel="Sim, Excluir"
        isLoading={isDeleting}
      />
    </div>
  );
}
