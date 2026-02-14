"use client"

import { formatDate, formatPhone, formatRA, cleanNumeric } from "@/utils/formatters"; // Updated import to include new formatters
import { isValidEmail } from "@/utils/validators";
import { AdminCard, ConfirmationModal, EditPageHeader, ImageUploadCard, Input, PageLoader, ProgressSlider, Select, TagInput } from "@/components";
import { apiService } from "@/services/api";
import { User } from "@/types";
import { DocumentTextIcon, CommandLineIcon, ChartBarIcon, RocketLaunchIcon, UserIcon } from "@heroicons/react/24/solid";
import { useParams, useRouter } from "next/navigation";
import { useState, useEffect } from "react";
import { toast } from "sonner";

export default function AdminEditUsuarioPage() {
  const params = useParams();
  const router = useRouter();

  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);

  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [resetPassModalOpen, setResetPassModalOpen] = useState(false);

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const response = await apiService.getUser(params.id as string);
        if (response) {
          response.ra = formatRA(response.ra || '');
          response.phone = formatPhone(response.phone || '');
        }
        setUser(response);
      } catch (error) {
        toast.error("Error fetching user");
      } finally {
        setIsLoading(false);
      }
    };

    fetchUser();
  }, [params.id]);

  if (isLoading) return <PageLoader />;
  if (!user)
    return (
      <div className="flex flex-col items-center justify-center h-[60vh]">
        <h1 className="text-2xl font-bold text-gray-400">
          Usuário não encontrado
        </h1>
      </div>
    );

  const handleGoBack = () => {
    router.back();
  }

  const handleSave = async () => {
    if (!isValidEmail(user.email)) {
      toast.error("Por favor, insira um email válido.");
      return;
    }

    setIsSaving(true);
    try {
      const sanitizedUser = {
        ...user,
        ra: cleanNumeric(user.ra),
        phone: cleanNumeric(user.phone)
      };
      await apiService.updateUser(user.id, sanitizedUser);
      toast.success("Usuário atualizado com sucesso");
    } catch (error) {
      toast.error("Erro ao atualizar usuário");
    } finally {
      setIsSaving(false);
    }
  }

  const handleDelete = async () => {
    setIsDeleting(true);
    try {
      await apiService.deleteUser(user.id);
      toast.success("Usuário excluído com sucesso");
    } catch (error) {
      toast.error("Erro ao excluir usuário");
    } finally {
      setIsDeleting(false);
    }
  }

  const handleSendPasswordReset = async () => {
    apiService.forgotPassword(user.email)
      .then(() => toast.success("Email de redefinição enviado com sucesso!"))
      .catch(() => toast.error("Erro ao enviar email de redefinição."));
  }

  const handleChangeEmail = async (newEmail: string) => {
    setUser({ ...user, email: newEmail.toLowerCase().trim() });
  }

  const handleChangePhone = async (newPhone: string) => {
    setUser({ ...user, phone: newPhone });
  }

  const handleChangeRole = async (newRole: "aluno" | "diretor" | "administrador") => {
    setUser({ ...user, role: newRole });
  }

  const handleChangeRA = async (newRA: string) => {
    setUser({ ...user, ra: newRA });
  }

  const handleChangeCourse = async (newCourse: string) => {
    setUser({ ...user, course: newCourse });
  }

  const handleChangeName = async (newName: string) => {
    setUser({ ...user, name: newName });
  }

  const handleChangeLastName = async (newLastName: string) => {
    setUser({ ...user, lastName: newLastName });
  }

  const handleChangeNewsletter = async (newValue: string) => {
    setUser({ ...user, isSubscribedToNews: newValue === "true" });
  }

  const handleChangeStatus = async (newValue: string) => {
    setUser({ ...user, isActive: newValue === "true" });
  }

  const handleChangeAvatar = async (newAvatar: string) => {
    setUser({ ...user, avatar: newAvatar });
  }

  const handleRemoveAvatar = async () => {
    setUser({ ...user, avatar: "" });
  }

  return (
    <div className="pb-20">
      <EditPageHeader
        title="Editar Projeto"
        id={user.id}
        onBack={handleGoBack}
        onSave={handleSave}
        loadingSave={isSaving}
        showDelete={true}
        onDelete={handleDelete}
        loadingDelete={isDeleting}
        status={{
          text: user.role,
          colorClass:
            user.role === "administrador" ? "bg-red-100 text-red-700" :
              user.role === "diretor" ? "bg-blue-100 text-blue-700" :
                user.role === "aluno" ? "bg-gray-100 text-gray-700" : ""
        }}
      />

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Coluna Principal */}
          <div className="lg:col-span-2 space-y-6">

            <AdminCard
              icon={<ChartBarIcon className="w-5 h-5 text-primary" />}
              title="Informações na plataforma"
            >
              <div className="space-y-6">
                <Select
                  label="Cargo na Plataforma"
                  value={user.role}
                  onChange={(e) => handleChangeRole(e.target.value as "aluno" | "diretor" | "administrador")}
                  options={[
                    { label: "Aluno", value: "aluno" },
                    { label: "Diretor", value: "diretor" },
                    { label: "Administrador", value: "administrador" },
                  ]}
                />

                <Select
                  label="Inscrito no Newsletter"
                  value={user.isSubscribedToNews ? "true" : "false"}
                  onChange={(e) => handleChangeNewsletter(e.target.value)}
                  options={[
                    { label: "Sim", value: "true" },
                    { label: "Não", value: "false" },
                  ]}
                />

                <Select
                  label="Status da Conta"
                  value={user.isActive ? "true" : "false"}
                  onChange={(e) => handleChangeStatus(e.target.value)}
                  options={[
                    { label: "Ativo", value: "true" },
                    { label: "Inativo", value: "false" },
                  ]}
                />
              </div>
            </AdminCard>

            <AdminCard
              icon={<CommandLineIcon className="w-5 h-5 text-primary" />}
              title="Informações acadêmicas"
            >
              <div className="space-y-4">
                <Input
                  label="RA"
                  placeholder="Ex: 12.345.678-9"
                  value={user.ra || ''}
                  onChange={(e) => {
                    const formatted = formatRA(e.target.value);
                    handleChangeRA(formatted);
                  }}
                />

                <Input
                  label="Curso"
                  placeholder="Ex: Ciência da Computação"
                  value={user.course}
                  onChange={(e) => handleChangeCourse(e.target.value)}
                />
              </div>
            </AdminCard>
          </div>

          {/* Coluna Lateral */}
          <div className="space-y-6">

            <AdminCard
              title="Perfil do usuário"
              icon={<UserIcon className="w-10 h-10" />}
            >
              <div className="flex flex-col space-y-4">
                {/* Metadata - Dates */}
                <div className="text-sm text-gray-500 space-y-1 pb-4 border-b border-gray-100">
                  <div className="flex justify-between">
                    <span>Cadastrado em:</span>
                    <span className="font-medium text-gray-700">{formatDate(user.createdAt || '')}</span>
                  </div>
                  {user.updatedAt && (
                    <div className="flex justify-between">
                      <span>Última atualização:</span>
                      <span className="font-medium text-gray-700">{formatDate(user.updatedAt || '')}</span>
                    </div>
                  )}
                </div>

                <Input
                  label="Nome"
                  value={user.name}
                  onChange={(e) => handleChangeName(e.target.value)}
                  placeholder="Digite o nome do usuário"
                  required
                />
                <Input
                  label="Sobrenome"
                  value={user.lastName}
                  onChange={(e) => handleChangeLastName(e.target.value)}
                  placeholder="Digite o sobrenome do usuário"
                  required
                />
                <Input
                  label="Email"
                  value={user.email}
                  onChange={(e) => handleChangeEmail(e.target.value)}
                  placeholder="Digite o email do usuário"
                  required
                />
                <Input
                  label="Número de telefone"
                  placeholder="Ex: (11) 91234-5678"
                  value={user.phone}
                  onChange={(e) => {
                    const formatted = formatPhone(e.target.value);
                    handleChangePhone(formatted);
                  }}
                />

                {/* Password Reset Action */}
                <div className="pt-2">
                  <button
                    type="button"
                    onClick={() => setResetPassModalOpen(true)}
                    className="w-full text-sm text-blue-600 hover:text-blue-800 hover:underline text-left flex items-center gap-1"
                  >
                    <span>Enviar Email de Redefinição de Senha</span>
                  </button>
                </div>
              </div>
            </AdminCard>

            <ImageUploadCard
              title="Avatar do usuário"
              description="Clique para alterar o avatar do usuário."
              icon={<RocketLaunchIcon className="w-10 h-10" />}
              image={user.avatar}
              onSetImage={handleChangeAvatar}
              onRemoveImage={handleRemoveAvatar}
              galleryTitle="Gerenciar Avatar"
              galleryDescription="Esta imagem será usada como o avatar do usuário."
              previewClassName="aspect-square w-32 h-32"
              showModal={true}
            />

          </div>
        </div>
      </div>

      {/* Modal de Confirmação de Deleção */}
      <ConfirmationModal
        isOpen={deleteModalOpen}
        onClose={() => setDeleteModalOpen(false)}
        onConfirm={handleDelete}
        title="Excluir Usuário"
        message={`Tem certeza que deseja excluir o usuário "${user.name + " " + user.lastName}"? Todos os dados associados a ele serão removidos permanentemente, incluindo pagamentos, notícias, avaliações, comentários, e-mails, mensagens, e outras informações relacionadas.`}
        confirmLabel="Sim, Excluir"
        isLoading={isDeleting}
      />

      {/* Modal de Confirmação de Resetar Senha*/}
      <ConfirmationModal
        isOpen={resetPassModalOpen}
        onClose={() => setResetPassModalOpen(false)}
        onConfirm={handleSendPasswordReset}
        title="Resetar Senha"
        message={`Tem certeza que deseja resetar a senha do usuário "${user.name + " " + user.lastName}"? Um link para redefinir a senha será enviado para o e-mail cadastrado.`}
        confirmLabel="Sim, Resetar"
        isLoading={isDeleting}
      />
    </div>
  );
}
