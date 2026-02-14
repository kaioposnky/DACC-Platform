"use client";

import {
  AdminProductList,
  ProductFilter,
  type ProductFilterOptions,
  Button,
} from "@/components";
import { apiService } from "@/services/api";
import { Product } from "@/types";
import { toast } from "sonner";
import { PlusIcon } from "@heroicons/react/24/outline";
import { useState, useCallback } from "react";
import Link from "next/link";

export default function AdminProdutosPage() {
  const [filters, setFilters] = useState<ProductFilterOptions>({
    category: "all",
    sortBy: "featured",
    searchQuery: "",
  });

  const handleFilterChange = useCallback((newFilters: ProductFilterOptions) => {
    setFilters(newFilters);
  }, []);

  const handleDeleteProduct = (product: Product) => {
    apiService
      .deleteProduct(product.id)
      .then((response) => {
        toast.success("Produto deletado com sucesso!");
      })
      .catch((error) => {
        toast.success("Ocorreu um erro ao deletar o produto!");
      });
  };

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      {/* 1. Header */}
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">
            Gerenciar Produtos
          </h1>
          <p className="text-gray-500 text-sm">
            Adicione, edite ou remova produtos da sua loja.
          </p>
        </div>
        <Link href="/admin/produtos/new">
          <Button variant="primary" className="flex items-center gap-2">
            <PlusIcon className="w-5 h-5" />
            Novo Produto
          </Button>
        </Link>
      </div>

      {/* 2. LISTA DE PRODUTOS INTEGRADA COM FILTROS */}
      <AdminProductList
        filters={filters}
        filterComponent={<ProductFilter onFilterChange={handleFilterChange} />}
        onDeleteProduct={handleDeleteProduct}
      />
    </div>
  );
}
