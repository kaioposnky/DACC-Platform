'use client'

import { EditPageHeader, ProductForm } from "@/components";
import { apiService } from "@/services/api";
import { Product } from "@/types";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { toast } from "sonner";

export default function AdminProdutoNewPage() {
  const router = useRouter();
  const [product, setProduct] = useState<Partial<Product>>({
    id: '',
    name: '',
    description: '',
    detailedDescription: '',
    perfectFor: [],
    price: 0,
    originalPrice: 0,
    category: '',
    inStock: false,
    featured: false,
    specifications: [],
    variations: [],
  });
  const [isCreating, setIsCreating] = useState(false);

  const handleGoBack = () => router.back();
  const handleCreate = () => {
    setIsCreating(true);
    const {
      rating,
      reviews,
      image,
      stockCount,
      images,
      colors,
      sizes,
      active,
      reviewsList,
      createdAt,
      ...createData
    } = product;

    apiService.createProduct(createData as any)
      .then(() => {
        toast.success('Produto criado com sucesso!');
        setIsCreating(false);
        router.push('/admin/produtos');
      })
      .catch(() => {
        toast.error('Erro ao criar produto!');
        setIsCreating(false);
      });
  };
  const handleProductChange = (field: keyof Product, value: any) => {
    setProduct(prevProduct => ({
      ...prevProduct,
      [field]: value
    }));
  };

  return (
    <div>
      <EditPageHeader
        title="Novo Produto"
        label="Criando"
        onBack={handleGoBack}
        onSave={handleCreate}
        loadingSave={isCreating}
        saveButtonText="Criar Novo Produto"
      />

      <div className="px-4 sm:px-6 lg:px-50 mt-8 mb-20">
        <ProductForm
          product={product}
          onChange={handleProductChange}
          mode="create"
        />
      </div>
    </div>
  )
}
