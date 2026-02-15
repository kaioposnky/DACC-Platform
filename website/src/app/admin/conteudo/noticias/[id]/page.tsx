"use client";

import {
  ConfirmationModal,
  EditPageHeader,
  PageLoader,
  NewsForm,
  Button,
} from "@/components";
import { apiService } from "@/services/api";
import { News, User } from "@/types";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { toast } from "sonner";

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

  const handleSave = async () => {
    if (!news) return;
    setIsSaving(true);
    try {
      let formattedDate = news.date;
      if (formattedDate && !formattedDate.endsWith('Z')) {
        formattedDate = formattedDate.includes('T')
          ? `${formattedDate.split('.')[0]}Z`
          : `${formattedDate}T00:00:00Z`;
      }

      const ensureUtc = (dateStr?: string) => {
        if (!dateStr) return new Date().toISOString();
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
        categoryId: news.categoryId || news.category?.id,
        icon: news.icon,
        gradient: news.gradient,
        readMoreLink: news.readMoreLink,
        authorId: news.author?.id,
        createdAt: ensureUtc(newsAny.createdAt),
        updatedAt: new Date().toISOString()
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

  const handleGoBack = () => router.push("/admin/conteudo");
  const handleOpenDeleteModal = () => setDeleteModalOpen(true);
  const handleCloseDeleteModal = () => setDeleteModalOpen(false);

  const handleSetImage = (url: string) => {
    if (!news) return;
    setNews({ ...news, image: url });
  };

  const handleRemoveImage = () => {
    if (!news) return;
    setNews({ ...news, image: "" });
  };

  const handleChange = (field: keyof News, value: any) => {
    if (!news) return;
    setNews({ ...news, [field]: value });
  };

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
          text: news.category?.name || news.categoryName || "Sem categoria",
          colorClass: "bg-blue-100 text-blue-700 font-bold",
        }}
      />

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        <NewsForm
          news={news}
          users={users}
          onChange={handleChange}
          mode="edit"
        />
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
