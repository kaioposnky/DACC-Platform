import { AdminCard, Input, Select, DateTimeInputs } from "@/components";
import { Event, User, EventType } from "@/types";
import {
  UserIcon,
  CalendarIcon,
  IdentificationIcon,
  LinkIcon,
  Square3Stack3DIcon,
  ChevronDownIcon,
  PlusIcon,
} from "@heroicons/react/24/outline";
import { useState, useEffect } from "react";
import { apiService } from "@/services/api";
import { toast } from "sonner";

interface EventFormProps {
  event: Partial<Event>;
  onChange: (field: keyof Event, value: any) => void;
  mode?: "create" | "edit" | "view";
}

export default function EventForm({
  event,
  onChange,
  mode = "edit",
}: EventFormProps) {
  const isReadonly = mode === "view";

  const [showTypeManager, setShowTypeManager] = useState(false);
  const [localEventTypes, setLocalEventTypes] = useState<EventType[]>([]);
  const [users, setUsers] = useState<User[]>([]);
  const [newTypeName, setNewTypeName] = useState("");
  const [isCreatingType, setIsCreatingType] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [typesData, usersData] = await Promise.all([
          apiService.getEventTypes(),
          apiService.getUsers(),
        ]);
        setLocalEventTypes(typesData);
        setUsers(usersData);
      } catch (error) {
        console.error("Erro ao buscar dados:", error);
      }
    };
    fetchData();
  }, []);

  const handleCreateType = async () => {
    if (!newTypeName.trim()) return;
    try {
      setIsCreatingType(true);
      const newType = await apiService.createEventType(newTypeName);
      setLocalEventTypes([...localEventTypes, newType]);
      setNewTypeName("");
      toast.success("Tipo de evento criado com sucesso!");
    } catch (error) {
      console.error("Erro ao criar tipo de evento:", error);
      toast.error(`${error}`);
    } finally {
      setIsCreatingType(false);
    }
  };

  const handleAuthorChange = (authorId: string) => {
    const selectedAuthor = users.find((u) => u.id === authorId);
    onChange("author", selectedAuthor || undefined);
  };

  return (
    <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
      {/* Coluna Principal */}
      <div className="lg:col-span-2 space-y-6">
        {/* Event Type Management Card */}
        {!isReadonly && (
          <AdminCard
            icon={<Square3Stack3DIcon className="w-5 h-5 text-primary" />}
            title="Gerenciar Tipos de Evento"
            actions={
              <button
                onClick={() => setShowTypeManager(!showTypeManager)}
                className="flex items-center gap-1 text-xs font-bold text-blue-600 hover:text-blue-800 uppercase"
              >
                {showTypeManager ? "Ocultar" : "Mostrar"}
                <ChevronDownIcon
                  className={`w-4 h-4 transition-transform ${showTypeManager ? "rotate-180" : ""}`}
                />
              </button>
            }
          >
            {showTypeManager && (
              <div className="space-y-4">
                <div className="space-y-2">
                  <label className="block text-xs font-semibold text-gray-700 uppercase">
                    Novo Tipo de Evento
                  </label>
                  <div className="flex flex-col gap-2">
                    <Input
                      value={newTypeName}
                      onChange={(e) => setNewTypeName(e.target.value)}
                      placeholder="Nome do tipo (ex: Workshop, Palestra)"
                      disabled={isCreatingType}
                      onKeyDown={(e) =>
                        e.key === "Enter" && !e.shiftKey && handleCreateType()
                      }
                    />
                    <button
                      onClick={handleCreateType}
                      disabled={!newTypeName.trim() || isCreatingType}
                      className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center gap-2 w-full"
                    >
                      <PlusIcon className="w-4 h-4" />
                      {isCreatingType ? "Criando..." : "Criar Tipo"}
                    </button>
                  </div>
                </div>
              </div>
            )}
          </AdminCard>
        )}

        <AdminCard
          icon={<IdentificationIcon className="w-5 h-5 text-primary" />}
          title="Informações do Evento"
        >
          <div className="space-y-6">
            <Input
              label="Título do Evento"
              placeholder="Ex: Workshop de React para Iniciantes"
              value={event.title || ""}
              onChange={(e) => onChange("title", e.target.value)}
              required
              disabled={isReadonly}
            />

            <Input
              label="Descrição"
              placeholder="Descreva o que acontecerá no evento, palestrantes, pré-requisitos, etc."
              value={event.description || ""}
              onChange={(e) => onChange("description", e.target.value)}
              multiline
              rows={8}
              required
              disabled={isReadonly}
            />

            <Select
              label="Tipo de Evento"
              value={event.type || ""}
              onChange={(e) => onChange("type", e.target.value)}
              options={[
                { label: "Selecione um tipo", value: "" },
                ...localEventTypes.map((t) => ({
                  label: t.name.charAt(0).toUpperCase() + t.name.slice(1),
                  value: t.name,
                })),
              ]}
              disabled={isReadonly}
            />
          </div>
        </AdminCard>

        <AdminCard
          icon={<LinkIcon className="w-5 h-5 text-primary" />}
          title="Links e Ações"
        >
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <Input
              label="Texto do Botão"
              placeholder="Ex: Inscrever-se"
              value={event.actionText || ""}
              onChange={(e) => onChange("actionText", e.target.value)}
              disabled={isReadonly}
            />
            <Input
              label="Link do Botão"
              placeholder="Ex: https://forms.gle/..."
              value={event.actionLink || ""}
              onChange={(e) => onChange("actionLink", e.target.value)}
              disabled={isReadonly}
            />
          </div>
        </AdminCard>
      </div>

      {/* Coluna Lateral */}
      <div className="space-y-6">
        <AdminCard
          icon={<CalendarIcon className="w-5 h-5 text-primary" />}
          title="Data e Horário"
        >
          <div className="space-y-4">
            <div className={isReadonly ? "pointer-events-none opacity-80" : ""}>
              <DateTimeInputs
                dateValue={event.date || ""}
                timeValue={event.time || ""}
                onDateChange={(val) => onChange("date", val.target.value)}
                onTimeChange={(val) => onChange("time", val.target.value)}
              />
            </div>
            {!isReadonly && (
              <p className="text-xs text-gray-500 italic mt-2 text-center">
                Certifique-se de que a data e o horário estão corretos.
              </p>
            )}
          </div>
        </AdminCard>

        <AdminCard
          icon={<UserIcon className="w-5 h-5 text-primary" />}
          title="Organização"
        >
          <div className="space-y-4">
            <Select
              label="Organizador / Autor"
              value={event.author?.id || ""}
              onChange={(e) => handleAuthorChange(e.target.value)}
              options={[
                { label: "Selecione um organizador", value: "" },
                ...users.map((user) => ({
                  label:
                    `${user.name} ${user.lastName || ""} ${user.ra ? `(RA: ${user.ra})` : ""}`.trim(),
                  value: user.id,
                })),
              ]}
              disabled={isReadonly}
            />
          </div>
        </AdminCard>
      </div>
    </div>
  );
}
