"use client";

import {
  ConfirmationModal,
  EditPageHeader,
  ImageGalleryEditor,
  PageLoader,
  AdminCard,
  Input,
  Select,
  Button,
  DateTimeInputs,
} from "@/components";
import { apiService } from "@/services/api";
import { Event, User } from "@/types";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { toast } from "sonner";
import {
  CalendarIcon,
  ClockIcon,
  MapPinIcon,
  DocumentTextIcon,
  InformationCircleIcon,
  LinkIcon,
  UserIcon,
} from "@heroicons/react/24/outline";

export default function AdminEditEventoPage() {
  const router = useRouter();
  const params = useParams();

  const [event, setEvent] = useState<Event | null>(null);
  const [users, setUsers] = useState<User[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [eventResponse, usersResponse] = await Promise.all([
          apiService.getEvent(params.id as string),
          apiService.getUsers(),
        ]);

        // Map backend 'autor' (Portuguese) to 'author' (English/User interface)
        const eventData = eventResponse as any;
        if (eventData.autor) {
          eventResponse.author = {
            id: eventData.autor.id,
            name: eventData.autor.nome,
            lastName: eventData.autor.sobrenome,
            email: "", // Placeholder
            avatar: "", // Placeholder
            role: "administrador" // Placeholder
          } as User;
        }

        setEvent(eventResponse);
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
    if (!event) return;
    setIsSaving(true);
    try {
      // Combine date and time to ensure backend receives full DateTime in UTC
      // Input date is YYYY-MM-DD, input time is HH:mm
      // We construct YYYY-MM-DDTHH:mm:00Z (Z = UTC) for PostgreSQL compatibility
      let formattedDate = event.date;
      if (event.date && event.time) {
        // Se já não tiver T (formato ISO), combina
        if (!event.date.includes('T')) {
          formattedDate = `${event.date}T${event.time}:00Z`;
        } else if (!event.date.endsWith('Z')) {
          // Já tem T mas não tem Z, adiciona
          formattedDate = event.date.endsWith('Z') ? event.date : `${event.date.split('.')[0]}Z`;
        }
      } else if (event.date && !event.date.endsWith('Z')) {
        formattedDate = `${event.date.split('T')[0]}T00:00:00Z`;
      }

      // Create payload with only necessary fields (exclude author object to avoid nested date issues)
      const payload = {
        id: event.id,
        title: event.title,
        description: event.description,
        date: formattedDate,
        time: event.time,
        actionText: event.actionText,
        actionLink: event.actionLink,
        type: event.type,
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

  const handleTitleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!event) return;
    setEvent({ ...event, title: e.target.value });
  };

  const handleDescriptionChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!event) return;
    setEvent({ ...event, description: e.target.value });
  };

  const handleActionTextChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!event) return;
    setEvent({ ...event, actionText: e.target.value });
  };

  const handleActionLinkChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!event) return;
    setEvent({ ...event, actionLink: e.target.value });
  };

  const handleDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!event) return;
    setEvent({ ...event, date: e.target.value });
  };

  const handleTimeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!event) return;
    setEvent({ ...event, time: e.target.value });
  };

  const handleTypeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    if (!event) return;
    setEvent({ ...event, type: e.target.value });
  };

  const handleLocationChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!event) return;
    setEvent({ ...event, location: e.target.value } as any);
  };

  const handleAuthorChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    if (!event) return;
    const selectedAuthorId = e.target.value;
    const selectedAuthor = users.find(u => u.id === selectedAuthorId);

    if (selectedAuthor) {
      setEvent({ ...event, author: selectedAuthor });
    } else {
      const { author, ...rest } = event;
      setEvent(rest as Event);
    }
  };

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
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Coluna Principal */}
          <div className="lg:col-span-2 space-y-6">
            <AdminCard
              icon={<DocumentTextIcon className="w-5 h-5 text-primary" />}
              title="Informações Gerais"
            >
              <div className="space-y-6">
                <Input
                  label="Título do Evento"
                  placeholder="Ex: Workshop de Robótica"
                  value={event.title}
                  onChange={handleTitleChange}
                  className="text-lg font-bold"
                />

                <Input
                  label="Descrição"
                  placeholder="Detalhes sobre o evento, o que será abordado, etc..."
                  value={event.description}
                  onChange={handleDescriptionChange}
                  multiline={true}
                  rows={10}
                />
              </div>
            </AdminCard>

            <AdminCard
              icon={<InformationCircleIcon className="w-5 h-5 text-primary" />}
              title="Chamada para Ação (Botão)"
            >
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <Input
                  label="Texto do Botão"
                  placeholder="Ex: Inscrever-se"
                  value={event.actionText}
                  onChange={handleActionTextChange}
                />
                <Input
                  label="Link do Botão"
                  placeholder="Ex: https://forms.gle/..."
                  value={event.actionLink}
                  onChange={handleActionLinkChange}
                />
              </div>
            </AdminCard>
          </div>

          {/* Coluna Lateral */}
          <div className="space-y-6">
            <AdminCard
              icon={<CalendarIcon className="w-5 h-5 text-primary" />}
              title="Data e Hora"
            >
              <div className="space-y-4">
                <DateTimeInputs
                  dateValue={event.date}
                  timeValue={event.time}
                  onDateChange={handleDateChange}
                  onTimeChange={handleTimeChange}
                />
                <Select
                  label="Tipo de Evento"
                  value={event.type}
                  onChange={handleTypeChange}
                  options={[
                    { label: "Workshop", value: "Workshop" },
                    { label: "Palestra", value: "Palestra" },
                    { label: "Hackathon", value: "Hackathon" },
                    { label: "Social", value: "Social" },
                    { label: "Outros", value: "Outros" },
                  ]}
                />
              </div>
            </AdminCard>

            <AdminCard
              icon={<MapPinIcon className="w-5 h-5 text-primary" />}
              title="Localização"
            >
              <Input
                label="Onde ocorrerá?"
                placeholder="Ex: Auditório Central, Virtual, etc."
                value={(event as any).location || ""}
                onChange={handleLocationChange}
              />
            </AdminCard>

            <AdminCard
              icon={<UserIcon className="w-5 h-5 text-primary" />}
              title="Responsável"
            >
              <Select
                label="Autor/Responsável"
                value={event.author?.id || ""}
                onChange={handleAuthorChange}
                options={[
                  { label: "Selecione um responsável", value: "" },
                  ...users.map((user) => ({
                    label:
                      `${user.name} ${user.lastName || ""} ${user.ra ? `(RA: ${user.ra})` : ""}`.trim(),
                    value: user.id,
                  })),
                ]}
              />
            </AdminCard>
          </div>
        </div>
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
