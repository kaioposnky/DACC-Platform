import { AdminCard, Input, Select, ImageUploadCard } from "@/components";
import { Announcement } from "@/types";
import {
    MegaphoneIcon,
    DocumentTextIcon,
    PhotoIcon,
    LinkIcon,
    XMarkIcon
} from "@heroicons/react/24/outline";

interface AnnouncementFormProps {
    announcement: Partial<Announcement>;
    onChange: (field: keyof Announcement, value: any) => void;
    onImageChange: (url: string) => void;
    onImageRemove: () => void;
    mode?: 'create' | 'edit' | 'view';
}

export default function AnnouncementForm({
    announcement,
    onChange,
    onImageChange,
    onImageRemove,
    mode = 'edit'
}: AnnouncementFormProps) {
    const isReadonly = mode === 'view';

    const addDetail = () => {
        const details = announcement.details || [];
        onChange('details', [...details, { icon: "", text: "" }]);
    };

    const removeDetail = (index: number) => {
        const details = announcement.details || [];
        onChange('details', details.filter((_, idx) => idx !== index));
    };

    const updateDetail = (index: number, field: 'icon' | 'text', value: string) => {
        const details = [...(announcement.details || [])];
        details[index] = { ...details[index], [field]: value };
        onChange('details', details);
    };

    return (
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            {/* Coluna Principal */}
            <div className="lg:col-span-2 space-y-6">
                <AdminCard
                    icon={<DocumentTextIcon className="w-5 h-5 text-primary" />}
                    title="Conteúdo do Anúncio"
                >
                    <div className="space-y-6">
                        <Input
                            label="Título do Anúncio"
                            placeholder="Ex: Novo Evento DACC"
                            value={announcement.title || ''}
                            onChange={(e) => onChange('title', e.target.value)}
                            className="text-lg font-bold"
                            required
                            disabled={isReadonly}
                        />

                        <Input
                            label="Conteúdo / Descrição"
                            placeholder="Descreva o anúncio..."
                            value={announcement.content || ''}
                            onChange={(e) => onChange('content', e.target.value)}
                            multiline
                            rows={6}
                            required
                            disabled={isReadonly}
                        />

                        <Input
                            label="Ícone (Emoji ou classe CSS)"
                            placeholder="Ex: 🎉 ou fa-rocket"
                            value={announcement.icon || ''}
                            onChange={(e) => onChange('icon', e.target.value)}
                            disabled={isReadonly}
                        />
                    </div>
                </AdminCard>

                <AdminCard
                    icon={<LinkIcon className="w-5 h-5 text-primary" />}
                    title="Botões de Ação"
                >
                    <div className="space-y-6">
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <div className="space-y-4">
                                <h4 className="text-sm font-semibold text-gray-700">Botão Primário</h4>
                                <Input
                                    label="Texto do Botão"
                                    placeholder="Ex: Saiba Mais"
                                    value={announcement.primaryButtonText || ''}
                                    onChange={(e) => onChange('primaryButtonText', e.target.value)}
                                    disabled={isReadonly}
                                />
                                <Input
                                    label="Link do Botão"
                                    placeholder="Ex: /eventos/123"
                                    value={announcement.primaryButtonLink || ''}
                                    onChange={(e) => onChange('primaryButtonLink', e.target.value)}
                                    disabled={isReadonly}
                                />
                            </div>

                            <div className="space-y-4">
                                <h4 className="text-sm font-semibold text-gray-700">Botão Secundário</h4>
                                <Input
                                    label="Texto do Botão"
                                    placeholder="Ex: Ver Detalhes"
                                    value={announcement.secondaryButtonText || ''}
                                    onChange={(e) => onChange('secondaryButtonText', e.target.value)}
                                    disabled={isReadonly}
                                />
                                <Input
                                    label="Link do Botão"
                                    placeholder="Ex: /projetos/456"
                                    value={announcement.secondaryButtonLink || ''}
                                    onChange={(e) => onChange('secondaryButtonLink', e.target.value)}
                                    disabled={isReadonly}
                                />
                            </div>
                        </div>
                    </div>
                </AdminCard>

                <AdminCard
                    icon={<MegaphoneIcon className="w-5 h-5 text-primary" />}
                    title="Detalhes do Anúncio"
                >
                    <div className="space-y-4">
                        <div className="flex items-center justify-between">
                            <p className="text-sm text-gray-600">
                                Adicione itens com ícone e texto para destacar informações
                            </p>
                            {!isReadonly && (
                                <button
                                    type="button"
                                    onClick={addDetail}
                                    className="px-3 py-1.5 bg-primary text-white rounded-lg text-sm font-medium hover:bg-primary/90 transition-colors"
                                >
                                    + Adicionar
                                </button>
                            )}
                        </div>

                        {(!announcement.details || announcement.details.length === 0) ? (
                            <p className="text-sm text-gray-400 italic py-4 text-center">
                                Nenhum detalhe adicionado.
                            </p>
                        ) : (
                            <div className="space-y-3">
                                {announcement.details.map((detail, index) => (
                                    <div
                                        key={index}
                                        className="flex items-start gap-3 p-3 bg-gray-50 rounded-lg border border-gray-100"
                                    >
                                        <div className="flex-1 grid grid-cols-2 gap-3">
                                            <Input
                                                label={`Ícone ${index + 1}`}
                                                placeholder="Ex: 📅"
                                                value={detail.icon}
                                                onChange={(e) => updateDetail(index, 'icon', e.target.value)}
                                                disabled={isReadonly}
                                            />
                                            <Input
                                                label={`Texto ${index + 1}`}
                                                placeholder="Ex: 20 de Março"
                                                value={detail.text}
                                                onChange={(e) => updateDetail(index, 'text', e.target.value)}
                                                disabled={isReadonly}
                                            />
                                        </div>
                                        {!isReadonly && (
                                            <button
                                                type="button"
                                                onClick={() => removeDetail(index)}
                                                className="mt-6 p-2 text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                                                title="Remover detalhe"
                                            >
                                                <XMarkIcon className="w-5 h-5" />
                                            </button>
                                        )}
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                </AdminCard>
            </div>

            {/* Coluna Lateral */}
            <div className="space-y-6">
                <AdminCard
                    icon={<MegaphoneIcon className="w-5 h-5 text-primary" />}
                    title="Configurações"
                >
                    <Select
                        label="Tipo de Anúncio"
                        value={announcement.type || ''}
                        onChange={(e) => onChange('type', e.target.value)}
                        options={[
                            { label: "Evento", value: "event" },
                            { label: "Destaque", value: "highlight" },
                        ]}
                        disabled={isReadonly}
                    />
                </AdminCard>

                <ImageUploadCard
                    title="Imagem do Anúncio"
                    description={isReadonly ? "Imagem exibida no card do anúncio." : "Clique para alterar a imagem do anúncio."}
                    icon={<PhotoIcon className="w-10 h-10" />}
                    image={announcement.imageSrc || ''}
                    onSetImage={onImageChange}
                    onRemoveImage={onImageRemove}
                    galleryTitle="Gerenciar Imagem do Anúncio"
                    galleryDescription="Esta imagem será exibida no card do anúncio."
                    showModal={!isReadonly}
                />

                <AdminCard title="Texto Alternativo">
                    <Input
                        label="Alt Text"
                        placeholder="Descrição da imagem para acessibilidade"
                        value={announcement.imageAlt || ''}
                        onChange={(e) => onChange('imageAlt', e.target.value)}
                        disabled={isReadonly}
                    />
                </AdminCard>
            </div>
        </div>
    );
}
