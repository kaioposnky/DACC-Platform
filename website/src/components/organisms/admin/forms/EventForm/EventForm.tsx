import { AdminCard, Input, Select, DateTimeInputs } from "@/components";
import { Event } from "@/types";
import { UserIcon, CalendarIcon, IdentificationIcon, LinkIcon } from "@heroicons/react/24/outline";

interface EventFormProps {
    event: Partial<Event>;
    onChange: (field: keyof Event, value: any) => void;
    mode?: 'create' | 'edit' | 'view';
}

const eventTypes = [
    { label: 'Palestra', value: 'Palestra' },
    { label: 'Workshop', value: 'Workshop' },
    { label: 'Hackathon', value: 'Hackathon' },
    { label: 'Visita Técnica', value: 'Visita Técnica' },
    { label: 'Congresso', value: 'Congresso' },
    { label: 'Outro', value: 'Outro' },
];

export default function EventForm({
    event,
    onChange,
    mode = 'edit'
}: EventFormProps) {
    const isReadonly = mode === 'view';

    return (
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            {/* Coluna Principal */}
            <div className="lg:col-span-2 space-y-6">
                <AdminCard
                    icon={<IdentificationIcon className="w-5 h-5 text-primary" />}
                    title="Informações do Evento"
                >
                    <div className="space-y-6">
                        <Input
                            label="Título do Evento"
                            placeholder="Ex: Workshop de React para Iniciantes"
                            value={event.title || ''}
                            onChange={(e) => onChange('title', e.target.value)}
                            required
                            disabled={isReadonly}
                        />

                        <Input
                            label="Descrição"
                            placeholder="Descreva o que acontecerá no evento, palestrantes, pré-requisitos, etc."
                            value={event.description || ''}
                            onChange={(e) => onChange('description', e.target.value)}
                            multiline
                            rows={8}
                            required
                            disabled={isReadonly}
                        />

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                            <Select
                                label="Tipo de Evento"
                                value={event.type || ''}
                                onChange={(e) => onChange('type', e.target.value)}
                                options={eventTypes}
                                disabled={isReadonly}
                            />
                            <Input
                                label="Localização"
                                placeholder="Ex: Auditório K-202 ou Online"
                                value={(event as any).location || ''}
                                onChange={(e) => onChange('location' as keyof Event, e.target.value)}
                                required
                                disabled={isReadonly}
                            />
                        </div>
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
                            value={event.actionText || ''}
                            onChange={(e) => onChange('actionText', e.target.value)}
                            disabled={isReadonly}
                        />
                        <Input
                            label="Link do Botão"
                            placeholder="Ex: https://forms.gle/..."
                            value={event.actionLink || ''}
                            onChange={(e) => onChange('actionLink', e.target.value)}
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
                                dateValue={event.date || ''}
                                timeValue={event.time || ''}
                                onDateChange={(val) => onChange('date', val.target.value)}
                                onTimeChange={(val) => onChange('time', val.target.value)}
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
                        <div className="p-4 bg-gray-50 rounded-xl border border-gray-100">
                            <p className="text-xs text-gray-500">
                                O autor é gerenciado na seleção de usuários.
                            </p>
                            {event.author && (
                                <div className="mt-2 flex items-center gap-2">
                                    <div className="w-8 h-8 rounded-full bg-primary/10 flex items-center justify-center">
                                        <UserIcon className="w-4 h-4 text-primary" />
                                    </div>
                                    <div className="text-sm">
                                        <p className="font-medium text-gray-900">{event.author.name}</p>
                                        <p className="text-xs text-gray-500">{event.author.email}</p>
                                    </div>
                                </div>
                            )}
                        </div>
                    </div>
                </AdminCard>
            </div>
        </div>
    );
}
