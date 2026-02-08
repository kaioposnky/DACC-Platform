"use client";

import {
  AdminProductList,
  ProductFilter,
  type ProductFilterOptions,
  Button,
  AdminListManager,
  SearchBar,
} from "@/components";
import { apiService } from "@/services/api";
import { Product, ProductReview } from "@/types";
import { toast } from "sonner";
import { PlusIcon } from "@heroicons/react/24/outline";
import { useState, useCallback, useEffect } from "react";
import { useDebounce } from "@/hooks/useDebounce";
import { ManageProductReviewCard } from "@/components/molecules/admin/ManageProductReviewCard";

export default function AdminAvaliacoesPage() {
  const [query, setQuery] = useState("");
  const [reviews, setReviews] = useState<ProductReview[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [pagination, setPagination] = useState({
    currentPage: 1,
    totalPages: 1,
    totalItems: 0,
    itemsPerPage: 9,
  });

  const debouncedSearch = useDebounce(query, 600);

  const fetchRatings = useCallback(async () => {
    setIsLoading(true);
    try {
      const response = await apiService.getReviews({
        search: debouncedSearch,
        page: pagination.currentPage,
        limit: pagination.itemsPerPage,
      });

      setReviews(response.reviews);
      setPagination((prev) => ({
        ...prev,
        totalItems: response.totalCount,
        totalPages: Math.ceil(response.totalCount / prev.itemsPerPage),
      }));
    } catch (error) {
      toast.error("Erro ao carregar professores");
    } finally {
      setIsLoading(false);
    }
  }, [query, pagination.currentPage, pagination.itemsPerPage]);

  useEffect(() => {
    fetchRatings();
  }, [fetchRatings]);

  const handleQueryChange = useCallback((query: string) => {
    setQuery(query);
  }, []);

  const handlePageChange = useCallback((page: number) => {
    setPagination(prev => ({ ...prev, currentPage: page }));
  }, []);

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      {/* 1. Header */}
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">
            Gerenciar Avaliações
          </h1>
          <p className="text-gray-500 text-sm">
            Veja as avaliações dos produtos da plataforma.
            Não é possível excluir avaliações por questões de transparência.
          </p>
        </div>
      </div>

      {/* 2. LISTA DE AVALIAÇÕES INTEGRADA COM FILTROS */}
      <AdminListManager
        isLoading={isLoading}
        totalItems={pagination.totalItems}
        currentPage={pagination.currentPage}
        totalPages={pagination.totalPages}
        onPageChange={handlePageChange}
        resourceName="avaliações"
        emptyMessage="Nenhuma avaliação encontrada com os filtros selecionados."
        gridClassName="flex flex-col gap-4"
        filters={
          <SearchBar
            onSearch={handleQueryChange}
            placeholder="Pesquisar avaliações..."
          />
        }
      >
        {/* CHILDREN */}
        {reviews.map((review) => (
          <ManageProductReviewCard
            key={review.id}
            review={review}
          />
        ))}
      </AdminListManager>
    </div>
  );
}
