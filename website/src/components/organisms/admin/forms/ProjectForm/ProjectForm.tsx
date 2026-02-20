'use client';

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
    DocumentTextIcon,
    ChevronDownIcon,
    PlusIcon,
    BuildingOfficeIcon
} from "@heroicons/react/24/outline";
import { useState, useEffect } from "react";
import { apiService } from "@/services/api";

interface ProjectFormProps {
    project: Partial<Project>;
    onChange: (field: keyof Project, value: any) => void;
    mode?: 'create' | 'edit' | 'view';
}

const PROJECT_STATUS_OPTIONS = [
    { label: "Em Andamento", value: "in_progress" },
    { label: "Concluído", value: "completed" },
    { label: "Planejado", value: "planned" },
];

export default function ProjectForm({
    project,
    onChange,
    mode = 'edit'
}: ProjectFormProps) {
    const isReadonly = mode === 'view';

    const [showDirectorateManager, setShowDirectorateManager] = useState(false);
    const [localDirectorates, setLocalDirectorates] = useState<Directorate[]>([]);
    const [newDirectorateName, setNewDirectorateName] = useState("");
    const [newDirectorateDescription, setNewDirectorateDescription] = useState("");
    const [isCreatingDirectorate, setIsCreatingDirectorate] = useState(false);
    const [isLoadingDirectorates, setIsLoadingDirectorates] = useState(false);

    useEffect(() => {
        const fetchDirectorates = async () => {
            try {
                setIsLoadingDirectorates(true);
                const data = await apiService.getDirectorates();
                setLocalDirectorates(data);
            } catch (error) {
                console.error("Erro ao buscar diretorias:", error);
            } finally {
                setIsLoadingDirectorates(false);
            }
        };
        fetchDirectorates();
    }, []);

    const handleCreateDirectorate = async () => {
        if (!newDirectorateName.trim()) return;
        try {
            setIsCreatingDirectorate(true);
            const newDirectorate = await apiService.createDirectorate(
                newDirectorateName,
                newDirectorateDescription || undefined
            );
            setLocalDirectorates([...localDirectorates, newDirectorate]);
            setNewDirectorateName("");
            setNewDirectorateDescription("");
        } catch (error) {
            console.error("Erro ao criar diretoria:", error);
        } finally {
            setIsCreatingDirectorate(false);
        }
    };

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
        const selectedDirectorate = localDirectorates.find(d => d.id === directorateId);
        onChange('directorate', selectedDirectorate);
    };

    const handleSetIcon = (url: string) => {
        onChange('icon', url);
    };

    const handleRemoveIcon = () => {
        onChange('icon', '');
    };

    return (
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            {/* Coluna Principal */}
            <div className="lg:col-span-2 space-y-6">
                {/* Directorate Management Card */}
                {!isReadonly && (
                    <AdminCard
                        icon={<BuildingOfficeIcon className="w-5 h-5 text-primary" />}
                        title="Gerenciar Diretorias"
                        actions={
                            <button
                                onClick={() => setShowDirectorateManager(!showDirectorateManager)}
                                className="flex items-center gap-1 text-xs font-bold text-blue-600 hover:text-blue-800 uppercase"
                            >
                                {showDirectorateManager ? "Ocultar" : "Mostrar"}
                                <ChevronDownIcon
                                    className={`w-4 h-4 transition-transform ${showDirectorateManager ? "rotate-180" : ""}`}
                                />
                            </button>
                        }
                    >
                        {showDirectorateManager && (
                            <div className="space-y-4">
                                <div className="space-y-2">
                                    <label className="block text-xs font-semibold text-gray-700 uppercase">
                                        Nova Diretoria
                                    </label>
                                    <div className="flex flex-col gap-2">
                                        <Input
                                            value={newDirectorateName}
                                            onChange={(e) => setNewDirectorateName(e.target.value)}
                                            placeholder="Nome da diretoria (ex: Diretoria de Tecnologia)"
                                            disabled={isCreatingDirectorate}
                                            onKeyDown={(e) => e.key === "Enter" && !e.shiftKey && handleCreateDirectorate()}
                                        />
                                        <Input
                                            value={newDirectorateDescription}
                                            onChange={(e) => setNewDirectorateDescription(e.target.value)}
                                            placeholder="Descrição (opcional)"
                                            disabled={isCreatingDirectorate}
                                            multiline
                                            rows={2}
                                        />
                                        <button
                                            onClick={handleCreateDirectorate}
                                            disabled={!newDirectorateName.trim() || isCreatingDirectorate}
                                            className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center gap-2 w-full"
                                        >
                                            <PlusIcon className="w-4 h-4" />
                                            {isCreatingDirectorate ? "Criando..." : "Criar Diretoria"}
                                        </button>
                                    </div>
                                </div>
                            </div>
                        )}
                    </AdminCard>
                )}

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
                            value={project.directorate?.id || ""}
                            onChange={(e) => handleDirectorateChange(e.target.value)}
                            options={[
                                { label: "Selecione uma diretoria", value: "" },
                                ...(localDirectorates || [])
                                    .filter(dir => dir && dir.name)
                                    .map(dir => ({
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
                    onSetImage={handleSetIcon}
                    onRemoveImage={handleRemoveIcon}
                    galleryTitle="Gerenciar Ícone do Projeto"
                    galleryDescription="Esta imagem será usada como o ícone principal nos cards e nos detalhes do projeto."
                    previewClassName="aspect-square w-32 h-32 mx-auto"
                    showModal={!isReadonly}
                />
            </div>
        </div>
    );
}
