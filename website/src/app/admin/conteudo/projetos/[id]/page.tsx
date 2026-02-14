"use client";

import {
  ConfirmationModal,
  EditPageHeader,
  PageLoader,
  ProjectForm,
  Button,
} from "@/components";
import { apiService } from "@/services/api";
import { Project } from "@/types";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { toast } from "sonner";

export default function AdminEditProjetoPage() {
  const router = useRouter();
  const params = useParams();

  const [project, setProject] = useState<Project | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const projectData = await apiService.getProject(params.id as string);
        setProject(projectData);
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
    if (!project) return;
    setIsSaving(true);

    const payload = {
      title: project.title,
      description: project.description,
      status: project.status,
      technologies: project.technologies,
      completionText: project.completionText,
      progress: project.progress,
      imageUrl: project.icon,
      directorateId: project.directorate?.id,
    };

    try {
      await apiService.updateProject(project.id, payload);
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

  const handleGoBack = () => router.push("/admin/conteudo");
  const handleOpenDeleteModal = () => setDeleteModalOpen(true);
  const handleCloseDeleteModal = () => setDeleteModalOpen(false);

  if (isLoading) return <PageLoader />;
  if (!project)
    return (
      <div className="flex flex-col items-center justify-center h-[60vh]">
        <h1 className="text-2xl font-bold text-gray-400">
          Projeto não encontrado
        </h1>
        <Button onClick={handleGoBack} className="mt-4">
          Voltar para Conteúdos
        </Button>
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
              ? "bg-green-100 text-green-700 font-bold"
              : project.status === "in_progress"
                ? "bg-blue-100 text-blue-700 font-bold"
                : "bg-gray-100 text-gray-700 font-bold",
        }}
      />

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        <ProjectForm
          project={project}
          onChange={(field, value) => {
            if (!project) return;
            setProject({ ...project, [field]: value });
          }}
          mode="edit"
        />
      </div>

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
