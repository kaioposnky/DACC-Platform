"use client"

import { AdminProductList, ProductFilter, type ProductFilterOptions } from "@/components";
import { apiService } from "@/services/api";
import { Product } from "@/types";
import { toast } from "sonner";
import { PlusIcon } from "@heroicons/react/24/outline";
import { useState } from "react";

export default function AdminProdutosPage() {

    const [filters, setFilters] = useState<ProductFilterOptions>({
        category: 'all',
        sortBy: 'featured',
        searchQuery: ''
    });

    const handleFilterChange = (filters: ProductFilterOptions) => {
        setFilters(filters);
    }

    const handleDeleteProduct = (product: Product) => {
        apiService.deleteProduct(product.id)
            .then((response) => {
                toast.success("Produto deletado com sucesso!");
            }).catch((error) => {
                toast.success("Ocorreu um erro ao deletar o produto!");
            });
    }

    return (
        <div className="p-6 max-w-7xl mx-auto space-y-6">

            {/* 1. Header */}
            <div className="flex justify-between items-center">
                <div>
                    <h1 className="text-2xl font-bold text-gray-900">Gerenciar Produtos</h1>
                    <p className="text-gray-500 text-sm">Adicione, edite ou remova produtos da sua loja.</p>
                </div>
                <button className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg flex items-center gap-2 font-semibold transition-all">
                    <PlusIcon className="w-5 h-5" />
                    Novo Produto
                </button>
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
