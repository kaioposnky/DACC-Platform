"use client"

import { EditPageHeader, PageLoader, ConfirmationModal, AdminCard } from "@/components";
import { apiService } from "@/services/api";
import { Order, OrderStatus } from "@/types";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { toast } from "sonner";
import { statusColors, statusLabels } from "@/utils/order";

export default function PedidoEditAdminPage() {
  const params = useParams();
  const router = useRouter();
  const [order, setOrder] = useState<Order | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  // States for actions
  const [loading, setLoading] = useState(false); // Save loading
  const [isDeleting, setIsDeleting] = useState(false);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);

  useEffect(() => {
    const fetchOrder = async () => {
      try {
        const response = await apiService.getOrder(params.id as string);
        setOrder(response);
      } catch (error) {
        console.error(error);
        toast.error("Erro ao buscar pedido");
      } finally {
        setIsLoading(false);
      }
    };
    fetchOrder();
  }, [params.id]);

  const handleGoBack = () => {
    router.back();
  };

  const handleSaveChanges = async () => {
    // Implementar lógica de atualização do pedido se houver campos editáveis
    setLoading(true);
    try {
      // Simulação de delay
      await new Promise(resolve => setTimeout(resolve, 1000));
      toast.success("Alterações salvas com sucesso!");
    } catch (error) {
      toast.error("Erro ao salvar alterações");
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteOrder = async () => {
    if (!order) return;
    setIsDeleting(true);
    try {
      await apiService.deleteOrder(order.id);
      toast.success("Pedido excluído com sucesso!");
      router.push("/admin/pedidos");
    } catch (error) {
      console.error(error);
      toast.error("Erro ao excluir pedido");
    } finally {
      setIsDeleting(false);
      setIsDeleteModalOpen(false);
    }
  };

  if (isLoading) {
    return <PageLoader />;
  }

  if (!order || !isLoading) {
    return <div className="flex flex-col items-center justify-center h-full">
      <h1 className="text-2xl font-bold font-primary">Pedido não encontrado</h1>
    </div>
  }

  return (
    <section>
      <EditPageHeader
        title={`Pedido ${order?.id} feito por ${order?.user?.name}`}
        id={order.id}
        status={{
          text: statusLabels[order.status as OrderStatus] || order.status,
          colorClass: statusColors[order.status as OrderStatus] || "bg-gray-100 text-gray-700",
        }}
        onSave={handleSaveChanges}
        onBack={handleGoBack}
        showDelete={true}
        onDelete={() => setIsDeleteModalOpen(true)}
        loadingSave={loading}
        loadingDelete={isDeleting}
      />

      <div className="flex justify-around items-center">
        <AdminCard
          title="Itens do Pedido"
        >
          <ul className="list-disc list-inside">
            {order.items?.map(item => (
              <li key={item.id}>{item.productName} - {item.quantity}x</li>
            ))}
          </ul>
        </AdminCard>
      </div>

      {/* Conteúdo do Pedido (A ser implementado: Detalhes dos Itens, Endereço, etc) */}
      <div className="mt-8 p-6 bg-white rounded-xl border border-gray-100 shadow-sm">
        <p className="text-gray-500 italic">Detalhes do pedido em construção...</p>
      </div>

      <ConfirmationModal
        isOpen={isDeleteModalOpen}
        onClose={() => setIsDeleteModalOpen(false)}
        onConfirm={handleDeleteOrder}
        isLoading={isDeleting}
        title="Excluir Pedido"
        message={`Tem certeza que deseja excluir este pedido? Esta ação não pode ser desfeita.`}
        confirmLabel="Excluir"
      />
    </section>
  )
}
