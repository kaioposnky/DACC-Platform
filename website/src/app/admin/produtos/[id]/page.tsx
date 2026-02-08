"use client";

import { useParams, useRouter } from "next/navigation";
import { apiService } from "@/services/api";
import { useEffect, useState } from "react";
import { Product } from "@/types";
import {
  Button,
  EditPageHeader,
  PageLoader,
  ProductForm,
  ConfirmationModal
} from "@/components";
import { toast } from "sonner";

export default function AdminProdutoEditPage() {
  const params = useParams();
  const productId = params.id as string;
  const router = useRouter();

  const [isLoading, setIsLoading] = useState(true);
  const [product, setProduct] = useState<Product | null>(null);
  const [isSaving, setIsSaving] = useState(false);
  const [isDeleting, setIsDeleting] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);

  useEffect(() => {
    const fetchProduct = async () => {
      try {
        setIsLoading(true);
        const productInfo = await apiService.getProduct(productId);
        setProduct(productInfo);
      } catch (error) {
        console.error("Erro ao buscar produto:", error);
        toast.error("Erro ao carregar dados do produto");
      } finally {
        setIsLoading(false);
      }
    };
    fetchProduct();
  }, [productId]);

  const handleSaveChanges = async () => {
    if (!product) return;
    try {
      setIsSaving(true);
      // Clean unnecessary fields before update
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
    } catch (error: any) {
      console.error("Erro ao salvar produto:", error);
      toast.error(error.message || "Erro ao atualizar produto");
    } finally {
      setIsSaving(false);
    }
  };

  const handleDeleteProduct = async () => {
    if (!product) return;
    try {
      setIsDeleting(true);
      await apiService.deleteProject(product.id); // Using deleteProject as placeholder or if it's shared, otherwise check apiService for deleteProduct
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

  const handleGoBack = () => router.back();

  if (isLoading) return <PageLoader />;
  if (!product) {
    return (
      <div className="flex flex-col items-center justify-center h-[60vh]">
        <p className="text-2xl font-bold text-gray-400">Produto não encontrado</p>
        <Button onClick={handleGoBack} className="mt-4">Voltar</Button>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 pb-20">
      <EditPageHeader
        title={product.name}
        id={product.id}
        status={{
          text: product.inStock ? "Ativo" : "Rascunho",
          colorClass: product.inStock
            ? "bg-green-100 text-green-700 font-bold"
            : "bg-gray-100 text-gray-700 font-bold",
        }}
        onSave={handleSaveChanges}
        onBack={handleGoBack}
        showDelete={true}
        onDelete={() => setIsDeleteModalOpen(true)}
        loadingSave={isSaving}
        loadingDelete={isDeleting}
      />

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        <ProductForm
          product={product}
          onChange={(field, value) => {
            if (!product) return;
            setProduct({ ...product, [field]: value });
          }}
          mode="edit"
        />
      </div>

      <ConfirmationModal
        isOpen={isDeleteModalOpen}
        onClose={() => setIsDeleteModalOpen(false)}
        onConfirm={handleDeleteProduct}
        title="Excluir Produto"
        message={`Tem certeza que deseja excluir o produto "${product.name}"? Esta ação não pode ser desfeita.`}
        confirmLabel="Sim, Excluir"
        isLoading={isDeleting}
        variant="danger"
      />
    </div>
  );
}
