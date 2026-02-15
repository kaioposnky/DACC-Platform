import {
	ChevronDownIcon,
	DocumentTextIcon,
	PhotoIcon,
	PlusIcon,
	Square3Stack3DIcon,
	UserIcon,
} from "@heroicons/react/24/outline";
import { useEffect, useState } from "react";
import {
	AdminCard,
	DateTimeInputs,
	ImageUploadCard,
	Input,
	Select,
	TagInput,
} from "@/components";
import { apiService } from "@/services/api";
import type { News, NewsCategory, User } from "@/types";
import { toast } from "sonner";

interface NewsFormProps {
	news: Partial<News>;
	users: User[];
	onChange: (field: keyof News, value: any) => void;
	mode?: "create" | "edit" | "view";
}

export default function NewsForm({
	news,
	users,
	onChange,
	mode = "edit",
}: NewsFormProps) {
	const isReadonly = mode === "view";

	const [showCategoryManager, setShowCategoryManager] = useState(false);
	const [localCategories, setLocalCategories] = useState<NewsCategory[]>([]);
	const [newCategoryName, setNewCategoryName] = useState("");
	const [isCreatingCategory, setIsCreatingCategory] = useState(false);

	useEffect(() => {
		const fetchCategories = async () => {
			try {
				const data = await apiService.getNewsCategories();
				setLocalCategories(data);
			} catch (error) {
				console.error("Erro ao buscar categorias:", error);
			}
		};
		fetchCategories();
	}, []);

	const handleCreateCategory = async () => {
		if (!newCategoryName.trim()) return;
		try {
			setIsCreatingCategory(true);
			const newCategory = await apiService.createNewsCategory(newCategoryName);
			setLocalCategories([...localCategories, newCategory]);
			setNewCategoryName("");
			toast.success("Categoria criada com sucesso!");
		} catch (error) {
			console.error("Erro ao criar categoria:", error);
			toast.error("Erro ao criar categoria!");
		} finally {
			setIsCreatingCategory(false);
		}
	};

	const handleCategoryChange = (categoryId: string) => {
		const selectedCategory = localCategories.find((c) => c.id === categoryId);
		onChange("categoryId", categoryId);
		if (selectedCategory) {
			onChange("category", selectedCategory);
			onChange("categoryName", selectedCategory.name);
		}
	};

	const handleAddTag = (tag: string) => {
		const tags = news.tags || [];
		onChange("tags", [...tags, tag]);
	};

	const handleRemoveTag = (tag: string) => {
		const tags = news.tags || [];
		onChange(
			"tags",
			tags.filter((t) => t !== tag),
		);
	};

	const handleAuthorChange = (authorId: string) => {
		const selectedAuthor = users.find((u) => u.id === authorId);
		onChange("author", selectedAuthor || undefined);
	};

	const handleDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const newDate = e.target.value;
		const currentTime = (news.date || "").includes("T")
			? (news.date || "").split("T")[1].substring(0, 5)
			: "00:00";
		onChange("date", `${newDate}T${currentTime}:00`);
	};

	const handleTimeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const newTime = e.target.value;
		const currentDate =
			(news.date || "").split("T")[0] || new Date().toISOString().split("T")[0];
		onChange("date", `${currentDate}T${newTime}:00`);
	};

	const handleSetImage = (url: string) => {
		onChange("image", url);
	};

	const handleRemoveImage = () => {
		onChange("image", "");
	};

	return (
		<div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
			<div className="lg:col-span-2 space-y-6">
				{!isReadonly && (
					<AdminCard
						icon={<Square3Stack3DIcon className="w-5 h-5 text-primary" />}
						title="Gerenciar Categorias"
						actions={
							<button
								onClick={() => setShowCategoryManager(!showCategoryManager)}
								className="flex items-center gap-1 text-xs font-bold text-blue-600 hover:text-blue-800 uppercase"
							>
								{showCategoryManager ? "Ocultar" : "Mostrar"}
								<ChevronDownIcon
									className={`w-4 h-4 transition-transform ${showCategoryManager ? "rotate-180" : ""}`}
								/>
							</button>
						}
					>
						{showCategoryManager && (
							<div className="space-y-4">
								<div className="space-y-2">
									<label className="block text-xs font-semibold text-gray-700 uppercase">
										Nova Categoria
									</label>
									<div className="flex flex-col gap-2">
										<Input
											value={newCategoryName}
											onChange={(e) => setNewCategoryName(e.target.value)}
											placeholder="Nome da categoria (ex: Evento, Acadêmico)"
											disabled={isCreatingCategory}
											onKeyDown={(e) =>
												e.key === "Enter" &&
												!e.shiftKey &&
												handleCreateCategory()
											}
										/>
										<button
											onClick={handleCreateCategory}
											disabled={!newCategoryName.trim() || isCreatingCategory}
											className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center gap-2 w-full"
										>
											<PlusIcon className="w-4 h-4" />
											{isCreatingCategory ? "Criando..." : "Criar Categoria"}
										</button>
									</div>
								</div>
							</div>
						)}
					</AdminCard>
				)}

				<AdminCard
					icon={<DocumentTextIcon className="w-5 h-5 text-primary" />}
					title="Conteúdo da Notícia"
				>
					<div className="space-y-6">
						<Input
							label="Título da Notícia"
							placeholder="Ex: Novo laboratório de IA é inaugurado na FEI"
							value={news.title || ""}
							onChange={(e) => onChange("title", e.target.value)}
							className="text-lg font-bold"
							required
							disabled={isReadonly}
						/>

						<Input
							label="Descrição Curta (Resumo)"
							placeholder="Um breve resumo que aparece na listagem..."
							value={news.description || ""}
							onChange={(e) => onChange("description", e.target.value)}
							multiline
							rows={3}
							required
							disabled={isReadonly}
						/>

						<Input
							label="Conteúdo Completo"
							placeholder="Escreva aqui o corpo da notícia..."
							value={news.content || ""}
							onChange={(e) => onChange("content", e.target.value)}
							multiline
							rows={15}
							required
							disabled={isReadonly}
						/>
					</div>
				</AdminCard>
			</div>

			<div className="space-y-6">
				<AdminCard
					icon={<Square3Stack3DIcon className="w-5 h-5 text-primary" />}
					title="Classificação"
				>
					<div className="space-y-4">
						<Select
							label="Categoria"
							value={news.categoryId || news.category?.id || ""}
							onChange={(e) => handleCategoryChange(e.target.value)}
							options={[
								{ label: "Selecione uma categoria", value: "" },
								...localCategories.map((c) => ({ label: c.name, value: c.id })),
							]}
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
									label:
										`${user.name} ${user.lastName || ""} ${user.ra ? `(RA: ${user.ra})` : ""}`.trim(),
									value: user.id,
								})),
							]}
							disabled={isReadonly}
						/>

						<div className={isReadonly ? "pointer-events-none opacity-80" : ""}>
							<DateTimeInputs
								dateLabel="Data de Publicação"
								timeLabel="Horário"
								dateValue={news.date || ""}
								timeValue={
									(news.date || "").includes("T")
										? (news.date || "").split("T")[1].substring(0, 5)
										: ""
								}
								onDateChange={handleDateChange}
								onTimeChange={handleTimeChange}
							/>
						</div>

						<Input
							label="Tempo de Leitura (minutos)"
							type="number"
							value={news.readTime || 0}
							onChange={(e) =>
								onChange("readTime", parseInt(e.target.value) || 0)
							}
							disabled={isReadonly}
						/>
					</div>
				</AdminCard>

				<ImageUploadCard
					title="Imagem de Capa"
					description={
						isReadonly
							? "Imagem exibida no topo da notícia."
							: "Clique para alterar a imagem de capa."
					}
					icon={<PhotoIcon className="w-10 h-10" />}
					image={news.image || ""}
					onSetImage={handleSetImage}
					onRemoveImage={handleRemoveImage}
					galleryTitle="Gerenciar Imagem de Capa"
					galleryDescription="Esta imagem aparecerá no topo da notícia e nos cards da listagem."
					showModal={!isReadonly}
				/>

				{!isReadonly && (
					<Input
						label="Texto Alternativo da Imagem (Acessibilidade)"
						placeholder="Descreva a imagem para leitores de tela..."
						value={news.imageAlt || ""}
						onChange={(e) => onChange("imageAlt", e.target.value)}
					/>
				)}

				<AdminCard
					icon={<Square3Stack3DIcon className="w-5 h-5 text-primary" />}
					title="Estilo e Links"
				>
					<div className="space-y-4">
						<Input
							label="Gradiente CSS (Opcional)"
							placeholder="Ex: from-blue-500 to-cyan-500"
							value={news.gradient || ""}
							onChange={(e) => onChange("gradient", e.target.value)}
							disabled={isReadonly}
						/>

						<Input
							label="Ícone (URL ou Classe)"
							placeholder="URL de ícone ou classe..."
							value={news.icon || ""}
							onChange={(e) => onChange("icon", e.target.value)}
							disabled={isReadonly}
						/>

						<Input
							label="Link 'Ler Mais' (Externo)"
							placeholder="https://..."
							value={news.readMoreLink || ""}
							onChange={(e) => onChange("readMoreLink", e.target.value)}
							disabled={isReadonly}
						/>
					</div>
				</AdminCard>
			</div>
		</div>
	);
}
