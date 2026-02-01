'use client';

import { useState, useEffect, useCallback } from 'react';
import { Order } from '@/types';
import { apiService } from '@/services/api';
import { motion } from 'framer-motion';
import { ArrowPathIcon } from '@heroicons/react/24/outline';
import { ManageOrderCard } from '@/components/molecules/admin/ManageOrderCard';
import { useRouter } from 'next/navigation';
import { AdminListManager } from '../AdminListManager';

interface AdminOrderListProps {
  filters?: {
    status?: string;
    searchQuery?: string;
    startDate?: string;
    endDate?: string;
  };
  filterComponent?: React.ReactNode;
  className?: string;
  onDeleteOrder: (order: Order) => void;
}

const ORDERS_PER_PAGE = 9;

export function AdminOrderList({
  filters = {},
  filterComponent,
  className = '',
  onDeleteOrder
}: AdminOrderListProps) {
  const router = useRouter();

  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [totalItems, setTotalItems] = useState(0);
  const [error, setError] = useState<string | null>(null);

  const loadOrders = useCallback(async (currentPage: number) => {
    try {
      setLoading(true);
      const response = await apiService.searchOrders({
        ...filters,
        page: currentPage,
        limit: ORDERS_PER_PAGE,
      } as any);

      // Em um cenário real, o backend retornaria o total de itens. 
      // Aqui simulamos com o tamanho da resposta para fins de demonstração.
      setOrders(response);
      setTotalItems(response.length * 2); // Simulação de mais páginas
      setError(null);
    } catch (err) {
      setError('Erro ao carregar pedidos. Tente novamente.');
      console.error('Error loading orders:', err);
    } finally {
      setLoading(false);
    }
  }, [filters]);

  useEffect(() => {
    setPage(1);
    setOrders([]);
    setError(null);
    loadOrders(1);
  }, [filters.status, filters.searchQuery, filters.startDate, filters.endDate, loadOrders]);

  const handlePageChange = (newPage: number) => {
    setPage(newPage);
    loadOrders(newPage);
  };

  const handleViewDetails = (order: Order) => {
    router.push(`/admin/pedidos/${order.id}`);
  };

  const totalPages = Math.ceil(totalItems / ORDERS_PER_PAGE);

  return (
    <AdminListManager
      filters={filterComponent}
      totalItems={totalItems}
      currentPage={page}
      totalPages={totalPages}
      onPageChange={handlePageChange}
      isLoading={loading}
      resourceName="pedidos"
      emptyMessage="Nenhum pedido encontrado"
      gridClassName="flex flex-col gap-4"
      className={className}
      skeleton={
        <div className="flex flex-col gap-4">
          {[...Array(3)].map((_, i) => (
            <div key={i} className="h-48 bg-white animate-pulse rounded-2xl border border-gray-100" />
          ))}
        </div>
      }
    >
      {orders.map((order) => (
        <ManageOrderCard
          key={order.id}
          order={order}
          onViewDetails={handleViewDetails}
          onDeleteOrder={onDeleteOrder}
        />
      ))}
    </AdminListManager>
  );
}
