"use client";

import {
  ConfirmationModal,
  EditPageHeader,
  PageLoader,
  EventForm,
  Button,
} from "@/components";
import { apiService } from "@/services/api";
import { Event, User } from "@/types";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { toast } from "sonner";

export default function AdminEditEventoPage() {
  const router = useRouter();
  const params = useParams();

  const [event, setEvent] = useState<Event | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const eventResponse = await apiService.getEvent(params.id as string);

        const eventData = eventResponse as any;
        if (eventData.autor) {
          eventResponse.author = {
            id: eventData.autor.id,
            name: eventData.autor.nome,
            lastName: eventData.autor.sobrenome,
            email: "",
            avatar: "",
            role: "administrador"
          } as User;
        }

        setEvent(eventResponse);
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
    if (!event) return;
    setIsSaving(true);
    try {
      let formattedDate = event.date;
      if (event.date && event.time) {
        if (!event.date.includes('T')) {
          formattedDate = `${event.date}T${event.time}:00Z`;
        } else if (!event.date.endsWith('Z')) {
          formattedDate = event.date.endsWith('Z') ? event.date : `${event.date.split('.')[0]}Z`;
        }
      } else if (event.date && !event.date.endsWith('Z')) {
        formattedDate = `${event.date.split('T')[0]}T00:00:00Z`;
      }

      const payload = {
        id: event.id,
        title: event.title,
        description: event.description,
        date: formattedDate,
        actionText: event.actionText,
        actionLink: event.actionLink,
        eventType: event.type,
        authorId: event.author?.id
      };

      await apiService.updateEvent(event.id, payload as any);
      toast.success("Evento salvo com sucesso!");
    } catch (error) {
      console.error(error);
      toast.error("Erro ao salvar evento");
    } finally {
      setIsSaving(false);
    }
  };

  const handleDelete = async () => {
    if (!event) return;
    setIsDeleting(true);
    try {
      await apiService.deleteEvent(event.id);
      toast.success("Evento excluído com sucesso!");
      router.push("/admin/conteudo");
    } catch (error) {
      console.error(error);
      toast.error("Erro ao excluir evento");
    } finally {
      setIsDeleting(false);
      setDeleteModalOpen(false);
    }
  };

  const handleGoBack = () => router.push("/admin/conteudo");
  const handleOpenDeleteModal = () => setDeleteModalOpen(true);
  const handleCloseDeleteModal = () => setDeleteModalOpen(false);

  if (isLoading) return <PageLoader />;
  if (!event)
    return (
      <div className="flex flex-col items-center justify-center h-[60vh]">
        <h1 className="text-2xl font-bold text-gray-500">
          Evento não encontrado
        </h1>
        <Button onClick={handleGoBack} className="mt-4">
          Voltar para Conteúdos
        </Button>
      </div>
    );

  return (
    <div className="pb-20">
      <EditPageHeader
        title="Editar Evento"
        id={event.id}
        onBack={handleGoBack}
        onSave={handleSave}
        onDelete={handleOpenDeleteModal}
        showDelete={true}
        loadingSave={isSaving}
        loadingDelete={isDeleting}
        status={{
          text: event.type,
          colorClass: "bg-purple-100 text-purple-700 font-bold",
        }}
      />

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        <EventForm
          event={event}
          onChange={(field, value) => {
            if (!event) return;
            setEvent({ ...event, [field]: value });
          }}
          mode="edit"
        />
      </div>

      <ConfirmationModal
        isOpen={deleteModalOpen}
        onClose={handleCloseDeleteModal}
        onConfirm={handleDelete}
        title="Excluir Evento"
        message={`Tem certeza que deseja excluir o evento "${event.title}"? Esta ação não pode ser desfeita.`}
        confirmLabel="Sim, Excluir"
        isLoading={isDeleting}
      />
    </div>
  );
}
