import {
    AdminCard,
    Input,
    Select,
    TagInput,
    ImageUploadCard,
    ProgressSlider
} from "@/components";
import { Project, Directorate } from "@/types";
import {
    RocketLaunchIcon,
    CommandLineIcon,
    ChartBarIcon,
    DocumentTextIcon
} from "@heroicons/react/24/outline";

interface ProjectFormProps {
    project: Partial<Project>;
    directorates: Directorate[];
    onChange: (field: keyof Project, value: any) => void;
    onImageChange: (url: string) => void;
    onImageRemove: () => void;
    mode?: 'create' | 'edit' | 'view';
}

const PROJECT_STATUS_OPTIONS = [
    { label: "Em Andamento", value: "in_progress" },
    { label: "Concluído", value: "completed" },
    { label: "Planejado", value: "planned" },
];

export default function ProjectForm({
    project,
    directorates,
    onChange,
    onImageChange,
    onImageRemove,
    mode = 'edit'
}: ProjectFormProps) {
    const isReadonly = mode === 'view';

    const handleAddTech = (tech: string) => {
        const technologies = project.technologies || [];
        if (!technologies.includes(tech.trim())) {
            onChange('technologies', [...technologies, tech.trim()]);
        }
    };

    const handleRemoveTech = (tech: string) => {
        const technologies = project.technologies || [];
        onChange('technologies', technologies.filter((t) => t !== tech));
    };

    const handleDirectorateChange = (directorateId: string) => {
        const selectedDirectorate = directorates.find(d => d.id === directorateId);
        onChange('department', selectedDirectorate);
    };

    return (
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            {/* Coluna Principal */}
            <div className="lg:col-span-2 space-y-6">
                <AdminCard
                    icon={<DocumentTextIcon className="w-5 h-5 text-primary" />}
                    title="Informações do Projeto"
                >
                    <div className="space-y-6">
                        <Input
                            label="Título do Projeto"
                            placeholder="Ex: Hytale Server Dashboard"
                            value={project.title || ''}
                            onChange={(e) => onChange('title', e.target.value)}
                            className="text-lg font-bold"
                            required
                            disabled={isReadonly}
                        />

                        <Input
                            label="Descrição"
                            placeholder="Descreva os objetivos e funcionalidades do projeto..."
                            value={project.description || ''}
                            onChange={(e) => onChange('description', e.target.value)}
                            multiline
                            rows={6}
                            required
                            disabled={isReadonly}
                        />

                        <Input
                            label="Texto de Conclusão (Opcional)"
                            placeholder="Ex: Lançado em Dezembro de 2023"
                            value={project.completionText || ""}
                            onChange={(e) => onChange('completionText', e.target.value)}
                            disabled={isReadonly}
                        />
                    </div>
                </AdminCard>

                <AdminCard
                    icon={<CommandLineIcon className="w-5 h-5 text-primary" />}
                    title="Tecnologias Utilizadas"
                >
                    <div className="space-y-4">
                        <TagInput
                            label="Adicionar tecnologia (ex: React, C#)"
                            tags={project.technologies || []}
                            onAddTag={handleAddTech}
                            onRemoveTag={handleRemoveTech}
                            disabled={isReadonly}
                        />
                        {(!project.technologies || project.technologies.length === 0) && (
                            <p className="text-sm text-gray-400 italic">
                                Nenhuma tecnologia adicionada.
                            </p>
                        )}
                    </div>
                </AdminCard>
            </div>

            {/* Coluna Lateral */}
            <div className="space-y-6">
                <AdminCard
                    icon={<ChartBarIcon className="w-5 h-5 text-primary" />}
                    title="Status e Progresso"
                >
                    <div className="space-y-6">
                        <Select
                            label="Status Atual"
                            value={project.status || 'planned'}
                            onChange={(e) => onChange('status', e.target.value)}
                            options={PROJECT_STATUS_OPTIONS}
                            disabled={isReadonly}
                        />

                        <Select
                            label="Diretoria Responsável"
                            value={project.department?.id || ""}
                            onChange={(e) => handleDirectorateChange(e.target.value)}
                            options={[
                                { label: "Selecione uma diretoria", value: "" },
                                ...directorates.map(dir => ({
                                    label: dir.name,
                                    value: dir.id
                                }))
                            ]}
                            disabled={isReadonly}
                        />

                        <div className={isReadonly ? "pointer-events-none opacity-80" : ""}>
                            <ProgressSlider
                                label="Progresso"
                                value={project.progress || 0}
                                onChange={(val) => onChange('progress', val)}
                                showInput={!isReadonly}
                            />
                        </div>
                    </div>
                </AdminCard>

                <ImageUploadCard
                    title="Ícone / Capa"
                    description={isReadonly ? "Imagem principal do projeto." : "Clique para alterar a imagem principal."}
                    icon={<RocketLaunchIcon className="w-10 h-10" />}
                    image={project.icon || ''}
                    onSetImage={onImageChange}
                    onRemoveImage={onImageRemove}
                    galleryTitle="Gerenciar Ícone do Projeto"
                    galleryDescription="Esta imagem será usada como o ícone principal nos cards e nos detalhes do projeto."
                    previewClassName="aspect-square w-32 h-32 mx-auto"
                    showModal={!isReadonly}
                />
            </div>
        </div>
    );
}
