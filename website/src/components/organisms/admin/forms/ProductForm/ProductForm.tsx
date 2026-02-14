"use client";

import {
  AdminCard,
  Input,
  Select,
  TagInput,
  Modal,
  ImageGalleryEditor,
} from "@/components";
import {
  Product,
  ProductVariation,
  ProductSpecification,
  ProductCategory,
  ProductSubcategory,
  ProductSize,
  ProductColor,
} from "@/types";
import {
  TrashIcon,
  InformationCircleIcon,
  ClipboardDocumentListIcon,
  CurrencyDollarIcon,
  SwatchIcon,
  ChevronDownIcon,
  PlusIcon,
} from "@heroicons/react/24/outline";
import { useState, useEffect, useMemo } from "react";
import { apiService } from "@/services/api";

interface ProductFormProps {
  product: Partial<Product>;
  onChange: (field: keyof Product, value: any) => void;
  mode?: "create" | "edit" | "view";
}

// Fetched from API

export default function ProductForm({
  product,
  onChange,
  mode = "edit",
}: ProductFormProps) {
  const isReadonly = mode === "view";

  // Gallery Modal State
  const [isGalleryOpen, setIsGalleryOpen] = useState(false);
  const [selectedVariantIndex, setSelectedVariantIndex] = useState<
    number | null
  >(null);

  // Filter Data State
  const [categories, setCategories] = useState<ProductCategory[]>([]);
  const [allSubcategories, setAllSubcategories] = useState<ProductSubcategory[]>([]);
  const [sizes, setSizes] = useState<ProductSize[]>([]);
  const [colors, setColors] = useState<ProductColor[]>([]);
  const [isLoadingFilters, setIsLoadingFilters] = useState(false);

  useEffect(() => {
    const fetchFilters = async () => {
      try {
        setIsLoadingFilters(true);
        const [cats, subcats, szs, cls] = await Promise.all([
          apiService.getCategories(),
          apiService.getSubcategories(),
          apiService.getSizes(),
          apiService.getColors()
        ]);
        setCategories(cats || []);
        setAllSubcategories(subcats || []);
        setSizes(szs || []);
        setColors(cls || []);
      } catch (error) {
        console.error("Erro ao buscar filtros de produto:", error);
      } finally {
        setIsLoadingFilters(false);
      }
    };

    fetchFilters();
  }, []);

  // Filter Mappings for Select Components
  const categoryOptions = useMemo(() =>
    categories.map(c => ({ label: c.name, value: c.id })),
    [categories]
  );

  const subcategoryOptions = useMemo(() => {
    if (!product.category) return [];
    return allSubcategories
      .filter(s => s.categoryId === product.category)
      .map(s => ({ label: s.name, value: s.id }));
  }, [allSubcategories, product.category]);

  const sizeOptions = useMemo(() =>
    sizes.map(s => ({ label: s.name, value: s.id })),
    [sizes]
  );

  const colorOptions = useMemo(() =>
    colors.map(c => ({ label: c.name, value: c.id })),
    [colors]
  );

  // Filter Management State
  const [showFilterManager, setShowFilterManager] = useState(false);
  const [newCategoryName, setNewCategoryName] = useState("");
  const [newSubcategoryName, setNewSubcategoryName] = useState("");
  const [newSubcategoryCategoryId, setNewSubcategoryCategoryId] = useState("");
  const [newSizeName, setNewSizeName] = useState("");
  const [newColorName, setNewColorName] = useState("");
  const [isCreatingFilter, setIsCreatingFilter] = useState(false);

  // Filter Create Handlers
  const handleCreateCategory = async () => {
    if (!newCategoryName.trim()) return;
    try {
      setIsCreatingFilter(true);
      const newCategory = await apiService.createCategory(newCategoryName);
      setCategories([...categories, newCategory]);
      setNewCategoryName("");
    } catch (error) {
      console.error("Erro ao criar categoria:", error);
    } finally {
      setIsCreatingFilter(false);
    }
  };

  const handleCreateSubcategory = async () => {
    if (!newSubcategoryName.trim() || !newSubcategoryCategoryId) return;
    try {
      setIsCreatingFilter(true);
      const newSubcategory = await apiService.createSubcategory(
        newSubcategoryName,
        newSubcategoryCategoryId
      );
      setAllSubcategories([...allSubcategories, newSubcategory]);
      setNewSubcategoryName("");
      setNewSubcategoryCategoryId("");
    } catch (error) {
      console.error("Erro ao criar subcategoria:", error);
    } finally {
      setIsCreatingFilter(false);
    }
  };

  const handleCreateSize = async () => {
    if (!newSizeName.trim()) return;
    try {
      setIsCreatingFilter(true);
      const newSize = await apiService.createSize(newSizeName);
      setSizes([...sizes, newSize]);
      setNewSizeName("");
    } catch (error) {
      console.error("Erro ao criar tamanho:", error);
    } finally {
      setIsCreatingFilter(false);
    }
  };

  const handleCreateColor = async () => {
    if (!newColorName.trim()) return;
    try {
      setIsCreatingFilter(true);
      const newColor = await apiService.createColor(newColorName);
      setColors([...colors, newColor]);
      setNewColorName("");
    } catch (error) {
      console.error("Erro ao criar cor:", error);
    } finally {
      setIsCreatingFilter(false);
    }
  };

  // Price Logic
  const calculateDiscount = (orig: number, final: number) => {
    if (!orig || orig === 0) return undefined;
    const discount = Math.round(((orig - final) / orig) * 100);
    return discount > 0 ? discount : undefined;
  };

  const handlePriceChange = (
    field: "price" | "originalPrice" | "discount",
    value: number,
  ) => {
    const currentPrice = product.price || 0;
    const currentOriginal = product.originalPrice || currentPrice;

    if (field === "originalPrice") {
      const discount = calculateDiscount(currentOriginal || 0, currentPrice);
      const newPrice = value * (1 - (discount ?? 0) / 100);
      onChange("originalPrice", value);
      onChange("price", Number(newPrice.toFixed(2)));
    } else if (field === "price") {
      onChange("price", value);
    } else if (field === "discount") {
      const newPrice = (currentOriginal || 0) * (1 - value / 100);
      onChange("price", Number(newPrice.toFixed(2)));
    }
  };

  // Variation Logic
  const addVariation = () => {
    const variations = product.variations || [];
    onChange("variations", [
      ...variations,
      {
        id: `temp-${Date.now()}`,
        color: "",
        size: "",
        stock: 0,
        sku: "",
        images: [],
      },
    ]);
  };

  const updateVariation = (
    index: number,
    field: keyof ProductVariation,
    value: any,
  ) => {
    const variations = [...(product.variations || [])];
    variations[index] = { ...variations[index], [field]: value };
    onChange("variations", variations);
  };

  const removeVariation = (index: number) => {
    const variations = (product.variations || []).filter((_, i) => i !== index);
    onChange("variations", variations);
  };

  const handleOpenGallery = (index: number) => {
    if (isReadonly) return;
    setSelectedVariantIndex(index);
    setIsGalleryOpen(true);
  };

  const handleAddImageToVariant = (imageUrl: string) => {
    if (!imageUrl || selectedVariantIndex === null || !product.variations)
      return;
    const variations = [...product.variations];
    const currentImages = variations[selectedVariantIndex].images || [];
    variations[selectedVariantIndex] = {
      ...variations[selectedVariantIndex],
      images: [
        ...currentImages,
        {
          url: imageUrl,
          order: currentImages.length + 1,
          imageAlt: product.name || "Imagem do produto"
        },
      ],
    };
    onChange("variations", variations);
  };

  const handleRemoveImageFromVariant = (imageIndex: number) => {
    if (selectedVariantIndex === null || !product.variations) return;
    const variations = [...product.variations];
    const filteredImages = variations[selectedVariantIndex].images.filter(
      (_, i) => i !== imageIndex,
    );
    // Reorder remaining images to maintain sequential order (1, 2, 3...)
    const reorderedImages = filteredImages.map((img, idx) => ({
      ...img,
      order: idx + 1,
    }));
    variations[selectedVariantIndex] = {
      ...variations[selectedVariantIndex],
      images: reorderedImages,
    };
    onChange("variations", variations);
  };

  // Specification Logic
  const addSpecification = () => {
    const specs = product.specifications || [];
    onChange("specifications", [...specs, { name: "", value: "" }]);
  };

  const updateSpecification = (index: number, name: string, value: string) => {
    const specs = [...(product.specifications || [])];
    specs[index] = { name, value };
    onChange("specifications", specs);
  };

  const removeSpecification = (index: number) => {
    const specs = (product.specifications || []).filter((_, i) => i !== index);
    onChange("specifications", specs);
  };

  return (
    <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
      {/* Coluna Principal */}
      <div className="lg:col-span-2 space-y-8">
        {/* Filter Management Card */}
        {!isReadonly && (
          <AdminCard
            icon={<SwatchIcon className="w-5 h-5 text-primary" />}
            title="Gerenciar Filtros de Produtos"
            actions={
              <button
                onClick={() => setShowFilterManager(!showFilterManager)}
                className="flex items-center gap-1 text-xs font-bold text-blue-600 hover:text-blue-800 uppercase"
              >
                {showFilterManager ? "Ocultar" : "Mostrar"}
                <ChevronDownIcon
                  className={`w-4 h-4 transition-transform ${showFilterManager ? "rotate-180" : ""}`}
                />
              </button>
            }
          >
            {showFilterManager && (
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                {/* Create Category */}
                <div className="space-y-2">
                  <label className="block text-xs font-semibold text-gray-700 uppercase">
                    Nova Categoria
                  </label>
                  <div className="flex gap-2">
                    <Input
                      value={newCategoryName}
                      onChange={(e) => setNewCategoryName(e.target.value)}
                      placeholder="Ex: Camisetas"
                      className="flex-1"
                      disabled={isCreatingFilter}
                      onKeyDown={(e) => e.key === "Enter" && handleCreateCategory()}
                    />
                    <button
                      onClick={handleCreateCategory}
                      disabled={!newCategoryName.trim() || isCreatingFilter}
                      className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-1"
                    >
                      <PlusIcon className="w-4 h-4" />
                      Criar
                    </button>
                  </div>
                </div>

                {/* Create Subcategory */}
                <div className="space-y-2">
                  <label className="block text-xs font-semibold text-gray-700 uppercase">
                    Nova Subcategoria
                  </label>
                  <div className="flex gap-2">
                    <Select
                      value={newSubcategoryCategoryId}
                      onChange={(e) => setNewSubcategoryCategoryId(e.target.value)}
                      options={categoryOptions}
                      placeholder="Categoria"
                      className="w-32"
                      disabled={isCreatingFilter}
                    />
                    <Input
                      value={newSubcategoryName}
                      onChange={(e) => setNewSubcategoryName(e.target.value)}
                      placeholder="Ex: Manga Longa"
                      className="flex-1"
                      disabled={isCreatingFilter}
                      onKeyDown={(e) => e.key === "Enter" && handleCreateSubcategory()}
                    />
                    <button
                      onClick={handleCreateSubcategory}
                      disabled={!newSubcategoryName.trim() || !newSubcategoryCategoryId || isCreatingFilter}
                      className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-1"
                    >
                      <PlusIcon className="w-4 h-4" />
                      Criar
                    </button>
                  </div>
                </div>

                {/* Create Size */}
                <div className="space-y-2">
                  <label className="block text-xs font-semibold text-gray-700 uppercase">
                    Novo Tamanho
                  </label>
                  <div className="flex gap-2">
                    <Input
                      value={newSizeName}
                      onChange={(e) => setNewSizeName(e.target.value)}
                      placeholder="Ex: GG"
                      className="flex-1"
                      disabled={isCreatingFilter}
                      onKeyDown={(e) => e.key === "Enter" && handleCreateSize()}
                    />
                    <button
                      onClick={handleCreateSize}
                      disabled={!newSizeName.trim() || isCreatingFilter}
                      className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-1"
                    >
                      <PlusIcon className="w-4 h-4" />
                      Criar
                    </button>
                  </div>
                </div>

                {/* Create Color */}
                <div className="space-y-2">
                  <label className="block text-xs font-semibold text-gray-700 uppercase">
                    Nova Cor
                  </label>
                  <div className="flex gap-2">
                    <Input
                      value={newColorName}
                      onChange={(e) => setNewColorName(e.target.value)}
                      placeholder="Ex: Azul Marinho"
                      className="flex-1"
                      disabled={isCreatingFilter}
                      onKeyDown={(e) => e.key === "Enter" && handleCreateColor()}
                    />
                    <button
                      onClick={handleCreateColor}
                      disabled={!newColorName.trim() || isCreatingFilter}
                      className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-1"
                    >
                      <PlusIcon className="w-4 h-4" />
                      Criar
                    </button>
                  </div>
                </div>
              </div>
            )}
          </AdminCard>
        )}

        <AdminCard
          icon={<InformationCircleIcon className="w-5 h-5 text-primary" />}
          title="Informações Gerais"
        >
          <div className="space-y-6">
            <Input
              label="Nome do Produto"
              placeholder="Ex: Camiseta DACC Oficial"
              value={product.name || ""}
              onChange={(e) => onChange("name", e.target.value)}
              required
              disabled={isReadonly}
            />
            <div className="space-y-4">
              <Input
                label="Descrição Curta"
                multiline
                rows={3}
                placeholder="Um breve resumo que aparece na listagem..."
                value={product.description || ""}
                onChange={(e) => onChange("description", e.target.value)}
                required
                disabled={isReadonly}
              />
              <Input
                label="Descrição Detalhada"
                multiline
                rows={6}
                placeholder="Conteúdo completo sobre as especificações do produto..."
                value={product.detailedDescription || ""}
                onChange={(e) =>
                  onChange("detailedDescription", e.target.value)
                }
                disabled={isReadonly}
              />
            </div>
            <TagInput
              label="Perfeito Para (Ocasiões de Uso)"
              tags={product.perfectFor || []}
              onAddTag={(tag) =>
                onChange("perfectFor", [...(product.perfectFor || []), tag])
              }
              onRemoveTag={(tag) =>
                onChange(
                  "perfectFor",
                  (product.perfectFor || []).filter((t) => t !== tag),
                )
              }
              disabled={isReadonly}
            />
          </div>
        </AdminCard>

        <AdminCard
          icon={<SwatchIcon className="w-5 h-5 text-primary" />}
          title="Variações de Estoque e Imagens"
          actions={
            !isReadonly && (
              <button
                onClick={addVariation}
                className="text-xs font-bold text-blue-600 hover:text-blue-800 uppercase"
              >
                + Adicionar Variação
              </button>
            )
          }
        >
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200">
              <thead>
                <tr>
                  <th className="px-3 py-2 text-left text-xs font-medium text-gray-500 uppercase">
                    Cor / Tamanho
                  </th>
                  <th className="px-3 py-2 text-left text-xs font-medium text-gray-500 uppercase">
                    SKU
                  </th>
                  <th className="px-3 py-2 text-left text-xs font-medium text-gray-500 uppercase w-20">
                    Estoque
                  </th>
                  <th className="px-3 py-2 text-left text-xs font-medium text-gray-500 uppercase">
                    Mídia
                  </th>
                  {!isReadonly && (
                    <th className="px-3 py-2 text-right text-xs font-medium text-gray-500 uppercase w-10"></th>
                  )}
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-200 bg-white">
                {(product.variations || []).map((variant, idx) => (
                  <tr
                    key={variant.id || idx}
                    className="group hover:bg-gray-50"
                  >
                    <td className="px-3 py-4 whitespace-nowrap">
                      <div className="flex gap-2">
                        <Select
                          className="w-32 py-1! text-xs!"
                          value={variant.color}
                          onChange={(e) =>
                            updateVariation(idx, "color", e.target.value)
                          }
                          options={colorOptions}
                          placeholder="Cor"
                          disabled={isReadonly}
                        />
                        <Select
                          className="w-20 py-1! text-xs!"
                          value={variant.size}
                          onChange={(e) =>
                            updateVariation(idx, "size", e.target.value)
                          }
                          options={sizeOptions}
                          placeholder="Tam"
                          disabled={isReadonly}
                        />
                      </div>
                    </td>
                    <td className="px-3 py-4">
                      <Input
                        className="py-1! text-xs! font-mono"
                        value={variant.sku}
                        onChange={(e) =>
                          updateVariation(idx, "sku", e.target.value)
                        }
                        placeholder="SKU-AUTO"
                        disabled={isReadonly}
                      />
                    </td>
                    <td className="px-3 py-4">
                      <Input
                        type="number"
                        className="py-1! text-xs! w-20"
                        value={variant.stock}
                        onChange={(e) =>
                          updateVariation(idx, "stock", Number(e.target.value))
                        }
                        disabled={isReadonly}
                      />
                    </td>
                    <td className="px-3 py-4">
                      <button
                        onClick={() => handleOpenGallery(idx)}
                        disabled={isReadonly}
                        className={`flex items-center gap-2 px-3 py-1.5 bg-gray-50 border border-gray-200 rounded-md transition-all group/btn w-full justify-center ${isReadonly ? "" : "hover:bg-gray-100"}`}
                      >
                        <div className="relative">
                          {variant.images && variant.images.length > 0 ? (
                            <img
                              src={
                                typeof variant.images[0] === "string"
                                  ? variant.images[0]
                                  : variant.images[0].url
                              }
                              className="w-6 h-6 rounded object-cover shadow-sm"
                              alt="Preview"
                            />
                          ) : (
                            <div className="w-6 h-6 bg-gray-200 rounded flex items-center justify-center text-gray-400">
                              <svg
                                className="w-3 h-3"
                                fill="none"
                                stroke="currentColor"
                                viewBox="0 0 24 24"
                              >
                                <path
                                  strokeLinecap="round"
                                  strokeLinejoin="round"
                                  strokeWidth="2"
                                  d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"
                                />
                              </svg>
                            </div>
                          )}
                          {variant.images && variant.images.length > 0 && (
                            <span className="absolute -top-1.5 -right-1.5 bg-blue-600 text-white text-[9px] w-3.5 h-3.5 flex items-center justify-center rounded-full font-bold shadow-sm">
                              {variant.images.length}
                            </span>
                          )}
                        </div>
                        <span className="text-xs font-medium text-gray-600 group-hover/btn:text-gray-900">
                          {isReadonly ? "Fotos" : "Editar Fotos"}
                        </span>
                      </button>
                    </td>
                    {!isReadonly && (
                      <td className="px-3 py-4 text-right">
                        <button
                          onClick={() => removeVariation(idx)}
                          className="text-gray-400 hover:text-red-500"
                        >
                          <TrashIcon className="w-4 h-4" />
                        </button>
                      </td>
                    )}
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
          {(!product.variations || product.variations.length === 0) && (
            <div className="bg-yellow-50 p-4 rounded-lg text-center text-sm text-yellow-700 mt-4 underline-offset-4 decoration-dashed underline">
              Esse produto ainda não tem variações. Adicione uma para definir o
              estoque.
            </div>
          )}
        </AdminCard>

        <AdminCard
          icon={<ClipboardDocumentListIcon className="w-5 h-5 text-primary" />}
          title="Especificações Técnicas"
          actions={
            !isReadonly && (
              <button
                onClick={addSpecification}
                className="text-xs font-bold text-blue-600 hover:text-blue-800 uppercase"
              >
                + Adicionar Especificação
              </button>
            )
          }
        >
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200">
              <thead>
                <tr>
                  <th className="px-3 py-2 text-left text-xs font-medium text-gray-500 uppercase w-1/2">
                    Nome
                  </th>
                  <th className="px-3 py-2 text-left text-xs font-medium text-gray-500 uppercase w-1/2">
                    Valor
                  </th>
                  {!isReadonly && (
                    <th className="px-3 py-2 text-right text-xs font-medium text-gray-500 uppercase w-10"></th>
                  )}
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-200 bg-white">
                {(product.specifications || []).map((spec, idx) => (
                  <tr key={idx} className="group hover:bg-gray-50">
                    <td className="px-3 py-4">
                      <Input
                        value={spec.name}
                        className="py-1! text-xs! bg-gray-50"
                        onChange={(e) =>
                          updateSpecification(idx, e.target.value, spec.value)
                        }
                        placeholder="Ex: Material"
                        disabled={isReadonly}
                      />
                    </td>
                    <td className="px-3 py-4">
                      <Input
                        value={spec.value}
                        className="py-1! text-xs!"
                        onChange={(e) =>
                          updateSpecification(idx, spec.name, e.target.value)
                        }
                        placeholder="Ex: Algodão"
                        disabled={isReadonly}
                      />
                    </td>
                    {!isReadonly && (
                      <td className="px-3 py-4 text-right">
                        <button
                          onClick={() => removeSpecification(idx)}
                          className="text-gray-400 hover:text-red-500"
                        >
                          <TrashIcon className="w-4 h-4" />
                        </button>
                      </td>
                    )}
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </AdminCard>
      </div>

      {/* Coluna Lateral */}
      <div className="space-y-8">
        <AdminCard
          icon={<CurrencyDollarIcon className="w-5 h-5 text-primary" />}
          title="Classificação e Preço"
        >
          <div className="space-y-4">
            <Select
              label="Categoria"
              value={product.category || ""}
              onChange={(e) => {
                onChange("category", e.target.value);
                onChange("subcategory", ""); // Reset subcategory when category changes
              }}
              options={categoryOptions}
              disabled={isReadonly || isLoadingFilters}
              placeholder={isLoadingFilters ? "Carregando..." : "Selecione a categoria"}
            />

            <Select
              label="Subcategoria"
              value={product.subcategory || ""}
              onChange={(e) => onChange("subcategory", e.target.value)}
              options={subcategoryOptions}
              disabled={isReadonly || isLoadingFilters || !product.category}
              placeholder={
                !product.category
                  ? "Selecione uma categoria primeiro"
                  : isLoadingFilters
                    ? "Carregando..."
                    : "Selecione a subcategoria"
              }
            />

            <div className="space-y-4 pt-2">
              <Input
                label="Preço Original (R$)"
                type="number"
                value={product.originalPrice || 0}
                onChange={(e) =>
                  handlePriceChange("originalPrice", Number(e.target.value))
                }
                disabled={isReadonly}
              />

              <div className="grid grid-cols-2 gap-4">
                <Input
                  label="Desconto (%)"
                  type="number"
                  value={calculateDiscount(
                    product.originalPrice || 0,
                    product.price || 0,
                  ) ?? ''}
                  onChange={(e) =>
                    handlePriceChange("discount", Number(e.target.value) || 0)
                  }
                  disabled={isReadonly}
                />
                <Input
                  label="Preço Final (R$)"
                  type="number"
                  value={product.price || 0}
                  onChange={(e) =>
                    handlePriceChange("price", Number(e.target.value))
                  }
                  disabled={isReadonly}
                />
              </div>
            </div>

            <div className="flex flex-col pt-2 border-t mt-4 gap-y-4">
              <label
                className={`flex items-center gap-2 ${isReadonly ? "cursor-default" : "cursor-pointer"}`}
              >
                <input
                  type="checkbox"
                  checked={product.active || false}
                  onChange={(e) => onChange("active", e.target.checked)}
                  disabled={isReadonly}
                  className="rounded text-blue-600 focus:ring-blue-500 disabled:opacity-50"
                />
                <span className="text-sm font-medium text-gray-700">
                  Produto Ativo no Site
                </span>
              </label>
              <label
                className={`flex items-center gap-2 ${isReadonly ? "cursor-default" : "cursor-pointer"}`}
              >
                <input
                  type="checkbox"
                  checked={product.featured || false}
                  onChange={(e) => onChange("featured", e.target.checked)}
                  disabled={isReadonly}
                  className="rounded text-blue-600 focus:ring-blue-500 disabled:opacity-50"
                />
                <span className="text-sm font-medium text-gray-700">
                  Produto Destaque no Site
                </span>
              </label>
            </div>
          </div>
        </AdminCard>

        {/* Galeria Modal */}
        <Modal
          isOpen={isGalleryOpen}
          onClose={() => setIsGalleryOpen(false)}
          className="max-w-3xl"
        >
          <ImageGalleryEditor
            title="Gerenciar Fotos da Variação"
            description={
              selectedVariantIndex !== null && product.variations
                ? `${product.variations[selectedVariantIndex].color} - ${product.variations[selectedVariantIndex].size}`
                : undefined
            }
            images={
              selectedVariantIndex !== null && product.variations
                ? product.variations[selectedVariantIndex].images
                : []
            }
            onAddImage={handleAddImageToVariant}
            onRemoveImage={handleRemoveImageFromVariant}
          />
        </Modal>
      </div>
    </div>
  );
}
