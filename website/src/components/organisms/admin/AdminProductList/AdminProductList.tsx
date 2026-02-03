'use client';

import { useState, useEffect, useCallback } from 'react';
import { Product } from '@/types';
import { apiService } from '@/services/api';
import { ManageProductCard } from "@/components";
import { AdminListManager } from '../AdminListManager';

interface AdminProductListProps {
  filters?: {
    category?: string;
    sortBy?: string;
    search?: string;
  };
  filterComponent?: React.ReactNode;
  className?: string;
  onDeleteProduct: (product: Product) => void;
}

const PRODUCTS_PER_PAGE = 9;

export function AdminProductList({
  filters = {},
  filterComponent,
  className = '',
  onDeleteProduct
}: AdminProductListProps) {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [totalItems, setTotalItems] = useState(0);
  const [error, setError] = useState<string | null>(null);

  const loadProducts = useCallback(async (currentPage: number) => {
    try {
      setLoading(true);
      const { products, totalCount } = await apiService.getProducts({
        ...filters,
        page: currentPage,
        limit: PRODUCTS_PER_PAGE,
      });

      setProducts(products);
      setTotalItems(totalCount);
      setError(null);
    } catch (err) {
      setError('Erro ao carregar produtos. Tente novamente.');
      console.error('Error loading products:', err);
    } finally {
      setLoading(false);
    }
  }, [filters]);

  useEffect(() => {
    setPage(1);
    loadProducts(1);
  }, [filters.category, filters.sortBy, filters.search, loadProducts]);

  const handlePageChange = (newPage: number) => {
    setPage(newPage);
    loadProducts(newPage);
  };

  const totalPages = Math.ceil(totalItems / PRODUCTS_PER_PAGE);

  return (
    <AdminListManager
      filters={filterComponent}
      totalItems={totalItems}
      currentPage={page}
      totalPages={totalPages}
      onPageChange={handlePageChange}
      isLoading={loading}
      resourceName="produtos"
      emptyMessage="Nenhum produto encontrado"
      gridClassName="flex flex-col gap-4"
      className={className}
      skeleton={
        <div className="flex flex-col gap-4">
          {[...Array(5)].map((_, i) => (
            <div key={i} className="animate-pulse bg-white p-4 rounded-xl border border-gray-100 shadow-sm flex items-center gap-4">
              <div className="bg-gray-200 w-24 h-24 rounded-lg flex-shrink-0" />
              <div className="flex-1 space-y-2">
                <div className="h-5 bg-gray-200 rounded w-1/3" />
                <div className="h-4 bg-gray-200 rounded w-1/4" />
              </div>
            </div>
          ))}
        </div>
      }
    >
      {products.map((product) => (
        <ManageProductCard
          key={product.id}
          product={product}
          onDeleteProduct={onDeleteProduct}
        />
      ))}
    </AdminListManager>
  );
}
