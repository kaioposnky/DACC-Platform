"use client";

import { useParams, useRouter } from "next/navigation";
import { apiService } from "@/services/api";
import { useEffect, useState } from "react";
import { Product, ProductReview, ProductSpecification, ProductVariation, ProductBatchUpdateRequest } from "@/types";
import {
  Button,
  Footer,
  Input,
  Navigation,
  ShoppingCart,
  EditPageHeader,
  AdminCard,
  TagInput,
  Select,
  Modal,
  ImageGalleryEditor,
} from "@/components";
import { ConfirmationModal } from "@/components/molecules/admin/ConfirmationModal";
import { TrashIcon } from "@heroicons/react/24/outline";
import { toast } from "sonner";

export default function AdminProdutoEditPage() {
  const params = useParams();
  const productId = params.id;
  const router = useRouter();

  if (!productId) {
    return <div className="flex flex-col items-center justify-center h-screen">
      <p className="text-2xl font-bold">Produto não encontrado</p>
      <Button onClick={() => router.back()}>Voltar</Button>
    </div>;
  }

  const [isLoading, setIsLoading] = useState(true);
  const [product, setProduct] = useState<Product | null>(null);
  const [loading, setLoading] = useState(false);

  // Controle do Modal de Galeria
  const [isGalleryOpen, setIsGalleryOpen] = useState(false);
  const [selectedVariantIndex, setSelectedVariantIndex] = useState<
    number | null
  >(null);

  // States para exclusão
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);

  useEffect(() => {
    setIsLoading(true);
    const fetchProduct = async () => {
      try {
        const productInfo = await apiService.getProduct(productId as string);
        setProduct(productInfo);
      } catch (error) {
        console.error("Erro ao buscar produto:", error);
        toast.error("Erro ao buscar produto: " + error);
      } finally {
        setIsLoading(false);
      }
    };
    fetchProduct();
  }, [productId]);

  if (!product && !isLoading) {
    return <div className="flex flex-col items-center justify-center h-full">
      <p className="text-2xl font-bold text-primary">Produto não encontrado</p>
      <Button onClick={() => router.back()}>Voltar</Button>
    </div>;
  }

  if (!product && isLoading) {
    return <div className="flex flex-col items-center justify-center h-full">
      <p className="text-2xl font-bold text-primary">Carregando produto...</p>
    </div>;
  }

  if (!product) {
    return <div className="flex flex-col items-center justify-center h-full">
      <p className="text-2xl font-bold text-primary">Produto não encontrado</p>
      <Button onClick={() => router.back()}>Voltar</Button>
    </div>;
  }

  const handleGoBack = () => router.back();

  const handleSaveChanges = async () => {
    try {
      setLoading(true);
      const {
        rating,
        reviews,
        image,
        stockCount,
        images,
        colors,
        sizes,
        reviewsList,
        createdAt,
        ...updateData
      } = product;

      await apiService.updateProductFull(product.id, updateData as any);
      toast.success("Produto atualizado com sucesso!");
    } catch (err: any) {
      console.error("Erro ao salvar produto:", err);
      toast.error(err.message || "Erro ao atualizar produto");
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteProduct = async () => {
    if (!product) return;

    try {
      setIsDeleting(true);
      await apiService.deleteProduct(product.id);
      toast.success("Produto excluído com sucesso!");
      router.push("/admin/produtos");
    } catch (error: any) {
      console.error("Erro ao excluir produto:", error);
      toast.error(error.message || "Erro ao excluir produto");
    } finally {
      setIsDeleting(false);
      setIsDeleteModalOpen(false);
    }
  };

  const handleAddTag = (tag: string) => {
    setProduct({
      ...product,
      perfectFor: [...(product.perfectFor || []), tag],
    });
  };

  const handleRemoveTag = (tagToRemove: string) => {
    setProduct({
      ...product,
      perfectFor: product.perfectFor?.filter((t: string) => t !== tagToRemove),
    });
  };

  // Lógica de Preços
  const calculateDiscount = (orig: number, final: number) => {
    if (!orig || orig === 0) return 0;
    return Math.round(((orig - final) / orig) * 100);
  };

  const handlePriceChange = (field: "price" | "originalPrice" | "discount", value: number) => {
    const currentPrice = product.price || 0;
    const currentOriginal = product.originalPrice || currentPrice;

    if (field === "originalPrice") {
      const discount = calculateDiscount(currentOriginal, currentPrice);
      const newPrice = value * (1 - discount / 100);
      setProduct({ ...product, originalPrice: value, price: Number(newPrice.toFixed(2)) });
    } else if (field === "price") {
      setProduct({ ...product, price: value });
    } else if (field === "discount") {
      const newPrice = currentOriginal * (1 - value / 100);
      setProduct({ ...product, price: Number(newPrice.toFixed(2)) });
    }
  };

  const updateVariation = (index: number, field: keyof ProductVariation, value: any) => {
    if (!product.variations) return;
    const newVariations = [...product.variations];
    newVariations[index] = { ...newVariations[index], [field]: value };
    setProduct({ ...product, variations: newVariations });
  };

  const removeVariation = (index: number) => {
    if (!product.variations) return;
    const newVariations = product.variations.filter(
      (_: ProductVariation, i: number) => i !== index,
    );
    setProduct({ ...product, variations: newVariations });
  };

  const addVariation = () => {
    setProduct({
      ...product,
      variations: [
        ...(product.variations || []),
        {
          id: `temp-${Date.now()}`,
          color: "",
          size: "",
          stock: 0,
          sku: "",
          images: [],
        },
      ],
    });
  };

  // Funções da Galeria
  const handleOpenGallery = (index: number) => {
    setSelectedVariantIndex(index);
    setIsGalleryOpen(true);
  };

  const handleAddImageToVariant = (imageUrl: string) => {
    if (!imageUrl || selectedVariantIndex === null || !product.variations) return;

    const newVariations = [...product.variations];
    const currentImages = newVariations[selectedVariantIndex].images || [];

    newVariations[selectedVariantIndex] = {
      ...newVariations[selectedVariantIndex],
      images: [
        ...currentImages,
        {
          url: imageUrl,
          order: currentImages.length,
          id: undefined
        }
      ],
    };

    setProduct({ ...product, variations: newVariations });
  };

  const handleRemoveImageFromVariant = (imageIndex: number) => {
    if (selectedVariantIndex === null || !product.variations) return;

    const newVariations = [...product.variations];
    newVariations[selectedVariantIndex].images = newVariations[
      selectedVariantIndex
    ].images.filter((_: any, i: number) => i !== imageIndex);

    setProduct({ ...product, variations: newVariations });
  };

  const addSpecification = () => {
    setProduct({ ...product, specifications: [...product.specifications ?? [], { name: "Insira um nome", value: "Insira um valor" }] });
  };

  const handleSpecificationChange = (index: number, spec: ProductSpecification) => {
    const newSpecifications = [...product.specifications ?? []];
    newSpecifications[index] = spec;
    setProduct({ ...product, specifications: newSpecifications });
  };

  const removeSpecification = (index: number) => {
    if (!product.specifications) return;
    const newSpecs = product.specifications.filter(
      (_: ProductSpecification, i: number) => i !== index,
    );
    setProduct({ ...product, specifications: newSpecs });
  };

  if (loading) return <div>Carregando...</div>;

  return (
    <div className="min-h-screen bg-gray-50 pb-12">
      <EditPageHeader
        title={product.name}
        id={product.id}
        status={{
          text: product.inStock ? "Ativo" : "Rascunho",
          colorClass: product.inStock
            ? "bg-green-100 text-green-700"
            : "bg-gray-100 text-gray-700",
        }}
        onSave={handleSaveChanges}
        onBack={handleGoBack}
        showDelete={true}
        onDelete={() => setIsDeleteModalOpen(true)}
        loadingSave={loading}
        loadingDelete={isDeleting}
      />

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* COLUNA ESQUERDA (PRINCIPAL) */}
          <div className="lg:col-span-2 space-y-8">
            {/* 1. INFO GERAL */}
            <AdminCard title="Informações Gerais">
              <div className="space-y-6">
                <Input
                  label="Nome do Produto"
                  value={product.name}
                  onChange={(e) =>
                    setProduct({ ...product, name: e.target.value })
                  }
                />
                <div className="space-y-4">
                  <Input
                    label="Descrição Curta"
                    multiline
                    rows={3}
                    value={product.description}
                    onChange={(e) =>
                      setProduct({ ...product, description: e.target.value })
                    }
                  />
                  <Input
                    label="Descrição Detalhada"
                    multiline
                    rows={6}
                    value={product.detailedDescription}
                    onChange={(e) =>
                      setProduct({
                        ...product,
                        detailedDescription: e.target.value,
                      })
                    }
                  />
                </div>
                <TagInput
                  label="Perfeito Para (Ocasiões de Uso)"
                  tags={product.perfectFor || []}
                  onAddTag={handleAddTag}
                  onRemoveTag={handleRemoveTag}
                />
              </div>
            </AdminCard>

            {/* 2. GERENCIADOR DE VARIAÇÕES */}
            <AdminCard
              title="Variações de Estoque e Imagens"
              actions={
                <button
                  onClick={addVariation}
                  className="text-xs font-bold text-blue-600 hover:text-blue-800 uppercase"
                >
                  + Adicionar Variação
                </button>
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
                      <th className="px-3 py-2 text-right text-xs font-medium text-gray-500 uppercase w-10"></th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-gray-200 bg-white">
                    {(product.variations || []).map((variant: ProductVariation, idx: number) => (
                      <tr key={variant.id} className="group hover:bg-gray-50">
                        <td className="px-3 py-4 whitespace-nowrap">
                          <div className="flex gap-2">
                            <Select
                              className="w-32 py-1! text-xs!"
                              value={variant.color}
                              onChange={(e) =>
                                updateVariation(idx, "color", e.target.value)
                              }
                              options={[
                                { label: "Preto Fosco", value: "Preto Fosco" },
                                { label: "Branco", value: "Branco" },
                                { label: "Azul", value: "Azul" },
                              ]}
                            />
                            <Select
                              className="w-20 py-1! text-xs!"
                              value={variant.size}
                              onChange={(e) =>
                                updateVariation(idx, "size", e.target.value)
                              }
                              options={["PP", "P", "M", "G", "GG", "XG"].map(
                                (s) => ({ label: s, value: s }),
                              )}
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
                          />
                        </td>
                        <td className="px-3 py-4">
                          <Input
                            type="number"
                            className="py-1! text-xs! w-20"
                            value={variant.stock}
                            onChange={(e) =>
                              updateVariation(
                                idx,
                                "stock",
                                Number(e.target.value),
                              )
                            }
                          />
                        </td>
                        <td className="px-3 py-4">
                          <div className="flex items-center gap-2">
                            <button
                              onClick={() => handleOpenGallery(idx)}
                              className="flex items-center gap-2 px-3 py-1.5 bg-gray-50 hover:bg-gray-100 border border-gray-200 rounded-md transition-all group/btn w-full justify-center"
                            >
                              <div className="relative">
                                {variant.images.length > 0 ? (
                                  <img
                                    src={variant.images[0].url}
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
                                      ></path>
                                    </svg>
                                  </div>
                                )}
                                {variant.images.length > 0 && (
                                  <span className="absolute -top-1.5 -right-1.5 bg-blue-600 text-white text-[9px] w-3.5 h-3.5 flex items-center justify-center rounded-full font-bold shadow-sm">
                                    {variant.images.length}
                                  </span>
                                )}
                              </div>
                              <span className="text-xs font-medium text-gray-600 group-hover/btn:text-gray-900">
                                Fotos
                              </span>
                            </button>
                          </div>
                        </td>
                        <td className="px-3 py-4 text-right">
                          <button
                            onClick={() => removeVariation(idx)}
                            className="text-gray-400 hover:text-red-500"
                          >
                            <TrashIcon className="w-4 h-4" />
                          </button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
              {(!product.variations || product.variations.length === 0) && (
                <div className="bg-yellow-50 p-4 rounded-lg text-center text-sm text-yellow-700 mt-4">
                  Esse produto ainda não tem variações cadastradas. Adicione uma
                  para definir o estoque.
                </div>
              )}
            </AdminCard>

            {/* 3. ESPECIFICAÇÕES */}
            <AdminCard
              title="Especificações Técnicas"
              className="justify-center"
              actions={
                <button
                  onClick={addSpecification}
                  className="text-xs font-bold text-blue-600 hover:text-blue-800 uppercase"
                >
                  + Adicionar Especificação
                </button>
              }
            >
              {/* Mantendo lógica antiga mas simplificada */}
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
                      <th className="px-3 py-2 text-right text-xs font-medium text-gray-500 uppercase w-10"></th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-gray-200 bg-white">
                    {product.specifications?.map((spec: any, idx: number) => (
                      <tr key={idx} className="group hover:bg-gray-50">
                        <td className="px-3 py-4 align-top">
                          <Input
                            value={spec.name}
                            className="py-1! text-xs! bg-gray-50"
                            onChange={(e) => handleSpecificationChange(idx, { name: e.target.value, value: spec.value })}
                            placeholder="Ex: Material"
                          />
                        </td>
                        <td className="px-3 py-4 align-top">
                          <Input
                            onChange={(e) => handleSpecificationChange(idx, { name: spec.name, value: e.target.value })}
                            value={spec.value}
                            className="py-1! text-xs!"
                            placeholder="Ex: Algodão"
                          />
                        </td>
                        <td className="px-3 py-4 text-right align-middle">
                          <button
                            onClick={() => removeSpecification(idx)}
                            className="text-gray-400 hover:text-red-500"
                          >
                            <TrashIcon className="w-4 h-4" />
                          </button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            </AdminCard>
          </div>

          {/* COLUNA DIREITA */}
          <div className="space-y-8">
            <AdminCard title="Classificação e Preço">
              <div className="space-y-4">
                <Select
                  label="Categoria (Subcategoria)"
                  value={product.category}
                  onChange={(e) =>
                    setProduct({ ...product, category: e.target.value })
                  }
                  options={[
                    { label: "Camisetas", value: "tshirts" },
                    { label: "Moletons", value: "hoodies" },
                    { label: "Canecas", value: "mugs" },
                  ]}
                />

                <div className="space-y-4 pt-2">
                  <Input
                    label="Preço Original (R$)"
                    type="number"
                    value={product.originalPrice ?? undefined}
                    onChange={(e) => handlePriceChange("originalPrice", Number(e.target.value))}
                  />

                  <div className="grid grid-cols-2 gap-4">
                    <Input
                      label="Desconto (%)"
                      type="number"
                      value={calculateDiscount(product.originalPrice || 0, product.price)}
                      onChange={(e) => handlePriceChange("discount", Number(e.target.value))}
                    />
                    <Input
                      label="Preço Final (R$)"
                      type="number"
                      value={product.price}
                      onChange={(e) => handlePriceChange("price", Number(e.target.value))}
                    />
                  </div>
                </div>

                <div className="pt-2 border-t mt-4">
                  <label className="flex items-center gap-2 cursor-pointer">
                    <input
                      type="checkbox"
                      checked={product.inStock}
                      onChange={(e) =>
                        setProduct({ ...product, inStock: e.target.checked })
                      }
                      className="rounded text-blue-600 focus:ring-blue-500"
                    />
                    <span className="text-sm font-medium text-gray-700">
                      Produto Ativo no Site
                    </span>
                  </label>
                </div>
              </div>
            </AdminCard>

            <AdminCard title="Envio e Entrega (DESABILITADO)">
              <div className="space-y-4 opacity-70 pointer-events-none grayscale">
                <Input
                  label="Prazo Estimado"
                  value="DESABILITADO"
                  onChange={() => { }}
                />
                <Input
                  label="Política"
                  multiline
                  value="DESABILITADO"
                  onChange={() => { }}
                />
              </div>
            </AdminCard>
          </div>
        </div>
      </div>

      {/* MODAL DE GALERIA */}
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

      {/* MODAL DE CONFIRMAÇÃO DE EXCLUSÃO */}
      <ConfirmationModal
        isOpen={isDeleteModalOpen}
        onClose={() => setIsDeleteModalOpen(false)}
        onConfirm={handleDeleteProduct}
        title="Excluir Produto"
        message={`Tem certeza que deseja excluir o produto "${product?.name}"? Esta ação não pode ser desfeita.`}
        confirmLabel="Sim, Excluir"
        cancelLabel="Cancelar"
        isLoading={isDeleting}
        variant="danger"
      />
    </div>
  );
}
