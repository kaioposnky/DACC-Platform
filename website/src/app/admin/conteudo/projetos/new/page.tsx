"use client"

import { EditPageHeader, ProjectForm } from "@/components";
import { apiService } from "@/services/api";
import { Project } from "@/types";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { toast } from "sonner";

export default function AdminProjetosNewPage() {

  const router = useRouter();

  const [isCreating, setIsCreating] = useState(false);
  const [project, setProject] = useState<Partial<Project>>({
    title: '',
    description: '',
    icon: '',
    technologies: [],
    status: 'in_progress',
    progress: 0,
    completionText: '',
  });

  const handleGoBack = () => router.back();
  const handleCreate = () => {
    if (!project.title || !project.description) {
      toast.error('Preencha os campos obrigatórios');
      return;
    }

    const requestData = {
      title: project.title,
      description: project.description,
      status: project.status || 'in_progress',
      directorateId: project.directorate?.id || '',
      technologies: project.technologies || [],
      completionText: project.completionText || '',
      progress: project.progress || 0,
      imageUrl: project.icon || '',
    };

    const finalPayload = { ...requestData, id: '' };

    setIsCreating(true);
    apiService.createProject(finalPayload).then(() => {
      router.push('/admin/conteudo');
      toast.success('Projeto criado com sucesso!');
    }).catch((error) => {
      console.error(error);
      toast.error('Erro ao criar projeto');
    }).finally(() => {
      setIsCreating(false);
    });
  }
  const handleChange = (field: keyof Project, value: any) => {
    setProject({ ...project, [field]: value });
  }

  return (
    <div>
      <EditPageHeader
        title="Novo Projeto"
        label="Criando"
        onBack={handleGoBack}
        onSave={handleCreate}
        loadingSave={isCreating}
        saveButtonText="Criar Novo Projeto"
      />

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        <ProjectForm
          project={project}
          onChange={handleChange}
        />

      </div>
    </div>
  )
}
