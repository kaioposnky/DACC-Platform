'use client'

import { useState, useEffect } from 'react'
import { motion } from 'framer-motion'
import { Footer, Navigation } from "@/components"
import { OrderHistoryBanner } from '@/components/organisms/OrderHistoryBanner'
import { OrderHistoryFilter } from '@/components/organisms/OrderHistoryFilter'
import { OrderHistoryPagination } from '@/components/organisms/OrderHistoryPagination'
import { useAuth } from '@/context/AuthContext'
import { apiService } from '@/services/api'
import { Order } from '@/types'
import { toast } from 'sonner'
import Link from 'next/link'

const formatCurrency = (value: number) => {
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(value);
};

export default function OrderHistoryPage() {
  const { user } = useAuth();
  const [orders, setOrders] = useState<Order[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;

  useEffect(() => {
    const fetchOrders = async () => {
      if (!user?.id) return;
      try {
        setIsLoading(true);
        const data = await apiService.getOrdersByUserId(user.id);
        setOrders(data || []);
      } catch (error) {
        console.error("Erro ao buscar histórico de pedidos:", error);
        toast.error("Não foi possível carregar os pedidos.");
      } finally {
        setIsLoading(false);
      }
    };
    fetchOrders();
  }, [user]);

  const totalPages = Math.ceil(orders.length / itemsPerPage);
  const paginatedOrders = orders.slice((currentPage - 1) * itemsPerPage, currentPage * itemsPerPage);

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
    window.scrollTo({ top: 300, behavior: 'smooth' });
  }

  const handleFilterChange = (filters: {
    status: string
    dateRange: string
    searchTerm: string
  }) => {
    // Filter logic would go here
    console.log('Filters changed:', filters)
    setCurrentPage(1); // Reset to first page when filters change
  }

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'created': return 'bg-gray-100 text-gray-800 border-gray-200';
      case 'pending': return 'bg-yellow-100 text-yellow-800 border-yellow-200';
      case 'approved': return 'bg-emerald-100 text-emerald-800 border-emerald-200';
      case 'rejected': return 'bg-red-100 text-red-800 border-red-200';
      case 'delivered': return 'bg-blue-100 text-blue-800 border-blue-200';
      case 'cancelled': return 'bg-gray-200 text-gray-700 border-gray-300';
      default: return 'bg-gray-100 text-gray-800 border-gray-200';
    }
  };

  const translateStatus = (status: string) => {
    const map: Record<string, string> = {
      'created': 'Criado',
      'pending': 'Pendente',
      'approved': 'Aprovado',
      'rejected': 'Recusado',
      'delivered': 'Entregue',
      'cancelled': 'Cancelado'
    };
    return map[status] || status;
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />

      <OrderHistoryBanner />
      <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-8">

        <OrderHistoryFilter onFilterChange={handleFilterChange} />

        {isLoading ? (
          <div className="flex justify-center items-center py-20">
            <div className="animate-spin rounded-full h-8 w-8 border-t-2 border-b-2 border-primary"></div>
          </div>
        ) : orders.length === 0 ? (
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.2 }}
            className="bg-white rounded-lg shadow-sm border border-gray-200 p-8"
          >
            <div className="text-center">
              <div className="text-gray-400 mb-4">
                <svg className="h-16 w-16 mx-auto" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
                </svg>
              </div>
              <h3 className="text-lg font-medium text-gray-900 mb-2">Sem pedidos</h3>
              <p className="text-gray-600">
                Seu histórico de pedidos aparecerá aqui assim que você realizar uma compra.
              </p>
            </div>
          </motion.div>
        ) : (
          <div className="space-y-4">
            {paginatedOrders.map((order, i) => (
              <motion.div
                key={order.id}
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.3, delay: i * 0.1 }}
                className="bg-white rounded-lg shadow-sm border border-gray-200 overflow-hidden"
              >
                <div className="p-6 border-b border-gray-100 flex flex-col md:flex-row md:items-center justify-between gap-4">
                  <div>
                    <div className="text-sm text-gray-500 mb-1">
                      Pedido feito em <span className="font-medium text-gray-900">{new Date(order.orderDate).toLocaleDateString()}</span>
                    </div>
                    <div className="text-xs text-gray-400">ID: {order.id.slice(0, 8).toUpperCase()}</div>
                  </div>

                  <div className="flex items-center gap-4">
                    <div className="text-right">
                      <div className="text-sm text-gray-500">Total</div>
                      <div className="font-semibold text-gray-900">{formatCurrency(order.totalAmount)}</div>
                    </div>
                    <div className={`px-3 py-1 rounded-full text-xs font-semibold border ${getStatusColor(order.status)}`}>
                      {translateStatus(order.status)}
                    </div>
                  </div>
                </div>

                <div className="p-6">
                  <div className="flex flex-col gap-4">
                    {order.items?.map((item) => (
                      <div key={item.id} className="flex gap-4 items-center">
                        <div className="w-16 h-16 bg-gray-100 rounded-md flex-shrink-0 overflow-hidden">
                          {item.productImage && (
                            <img src={item.productImage} alt={item.productName} className="w-full h-full object-cover" />
                          )}
                        </div>
                        <div className="flex-1">
                          <Link href={`/loja/${item.productId}`} className="font-medium text-gray-900 hover:text-primary transition-colors">
                            {item.productName}
                          </Link>
                          <div className="text-sm text-gray-500 mt-1">
                            {item.variationColor && <span>Cor: {item.variationColor}</span>}
                            {item.variationColor && item.variationSize && <span> | </span>}
                            {item.variationSize && <span>Tamanho: {item.variationSize}</span>}
                            <span className="md:hidden ml-2">x {item.quantity}</span>
                          </div>
                        </div>
                        <div className="hidden md:flex flex-col items-end">
                          <div className="text-sm text-gray-500">Qtd: {item.quantity}</div>
                          <div className="font-medium text-gray-900">{formatCurrency(item.unitPrice)}</div>
                        </div>
                      </div>
                    ))}
                  </div>
                </div>
              </motion.div>
            ))}
          </div>
        )}

        {totalPages > 1 && (
          <OrderHistoryPagination
            currentPage={currentPage}
            totalPages={totalPages}
            onPageChange={handlePageChange}
          />
        )}
      </div>


      <Footer />
    </div>
  )
}
