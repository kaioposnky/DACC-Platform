'use client'

import { EditPageHeader, EventForm } from "@/components";
import { apiService } from "@/services/api";
import { Event, EventRequest } from "@/types";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { toast } from "sonner";

export default function AdminEventosNewPage() {

  const [event, setEvent] = useState<Partial<Event>>({
    title: '',
    description: '',
    date: '',
    time: '',
    actionText: '',
    actionLink: '',
    type: '',
  });
  const [isCreating, setIsCreating] = useState(false);
  const router = useRouter();

  const handleGoBack = () => router.back();
  const handleCreate = () => {
    setIsCreating(true);
    const combinedDate = event.time
      ? `${event.date?.split('T')[0]}T${event.time}:00`
      : event.date;

    const payload: any = {
      ...event,
      date: combinedDate,
      authorId: event.author?.id ?? "0",
      eventType: event.type ?? "0"
    };

    // Remove unsupported fields
    delete payload.time;
    delete payload.type;
    delete payload.author;
    delete payload.location;

    apiService.createEvent(payload).then(() => {
      router.push('/admin/conteudo');
      toast.success('Evento criado com sucesso!');
    }).catch((error) => {
      toast.error('Erro ao criar evento!');
      console.error(error);
    }).finally(() => {
      setIsCreating(false);
    });
  }
  const handleChange = (field: keyof Event, value: any) => {
    if (!event) return;
    setEvent({ ...event, [field]: value });
  }

  return (
    <div className="mb-10">
      <EditPageHeader
        title="Novo Evento"
        label="Criando"
        onBack={handleGoBack}
        onSave={handleCreate}
        loadingSave={isCreating}
        saveButtonText="Criar Evento"
      />

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        <EventForm
          event={event}
          onChange={handleChange}
          mode="create"
        />
      </div>
    </div>
  )
}
