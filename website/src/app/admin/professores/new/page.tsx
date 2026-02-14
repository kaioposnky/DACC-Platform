"use client"

import { EditPageHeader, FacultyForm, LoadingSpinner } from "@/components";
import { apiService } from "@/services/api";
import { Faculty, User } from "@/types";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { toast } from "sonner";

export default function AdminCreateProfessorPage() {

  const router = useRouter();
  const [faculty, setFaculty] = useState<Faculty>({
    id: '',
    name: '',
    title: '',
    position: '',
    specialization: '',
    imageUrl: '',
    userId: null,
    social: {
      linkedin: '',
      github: '',
      email: '',
    },
    createdAt: '',
    updatedAt: '',
  });
  const [users, setUsers] = useState<User[]>([]);
  const [isCreating, setIsCreating] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    setIsLoading(true);

    const fetchUsers = async () => {
      try {
        const response = await apiService.getUsers();
        setUsers(response);
      } catch (error) {
        toast.error('Erro ao buscar usuários! ' + error);
        console.error(error);
      } finally {
        setIsLoading(false);
      }
    }

    fetchUsers();
  }, []);

  const handleGoBack = () => router.back();

  const handleCreate = async () => {
    setIsCreating(true);
    try {
      await apiService.createFacultyMember(faculty);
      router.push('/admin/professores');
      toast.success('Professor criado com sucesso!');
    } catch (error) {
      toast.error('Erro ao criar professor! ' + error);
      console.error(error);
    } finally {
      setIsCreating(false);
    }
  }

  const handleChange = (key: keyof Faculty, value: string) => {
    setFaculty(prev => ({ ...prev, [key]: value }));
  }

  const handleChangeSocials = (key: keyof Faculty['social'], value: string) => {
    setFaculty(prev => ({ ...prev, social: { ...prev.social, [key]: value } }));
  }

  const handleChangeImage = (value: string) => {
    setFaculty(prev => ({ ...prev, imageUrl: value }));
  }

  const handleRemoveImage = () => {
    setFaculty(prev => ({ ...prev, imageUrl: '' }));
  }

  if (isLoading) {
    return (<LoadingSpinner />);
  }

  return (
    <div className="pb-20">
      <EditPageHeader
        title="Novo Professor"
        label="Criando"
        onBack={handleGoBack}
        onSave={handleCreate}
        loadingSave={isCreating}
        saveButtonText="Criar Novo Professor"
      />

      <div className="px-4 sm:px-6 lg:px-50 mt-8">
        <FacultyForm
          faculty={faculty}
          users={users}
          onChange={handleChange}
          onSocialChange={handleChangeSocials}
          onImageChange={handleChangeImage}
          onImageRemove={handleRemoveImage}
          mode="create"
        />
      </div>

    </div>
  )
}
