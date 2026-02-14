import {
    AdminCard,
    Input,
    Select,
    TagInput,
    ImageUploadCard,
    DateTimeInputs
} from "@/components";
import { News, User } from "@/types";
import {
    UserIcon,
    Square3Stack3DIcon,
    DocumentTextIcon,
    PhotoIcon
} from "@heroicons/react/24/outline";

interface NewsFormProps {
    news: Partial<News>;
    users: User[];
    onChange: (field: keyof News, value: any) => void;
    onImageChange: (url: string) => void;
    onImageRemove: () => void;
    mode?: 'create' | 'edit' | 'view';
}

const CATEGORIES = [
    { label: "Acadêmico", value: "Acadêmico" },
    { label: "Evento", value: "Evento" },
    { label: "DACC", value: "DACC" },
    { label: "Competição", value: "Competição" },
    { label: "Outros", value: "Outros" },
];

export default function NewsForm({
    news,
    users,
    onChange,
    onImageChange,
    onImageRemove,
    mode = 'edit'
}: NewsFormProps) {
    const isReadonly = mode === 'view';

    const handleAddTag = (tag: string) => {
        const tags = news.tags || [];
        onChange('tags', [...tags, tag]);
    };

    const handleRemoveTag = (tag: string) => {
        const tags = news.tags || [];
        onChange('tags', tags.filter((t) => t !== tag));
    };

    const handleAuthorChange = (authorId: string) => {
        const selectedAuthor = users.find(u => u.id === authorId);
        if (selectedAuthor) {
            onChange('author', selectedAuthor);
        } else {
            onChange('author', undefined);
        }
    };

    const handleDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newDate = e.target.value;
        const currentTime = (news.date || "").includes('T') ? (news.date || "").split('T')[1].substring(0, 5) : '00:00';
        onChange('date', `${newDate}T${currentTime}:00`);
    };

    const handleTimeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newTime = e.target.value;
        const currentDate = (news.date || "").split('T')[0] || new Date().toISOString().split('T')[0];
        onChange('date', `${currentDate}T${newTime}:00`);
    };

    return (
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            {/* Coluna Principal */}
            <div className="lg:col-span-2 space-y-6">
                <AdminCard
                    icon={<DocumentTextIcon className="w-5 h-5 text-primary" />}
                    title="Conteúdo da Notícia"
                >
                    <div className="space-y-6">
                        <Input
                            label="Título da Notícia"
                            placeholder="Ex: Novo laboratório de IA é inaugurado na FEI"
                            value={news.title || ''}
                            onChange={(e) => onChange('title', e.target.value)}
                            className="text-lg font-bold"
                            required
                            disabled={isReadonly}
                        />

                        <Input
                            label="Descrição Curta (Resumo)"
                            placeholder="Um breve resumo que aparece na listagem..."
                            value={news.description || ''}
                            onChange={(e) => onChange('description', e.target.value)}
                            multiline
                            rows={3}
                            required
                            disabled={isReadonly}
                        />

                        <Input
                            label="Conteúdo Completo"
                            placeholder="Escreva aqui o corpo da notícia..."
                            value={news.content || ""}
                            onChange={(e) => onChange('content', e.target.value)}
                            multiline
                            rows={15}
                            required
                            disabled={isReadonly}
                        />
                    </div>
                </AdminCard>
            </div>

            {/* Coluna Lateral */}
            <div className="space-y-6">
                <AdminCard
                    icon={<Square3Stack3DIcon className="w-5 h-5 text-primary" />}
                    title="Classificação"
                >
                    <div className="space-y-4">
                        <Select
                            label="Categoria"
                            value={news.category || ''}
                            onChange={(e) => onChange('category', e.target.value)}
                            options={CATEGORIES}
                            disabled={isReadonly}
                        />
                        <div>
                            <TagInput
                                label="Tags da Notícia"
                                tags={news.tags || []}
                                onAddTag={handleAddTag}
                                onRemoveTag={handleRemoveTag}
                                disabled={isReadonly}
                            />
                        </div>
                    </div>
                </AdminCard>

                <AdminCard
                    icon={<UserIcon className="w-5 h-5 text-primary" />}
                    title="Publicação"
                >
                    <div className="space-y-4">
                        <Select
                            label="Autor"
                            value={news.author?.id || ""}
                            onChange={(e) => handleAuthorChange(e.target.value)}
                            options={[
                                { label: "Selecione um autor", value: "" },
                                ...users.map((user) => ({
                                    label: `${user.name} ${user.lastName || ""} ${user.ra ? `(RA: ${user.ra})` : ""}`.trim(),
                                    value: user.id,
                                })),
                            ]}
                            disabled={isReadonly}
                        />

                        <div className={isReadonly ? "pointer-events-none opacity-80" : ""}>
                            <DateTimeInputs
                                dateLabel="Data de Publicação"
                                timeLabel="Horário"
                                dateValue={news.date || ''}
                                timeValue={(news.date || '').includes('T') ? (news.date || '').split('T')[1].substring(0, 5) : ''}
                                onDateChange={handleDateChange}
                                onTimeChange={handleTimeChange}
                            />
                        </div>

                        <Input
                            label="Tempo de Leitura (minutos)"
                            type="number"
                            value={news.readTime || 0}
                            onChange={(e) => onChange('readTime', parseInt(e.target.value) || 0)}
                            disabled={isReadonly}
                        />
                    </div>
                </AdminCard>

                <ImageUploadCard
                    title="Imagem de Capa"
                    description={isReadonly ? "Imagem exibida no topo da notícia." : "Clique para alterar a imagem de capa."}
                    icon={<PhotoIcon className="w-10 h-10" />}
                    image={news.image || ''}
                    onSetImage={onImageChange}
                    onRemoveImage={onImageRemove}
                    galleryTitle="Gerenciar Imagem de Capa"
                    galleryDescription="Esta imagem aparecerá no topo da notícia e nos cards da listagem."
                    showModal={!isReadonly}
                />
            </div>
        </div>
    );
}
