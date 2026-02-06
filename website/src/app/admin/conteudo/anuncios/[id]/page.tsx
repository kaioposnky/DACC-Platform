"use client";

import {
  ConfirmationModal,
  EditPageHeader,
  PageLoader,
  AdminCard,
  Input,
  Select,
  ImageUploadCard,
} from "@/components";
import { apiService } from "@/services/api";
import { Announcement } from "@/types";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { toast } from "sonner";
import {
  MegaphoneIcon,
  DocumentTextIcon,
  PhotoIcon,
  LinkIcon,
  XMarkIcon,
} from "@heroicons/react/24/outline";

export default function AdminEditAnuncioPage() {
  const router = useRouter();
  const params = useParams();

  const [announcement, setAnnouncement] = useState<Announcement | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const announcementData = await apiService.getAnnouncement(params.id as string);
        setAnnouncement(announcementData);
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
    if (!announcement) return;
    setIsSaving(true);
    try {
      await apiService.updateAnnouncement(announcement.id, announcement);
      toast.success("Anúncio salvo com sucesso!");
    } catch (error) {
      console.error(error);
      toast.error("Erro ao salvar anúncio");
    } finally {
      setIsSaving(false);
    }
  };

  const handleDelete = async () => {
    if (!announcement) return;
    setIsDeleting(true);
    try {
      await apiService.deleteAnnouncement(announcement.id);
      toast.success("Anúncio excluído com sucesso!");
      router.push("/admin/conteudo");
    } catch (error) {
      console.error(error);
      toast.error("Erro ao excluir anúncio");
    } finally {
      setIsDeleting(false);
      setDeleteModalOpen(false);
    }
  };

  const handleGoBack = () => router.push("/admin/conteudo");
  const handleOpenDeleteModal = () => setDeleteModalOpen(true);
  const handleCloseDeleteModal = () => setDeleteModalOpen(false);

  const handleTitleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!announcement) return;
    setAnnouncement({ ...announcement, title: e.target.value });
  };

  const handleContentChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!announcement) return;
    setAnnouncement({ ...announcement, content: e.target.value });
  };

  const handleTypeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    if (!announcement) return;
    setAnnouncement({ ...announcement, type: e.target.value });
  };

  const handleIconChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!announcement) return;
    setAnnouncement({ ...announcement, icon: e.target.value });
  };

  const handlePrimaryButtonTextChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!announcement) return;
    setAnnouncement({ ...announcement, primaryButtonText: e.target.value });
  };

  const handlePrimaryButtonLinkChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!announcement) return;
    setAnnouncement({ ...announcement, primaryButtonLink: e.target.value });
  };

  const handleSecondaryButtonTextChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!announcement) return;
    setAnnouncement({ ...announcement, secondaryButtonText: e.target.value });
  };

  const handleSecondaryButtonLinkChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!announcement) return;
    setAnnouncement({ ...announcement, secondaryButtonLink: e.target.value });
  };

  const handleSetImage = (url: string) => {
    if (!announcement) return;
    setAnnouncement({ ...announcement, imageSrc: url });
  };

  const handleRemoveImage = () => {
    if (!announcement) return;
    setAnnouncement({ ...announcement, imageSrc: "" });
  };

  const handleImageAltChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!announcement) return;
    setAnnouncement({ ...announcement, imageAlt: e.target.value });
  };

  const addDetail = () => {
    if (!announcement) return;
    setAnnouncement({
      ...announcement,
      details: [...announcement.details, { icon: "", text: "" }],
    });
  };

  const removeDetail = (index: number) => {
    if (!announcement) return;
    setAnnouncement({
      ...announcement,
      details: announcement.details.filter((_, idx) => idx !== index),
    });
  };

  const updateDetailIcon = (index: number, icon: string) => {
    if (!announcement) return;
    const newDetails = [...announcement.details];
    newDetails[index] = { ...newDetails[index], icon };
    setAnnouncement({ ...announcement, details: newDetails });
  };

  const updateDetailText = (index: number, text: string) => {
    if (!announcement) return;
    const newDetails = [...announcement.details];
    newDetails[index] = { ...newDetails[index], text };
    setAnnouncement({ ...announcement, details: newDetails });
  };

  if (isLoading) return <PageLoader />;
  if (!announcement)
    return (
      <div className="flex flex-col items-center justify-center h-[60vh]">
        <h1 className="text-2xl font-bold text-gray-400">
          Anúncio não encontrado
        </h1>
      </div>
    );

  const isEvent = announcement.type === 'event';

  return (
    <div className="pb-20">
      <EditPageHeader
        title="Editar Anúncio"
        id={announcement.id}
        onBack={handleGoBack}
        onSave={handleSave}
        onDelete={handleOpenDeleteModal}
        showDelete={true}
        loadingSave={isSaving}
        loadingDelete={isDeleting}
        status={{
          text: isEvent ? "Evento" : "Destaque",
          colorClass: isEvent
            ? "bg-purple-100 text-purple-700"
            : "bg-blue-100 text-blue-700",
        }}
      />

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Coluna Principal */}
          <div className="lg:col-span-2 space-y-6">
            <AdminCard
              icon={<DocumentTextIcon className="w-5 h-5 text-primary" />}
              title="Conteúdo do Anúncio"
            >
              <div className="space-y-6">
                <Input
                  label="Título do Anúncio"
                  placeholder="Ex: Novo Evento DACC"
                  value={announcement.title}
                  onChange={handleTitleChange}
                  className="text-lg font-bold"
                />

                <Input
                  label="Conteúdo / Descrição"
                  placeholder="Descreva o anúncio..."
                  value={announcement.content}
                  onChange={handleContentChange}
                  multiline={true}
                  rows={6}
                />

                <Input
                  label="Ícone (Emoji ou classe CSS)"
                  placeholder="Ex: 🎉 ou fa-rocket"
                  value={announcement.icon}
                  onChange={handleIconChange}
                />
              </div>
            </AdminCard>

            <AdminCard
              icon={<LinkIcon className="w-5 h-5 text-primary" />}
              title="Botões de Ação"
            >
              <div className="space-y-6">
                <div className="space-y-4">
                  <h4 className="text-sm font-semibold text-gray-700">Botão Primário</h4>
                  <Input
                    label="Texto do Botão"
                    placeholder="Ex: Saiba Mais"
                    value={announcement.primaryButtonText}
                    onChange={handlePrimaryButtonTextChange}
                  />
                  <Input
                    label="Link do Botão"
                    placeholder="Ex: /eventos/123"
                    value={announcement.primaryButtonLink}
                    onChange={handlePrimaryButtonLinkChange}
                  />
                </div>

                <div className="border-t border-gray-100 pt-6 space-y-4">
                  <h4 className="text-sm font-semibold text-gray-700">Botão Secundário</h4>
                  <Input
                    label="Texto do Botão"
                    placeholder="Ex: Ver Detalhes"
                    value={announcement.secondaryButtonText}
                    onChange={handleSecondaryButtonTextChange}
                  />
                  <Input
                    label="Link do Botão"
                    placeholder="Ex: /projetos/456"
                    value={announcement.secondaryButtonLink}
                    onChange={handleSecondaryButtonLinkChange}
                  />
                </div>
              </div>
            </AdminCard>

            <AdminCard
              icon={<MegaphoneIcon className="w-5 h-5 text-primary" />}
              title="Detalhes do Anúncio"
            >
              <div className="space-y-4">
                <div className="flex items-center justify-between">
                  <p className="text-sm text-gray-600">
                    Adicione itens com ícone e texto para destacar informações
                  </p>
                  <button
                    type="button"
                    onClick={addDetail}
                    className="px-3 py-1.5 bg-primary text-white rounded-lg text-sm font-medium hover:bg-primary/90 transition-colors"
                  >
                    + Adicionar
                  </button>
                </div>

                {announcement.details.length === 0 ? (
                  <p className="text-sm text-gray-400 italic py-4 text-center">
                    Nenhum detalhe adicionado.
                  </p>
                ) : (
                  <div className="space-y-3">
                    {announcement.details.map((detail, index) => (
                      <div
                        key={index}
                        className="flex items-start gap-3 p-3 bg-gray-50 rounded-lg border border-gray-100"
                      >
                        <div className="flex-1 grid grid-cols-2 gap-3">
                          <Input
                            label={`Ícone ${index + 1}`}
                            placeholder="Ex: 📅"
                            value={detail.icon}
                            onChange={(e) => updateDetailIcon(index, e.target.value)}
                          />
                          <Input
                            label={`Texto ${index + 1}`}
                            placeholder="Ex: 20 de Março"
                            value={detail.text}
                            onChange={(e) => updateDetailText(index, e.target.value)}
                          />
                        </div>
                        <button
                          type="button"
                          onClick={() => removeDetail(index)}
                          className="mt-6 p-2 text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                          title="Remover detalhe"
                        >
                          <XMarkIcon className="w-5 h-5" />
                        </button>
                      </div>
                    ))}
                  </div>
                )}
              </div>
            </AdminCard>
          </div>

          {/* Coluna Lateral */}
          <div className="space-y-6">
            <AdminCard
              icon={<MegaphoneIcon className="w-5 h-5 text-primary" />}
              title="Tipo de Anúncio"
            >
              <div className="space-y-4">
                <Select
                  label="Tipo"
                  value={announcement.type}
                  onChange={handleTypeChange}
                  options={[
                    { label: "Evento", value: "event" },
                    { label: "Destaque", value: "highlight" },
                  ]}
                />
              </div>
            </AdminCard>

            <ImageUploadCard
              title="Imagem do Anúncio"
              description="Clique para alterar a imagem do anúncio."
              icon={<PhotoIcon className="w-10 h-10" />}
              image={announcement.imageSrc}
              onSetImage={handleSetImage}
              onRemoveImage={handleRemoveImage}
              galleryTitle="Gerenciar Imagem do Anúncio"
              galleryDescription="Esta imagem será exibida no card do anúncio."
              showModal={true}
            />

            <AdminCard title="Texto Alternativo da Imagem">
              <Input
                label="Alt Text"
                placeholder="Descrição da imagem para acessibilidade"
                value={announcement.imageAlt}
                onChange={handleImageAltChange}
              />
            </AdminCard>
          </div>
        </div>
      </div>

      {/* Modal de Confirmação de Deleção */}
      <ConfirmationModal
        isOpen={deleteModalOpen}
        onClose={handleCloseDeleteModal}
        onConfirm={handleDelete}
        title="Excluir Anúncio"
        message={`Tem certeza que deseja excluir o anúncio "${announcement.title}"? Todos os dados associados serão removidos permanentemente.`}
        confirmLabel="Sim, Excluir"
        isLoading={isDeleting}
      />
    </div>
  );
}
