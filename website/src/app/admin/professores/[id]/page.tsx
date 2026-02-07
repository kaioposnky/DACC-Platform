"use client";

import {
  ConfirmationModal,
  EditPageHeader,
  PageLoader,
  AdminCard,
  Input,
  ImageUploadCard,
} from "@/components";
import { apiService } from "@/services/api";
import { Faculty } from "@/types";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { toast } from "sonner";
import {
  UserIcon,
  AcademicCapIcon,
  ShareIcon,
  IdentificationIcon,
} from "@heroicons/react/24/outline";

export default function AdminEditFacultyPage() {
  const router = useRouter();
  const params = useParams();

  const [faculty, setFaculty] = useState<Faculty | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const facultyData = await apiService.getFacultyMember(
          params.id as string,
        );
        setFaculty(facultyData);
      } catch (error) {
        console.error(error);
        toast.error("Erro ao carregar dados do professor");
      } finally {
        setIsLoading(false);
      }
    };
    fetchData();
  }, [params.id]);

  const handleSave = async () => {
    if (!faculty) return;
    setIsSaving(true);
    try {
      await apiService.updateFacultyMember(faculty.id, faculty);
      toast.success("Professor salvo com sucesso!");
    } catch (error) {
      console.error(error);
      toast.error("Erro ao salvar professor");
    } finally {
      setIsSaving(false);
    }
  };

  const handleDelete = async () => {
    if (!faculty) return;
    setIsDeleting(true);
    try {
      await apiService.deleteFacultyMember(faculty.id);
      toast.success("Professor excluído com sucesso!");
      router.push("/admin/professores");
    } catch (error) {
      console.error(error);
      toast.error("Erro ao excluir professor");
    } finally {
      setIsDeleting(false);
      setDeleteModalOpen(false);
    }
  };

  const handleGoBack = () => router.push("/admin/professores");
  const handleOpenDeleteModal = () => setDeleteModalOpen(true);
  const handleCloseDeleteModal = () => setDeleteModalOpen(false);

  // Field change handlers
  const handleNameChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!faculty) return;
    setFaculty({ ...faculty, name: e.target.value });
  };

  const handleTitleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!faculty) return;
    setFaculty({ ...faculty, title: e.target.value });
  };

  const handlePositionChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!faculty) return;
    setFaculty({ ...faculty, position: e.target.value });
  };

  const handleSpecializationChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
  ) => {
    if (!faculty) return;
    setFaculty({ ...faculty, specialization: e.target.value });
  };

  const handleSocialChange = (
    platform: keyof Faculty["social"],
    value: string,
  ) => {
    if (!faculty) return;
    setFaculty({
      ...faculty,
      social: {
        ...faculty.social,
        [platform]: value,
      },
    });
  };

  const handleSetImage = (url: string) => {
    if (!faculty) return;
    setFaculty({ ...faculty, imageUrl: url });
  };

  const handleRemoveImage = () => {
    if (!faculty) return;
    setFaculty({ ...faculty, imageUrl: "" });
  };

  if (isLoading) return <PageLoader />;
  if (!faculty)
    return (
      <div className="flex flex-col items-center justify-center h-[60vh]">
        <h1 className="text-2xl font-bold text-gray-400">
          Professor não encontrado
        </h1>
      </div>
    );

  return (
    <div className="pb-20">
      <EditPageHeader
        title="Editar Professor"
        id={faculty.id}
        onBack={handleGoBack}
        onSave={handleSave}
        onDelete={handleOpenDeleteModal}
        showDelete={true}
        loadingSave={isSaving}
        loadingDelete={isDeleting}
        status={{
          text: faculty.position || "Professor",
          colorClass: "bg-blue-100 text-blue-700",
        }}
      />

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Coluna Principal */}
          <div className="lg:col-span-2 space-y-6">
            <AdminCard
              icon={<IdentificationIcon className="w-5 h-5 text-primary" />}
              title="Informações Profissionais"
            >
              <div className="space-y-6">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <Input
                    label="Nome Completo"
                    placeholder="Ex: João Silva"
                    value={faculty.name}
                    onChange={handleNameChange}
                    required
                  />
                  <Input
                    label="Título"
                    placeholder="Ex: Prof. Dr."
                    value={faculty.title}
                    onChange={handleTitleChange}
                  />
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <Input
                    label="Cargo"
                    placeholder="Ex: Professor Titular"
                    value={faculty.position}
                    onChange={handlePositionChange}
                  />
                  <Input
                    label="Especialização"
                    placeholder="Ex: Inteligência Artificial"
                    value={faculty.specialization}
                    onChange={handleSpecializationChange}
                  />
                </div>
              </div>
            </AdminCard>

            <AdminCard
              icon={<ShareIcon className="w-5 h-5 text-primary" />}
              title="Redes Sociais e Contato"
            >
              <div className="space-y-6">
                <Input
                  label="Email Acadêmico"
                  placeholder="Ex: joao.silva@fei.edu.br"
                  value={faculty.social.email || ""}
                  onChange={(e) => handleSocialChange("email", e.target.value)}
                />
                <Input
                  label="LinkedIn (URL)"
                  placeholder="Ex: https://linkedin.com/in/joaosilva"
                  value={faculty.social.linkedin || ""}
                  onChange={(e) =>
                    handleSocialChange("linkedin", e.target.value)
                  }
                />
                <Input
                  label="GitHub (URL)"
                  placeholder="Ex: https://github.com/joaosilva"
                  value={faculty.social.github || ""}
                  onChange={(e) => handleSocialChange("github", e.target.value)}
                />
              </div>
            </AdminCard>
          </div>

          {/* Coluna Lateral */}
          <div className="space-y-6">
            <ImageUploadCard
              title="Foto do Professor"
              description="Clique para alterar a foto de perfil."
              icon={<UserIcon className="w-10 h-10" />}
              image={faculty.imageUrl}
              onSetImage={handleSetImage}
              onRemoveImage={handleRemoveImage}
              galleryTitle="Gerenciar Foto do Professor"
              galleryDescription="Esta imagem será exibida no card do professor."
              previewClassName="aspect-square w-full rounded-2xl object-cover"
              showModal={true}
            />

            <AdminCard
              title="Informações Acadêmicas"
              icon={<AcademicCapIcon className="w-5 h-5 text-primary" />}
            >
              <div className="space-y-4 text-sm text-gray-500">
                <p>
                  Certifique-se de que o título e a especialização estão
                  atualizados conforme o currículo Lattes.
                </p>
                <div className="pt-4 border-t border-gray-100 italic">
                  Última atualização:{" "}
                  {faculty.updatedAt
                    ? new Date(faculty.updatedAt).toLocaleDateString("pt-BR")
                    : "N/A"}
                </div>
              </div>
            </AdminCard>
          </div>
        </div>
      </div>

      <ConfirmationModal
        isOpen={deleteModalOpen}
        onClose={handleCloseDeleteModal}
        onConfirm={handleDelete}
        title="Excluir Professor"
        message={`Tem certeza que deseja excluir o professor "${faculty.title} ${faculty.name}"? Esta ação não pode ser desfeita.`}
        confirmLabel="Sim, Excluir"
        isLoading={isDeleting}
      />
    </div>
  );
}
