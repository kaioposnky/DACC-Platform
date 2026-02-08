import { AdminCard, Input, ImageUploadCard } from "@/components";
import { Faculty } from "@/types";
import {
    UserIcon,
    ShareIcon,
    IdentificationIcon,
    AcademicCapIcon,
} from "@heroicons/react/24/outline";

interface FacultyFormProps {
    faculty: Partial<Faculty>;
    onChange: (field: keyof Faculty, value: any) => void;
    onSocialChange: (platform: keyof Faculty['social'], value: string) => void;
    onImageChange: (url: string) => void;
    onImageRemove: () => void;
    mode?: 'create' | 'edit' | 'view';
}

export default function FacultyForm({
    faculty,
    onChange,
    onSocialChange,
    onImageChange,
    onImageRemove,
    mode = 'edit'
}: FacultyFormProps) {
    const isReadonly = mode === 'view';

    return (
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
                                value={faculty.name || ''}
                                onChange={(e) => onChange('name', e.target.value)}
                                required
                                disabled={isReadonly}
                            />
                            <Input
                                label="Título"
                                placeholder="Ex: Prof. Dr."
                                value={faculty.title || ''}
                                onChange={(e) => onChange('title', e.target.value)}
                                disabled={isReadonly}
                            />
                        </div>

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                            <Input
                                label="Cargo"
                                placeholder="Ex: Professor Titular"
                                value={faculty.position || ''}
                                onChange={(e) => onChange('position', e.target.value)}
                                disabled={isReadonly}
                            />
                            <Input
                                label="Especialização"
                                placeholder="Ex: Inteligência Artificial"
                                value={faculty.specialization || ''}
                                onChange={(e) => onChange('specialization', e.target.value)}
                                disabled={isReadonly}
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
                            value={faculty.social?.email || ''}
                            onChange={(e) => onSocialChange('email', e.target.value)}
                            disabled={isReadonly}
                        />
                        <Input
                            label="LinkedIn (URL)"
                            placeholder="Ex: https://linkedin.com/in/joaosilva"
                            value={faculty.social?.linkedin || ''}
                            onChange={(e) => onSocialChange('linkedin', e.target.value)}
                            disabled={isReadonly}
                        />
                        <Input
                            label="GitHub (URL)"
                            placeholder="Ex: https://github.com/joaosilva"
                            value={faculty.social?.github || ''}
                            onChange={(e) => onSocialChange('github', e.target.value)}
                            disabled={isReadonly}
                        />
                    </div>
                </AdminCard>
            </div>

            {/* Coluna Lateral */}
            <div className="space-y-6">
                <ImageUploadCard
                    title="Foto do Professor"
                    description={isReadonly ? "Foto de perfil do professor." : "Clique para alterar a foto de perfil."}
                    icon={<UserIcon className="w-10 h-10" />}
                    image={faculty.imageUrl || ''}
                    onSetImage={onImageChange}
                    onRemoveImage={onImageRemove}
                    galleryTitle="Gerenciar Foto do Professor"
                    galleryDescription="Esta imagem será exibida no card do professor."
                    previewClassName="aspect-square w-full rounded-2xl object-cover"
                    showModal={!isReadonly}
                />

                {mode === 'edit' && faculty.updatedAt && (
                    <AdminCard
                        title="Informações Acadêmicas"
                        icon={<AcademicCapIcon className="w-5 h-5 text-primary" />}
                    >
                        <div className="space-y-4 text-sm text-gray-500">
                            <p>Certifique-se de que o título e a especialização estão atualizados conforme o currículo Lattes.</p>
                            <div className="pt-4 border-t border-gray-100 italic">
                                Última atualização: {new Date(faculty.updatedAt).toLocaleDateString('pt-BR')}
                            </div>
                        </div>
                    </AdminCard>
                )}
            </div>
        </div>
    );
}
