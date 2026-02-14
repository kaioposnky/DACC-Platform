"use client";

import {
  ConfirmationModal,
  EditPageHeader,
  PageLoader,
  FacultyForm,
} from "@/components";
import { apiService } from "@/services/api";
import { Faculty, User } from "@/types";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { toast } from "sonner";

export default function AdminEditFacultyPage() {
  const router = useRouter();
  const params = useParams();

  const [users, setUsers] = useState<User[]>([]);
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
      }
    };

    const fetchUsers = async () => {
      try {
        const response = await apiService.getUsers();
        setUsers(response);
      } catch (error) {
        toast.error('Erro ao buscar usuários! ' + error);
        console.error(error);
      }
    }

    Promise.all([fetchData(), fetchUsers()]).then(() => {
      setIsLoading(false);
    });
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

  const handleFieldChange = (field: keyof Faculty, value: any) => {
    if (!faculty) return;
    setFaculty({ ...faculty, [field]: value });
  };

  const handleSocialChange = (platform: keyof Faculty['social'], value: string) => {
    if (!faculty) return;
    setFaculty({
      ...faculty,
      social: {
        ...faculty.social,
        [platform]: value
      }
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
        <FacultyForm
          faculty={faculty}
          users={users}
          onChange={handleFieldChange}
          onSocialChange={handleSocialChange}
          onImageChange={handleSetImage}
          onImageRemove={handleRemoveImage}
          mode="edit"
        />
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
