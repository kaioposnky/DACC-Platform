"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { EditPageHeader, AdminOrderList, ConfirmationModal } from "@/components";
import { OrderFilter, OrderFilterOptions } from "@/components/molecules/OrderFilter/OrderFilter";
import { Order } from "@/types";
import { apiService } from "@/services/api";
import { toast } from "sonner";

export default function PedidosAdminPage() {
  const router = useRouter();
  const [filters, setFilters] = useState<OrderFilterOptions>({
    searchQuery: "",
    status: "all",
    startDate: "",
    endDate: "",
  });

  // Modal de Exclusão
  const [orderToDelete, setOrderToDelete] = useState<Order | null>(null);
  const [isDeleting, setIsDeleting] = useState(false);

  const handleGoBack = () => router.back();

  const handleFilterChange = (newFilters: OrderFilterOptions) => {
    setFilters(newFilters);
  };

  const handleDeleteOrder = async () => {
    if (!orderToDelete) return;

    try {
      setIsDeleting(true);
      await apiService.deleteOrder(orderToDelete.id);
      toast.success("Pedido excluído com sucesso!");

      setFilters({ ...filters });
      setOrderToDelete(null);
    } catch (error: any) {
      console.error("Erro ao excluir pedido:", error);
      toast.error(error.message || "Erro ao excluir pedido");
    } finally {
      setIsDeleting(false);
    }
  };

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      {/* 1. Header */}
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Gerenciar Pedidos</h1>
          <p className="text-gray-500 text-sm">Visualize, filtre e gerencie os pedidos dos seus clientes.</p>
        </div>
      </div>

      {/* 2. LISTA DE PEDIDOS INTEGRADA COM FILTROS */}
      <AdminOrderList
        filters={filters}
        filterComponent={<OrderFilter onFilterChange={handleFilterChange} />}
        onDeleteOrder={setOrderToDelete}
      />

      {/* Modal de Confirmação de Exclusão */}
      <ConfirmationModal
        isOpen={!!orderToDelete}
        onClose={() => setOrderToDelete(null)}
        onConfirm={handleDeleteOrder}
        isLoading={isDeleting}
        title="Excluir Pedido"
        message={`Tem certeza que deseja excluir o pedido #${orderToDelete?.id.slice(0, 8).toUpperCase()}? Esta ação não pode ser desfeita.`}
        confirmLabel="Excluir Pedido"
      />
    </div>
  );
}
