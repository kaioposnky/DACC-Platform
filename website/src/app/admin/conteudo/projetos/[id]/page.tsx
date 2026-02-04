"use client";

import {
  ConfirmationModal,
  EditPageHeader,
  PageLoader,
  AdminCard,
  Input,
  Select,
  TagInput,
  ImageUploadCard,
  ProgressSlider,
} from "@/components";
import { apiService } from "@/services/api";
import { Project } from "@/types";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { toast } from "sonner";
import {
  RocketLaunchIcon,
  CommandLineIcon,
  ChartBarIcon,
  PhotoIcon,
  DocumentTextIcon,
  TagIcon,
  XMarkIcon,
} from "@heroicons/react/24/outline";

export default function AdminEditProjetoPage() {
  const router = useRouter();
  const params = useParams();

  const [project, setProject] = useState<Project | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);

  useEffect(() => {
    const fetchProject = async () => {
      try {
        const response = await apiService.getProject(params.id as string);
        setProject(response);
      } catch (error) {
        console.error(error);
        toast.error("Erro ao carregar projeto");
      } finally {
        setIsLoading(false);
      }
    };
    fetchProject();
  }, [params.id]);

  const handleSave = async () => {
    if (!project) return;
    setIsSaving(true);
    try {
      await apiService.updateProject(project.id, project);
      toast.success("Projeto salvo com sucesso!");
    } catch (error) {
      console.error(error);
      toast.error("Erro ao salvar projeto");
    } finally {
      setIsSaving(false);
    }
  };

  const handleDelete = async () => {
    if (!project) return;
    setIsDeleting(true);
    try {
      await apiService.deleteProject(project.id);
      toast.success("Projeto excluído com sucesso!");
      router.push("/admin/conteudo");
    } catch (error) {
      console.error(error);
      toast.error("Erro ao excluir projeto");
    } finally {
      setIsDeleting(false);
      setDeleteModalOpen(false);
    }
  };

  const addTech = (tech: string) => {
    if (tech.trim() && project) {
      if (!project.technologies.includes(tech.trim())) {
        setProject({
          ...project,
          technologies: [...project.technologies, tech.trim()],
        });
      }
    }
  };

  const removeTech = (tech: string) => {
    if (project) {
      setProject({
        ...project,
        technologies: project.technologies.filter((t) => t !== tech),
      });
    }
  };

  const handleGoBack = () => router.push("/admin/conteudo");
  const handleOpenDeleteModal = () => setDeleteModalOpen(true);
  const handleCloseDeleteModal = () => setDeleteModalOpen(false);

  const handleTitleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!project) return;
    setProject({ ...project, title: e.target.value });
  };

  const handleDescriptionChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!project) return;
    setProject({ ...project, description: e.target.value });
  };

  const handleCompletionTextChange = (
    e: React.ChangeEvent<HTMLInputElement>,
  ) => {
    if (!project) return;
    setProject({ ...project, completionText: e.target.value });
  };

  const handleStatusChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    if (!project) return;
    setProject({ ...project, status: e.target.value as any });
  };

  const handleProgressChange = (value: number) => {
    if (!project) return;
    setProject({ ...project, progress: value });
  };

  const handleSetIcon = (url: string) => {
    if (!project) return;
    setProject({ ...project, icon: url });
  };

  const handleRemoveIcon = () => {
    if (!project) return;
    setProject({ ...project, icon: "" });
  };

  if (isLoading) return <PageLoader />;
  if (!project)
    return (
      <div className="flex flex-col items-center justify-center h-[60vh]">
        <h1 className="text-2xl font-bold text-gray-400">
          Projeto não encontrado
        </h1>
      </div>
    );

  return (
    <div className="pb-20">
      <EditPageHeader
        title="Editar Projeto"
        id={project.id}
        onBack={handleGoBack}
        onSave={handleSave}
        onDelete={handleOpenDeleteModal}
        showDelete={true}
        loadingSave={isSaving}
        loadingDelete={isDeleting}
        status={{
          text:
            project.status === "completed"
              ? "Concluído"
              : project.status === "in_progress"
                ? "Em Andamento"
                : "Planejado",
          colorClass:
            project.status === "completed"
              ? "bg-green-100 text-green-700"
              : project.status === "in_progress"
                ? "bg-blue-100 text-blue-700"
                : "bg-gray-100 text-gray-700",
        }}
      />

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
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
                  value={project.title}
                  onChange={handleTitleChange}
                  className="text-lg font-bold"
                />

                <Input
                  label="Descrição"
                  placeholder="Descreva os objetivos e funcionalidades do projeto..."
                  value={project.description}
                  onChange={handleDescriptionChange}
                  multiline={true}
                  rows={6}
                />

                <Input
                  label="Texto de Conclusão (Opcional)"
                  placeholder="Ex: Lançado em Dezembro de 2023"
                  value={project.completionText || ""}
                  onChange={handleCompletionTextChange}
                />
              </div>
            </AdminCard>

            <AdminCard
              icon={<CommandLineIcon className="w-5 h-5 text-primary" />}
              title="Tecnologias Utilizadas"
            >
              <div className="space-y-4">
                <div className="flex gap-2">
                  <div className="flex-1">
                    <TagInput
                      label="Adicionar tecnologia (ex: React, C#)"
                      tags={project.technologies || []}
                      onAddTag={(tech) => addTech(tech)}
                      onRemoveTag={(tech) => removeTech(tech)}
                    />
                  </div>
                  {project.technologies.length === 0 && (
                    <p className="text-sm text-gray-400 italic">
                      Nenhuma tecnologia adicionada.
                    </p>
                  )}
                </div>
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
                  value={project.status}
                  onChange={handleStatusChange}
                  options={[
                    { label: "Em Andamento", value: "in_progress" },
                    { label: "Concluído", value: "completed" },
                    { label: "Planejado", value: "planned" },
                  ]}
                />

                <ProgressSlider
                  label="Progresso"
                  value={project.progress}
                  onChange={handleProgressChange}
                  showInput={true}
                />
              </div>
            </AdminCard>


            <ImageUploadCard
              title="Ícone / Capa"
              description="Clique para alterar a imagem principal do projeto."
              icon={<RocketLaunchIcon className="w-10 h-10" />}
              image={project.icon}
              onSetImage={handleSetIcon}
              onRemoveImage={handleRemoveIcon}
              galleryTitle="Gerenciar Ícone do Projeto"
              galleryDescription="Esta imagem será usada como o ícone principal nos cards e nos detalhes do projeto."
              previewClassName="aspect-square w-32 h-32"
              showModal={true}
            />

          </div>
        </div>
      </div>

      {/* Modal de Confirmação de Deleção */}
      <ConfirmationModal
        isOpen={deleteModalOpen}
        onClose={handleCloseDeleteModal}
        onConfirm={handleDelete}
        title="Excluir Projeto"
        message={`Tem certeza que deseja excluir o projeto "${project.title}"? Todos os dados associados serão removidos permanentemente.`}
        confirmLabel="Sim, Excluir"
        isLoading={isDeleting}
      />
    </div>
  );
}
