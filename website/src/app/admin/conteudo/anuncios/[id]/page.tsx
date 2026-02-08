"use client";

import {
  ConfirmationModal,
  EditPageHeader,
  PageLoader,
  AnnouncementForm,
} from "@/components";
import { apiService } from "@/services/api";
import { Announcement } from "@/types";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { toast } from "sonner";

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

  const handleSetImage = (url: string) => {
    if (!announcement) return;
    setAnnouncement({ ...announcement, imageSrc: url });
  };

  const handleRemoveImage = () => {
    if (!announcement) return;
    setAnnouncement({ ...announcement, imageSrc: "" });
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
        <AnnouncementForm
          announcement={announcement}
          onChange={(field, value) => {
            if (!announcement) return;
            setAnnouncement({ ...announcement, [field]: value });
          }}
          onImageChange={handleSetImage}
          onImageRemove={handleRemoveImage}
          mode="edit"
        />
      </div>

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
