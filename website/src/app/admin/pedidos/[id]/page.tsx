"use client"

import { EditPageHeader, PageLoader, ConfirmationModal, AdminCard, OrderForm } from "@/components";
import { apiService } from "@/services/api";
import { Order, OrderStatus } from "@/types";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { toast } from "sonner";
import { statusColors, statusLabels } from "@/utils/order";
import Image from "next/image";
import { formatDate } from "@/utils";

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

  const updateOrderStatus = (newStatus: string) => {
    if (order) {
      setOrder({ ...order, status: newStatus as OrderStatus });
    }
  };

  const handleSaveChanges = async () => {
    if (!order) return;

    setLoading(true);
    try {
      await apiService.updateOrderStatus(order.id, order.status);
      toast.success("Pedido atualizado com sucesso!");
    } catch (error) {
      console.error(error);
      toast.error("Erro ao atualizar pedido");
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

  if (!order) {
    return <div className="flex items-center justify-center">
      <h1 className="text-2xl font-bold text-primary">Pedido não encontrado</h1>
    </div>
  }

  const subtotal = order.items?.reduce((acc, item) => acc + (item.unitPrice * item.quantity), 0) || 0;
  const discountAmount = subtotal - order.totalAmount;

  return (
    <section>
      <EditPageHeader
        title={`Pedido ${order?.id}`}
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

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">

          {/* Coluna da Esquerda (Principal) */}
          <div className="lg:col-span-2 space-y-8">
            <AdminCard title="Informações do Pedido">
              <div className="space-y-4">
                <div className="flex justify-between items-center border-b border-gray-100 pb-2">
                  <span className="font-semibold text-gray-600">ID do Pedido:</span>
                  <span className="text-gray-900 font-mono text-sm">{order.id}</span>
                </div>

                <div className="flex justify-between items-center border-b border-gray-100 pb-2">
                  <span className="font-semibold text-gray-600">Data:</span>
                  <span className="text-gray-900">{formatDate(order.orderDate)}</span>
                </div>

                <div className="flex justify-between items-center border-b border-gray-100 pb-2">
                  <span className="font-semibold text-gray-600">Subtotal:</span>
                  <span className="text-gray-900 font-medium">
                    {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(subtotal)}
                  </span>
                </div>

                {discountAmount > 0 && (
                  <div className="flex justify-between items-center border-b border-gray-100 pb-2">
                    <span className="font-semibold text-gray-600">Desconto:</span>
                    <span className="text-red-600 font-medium">
                      -{new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(discountAmount)}
                    </span>
                  </div>
                )}

                <div className="flex justify-between items-center border-b border-gray-100 pb-2">
                  <span className="font-semibold text-gray-600">Total Pago:</span>
                  <span className="text-lg font-bold text-green-600">
                    {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(order.totalAmount)}
                  </span>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4 pt-2 pb-4">
                  <div className="flex flex-col gap-1">
                    <span className="text-xs font-bold text-gray-500 uppercase">Método de Pagamento</span>
                    <div className="h-10 flex items-center text-gray-900 font-medium bg-gray-50 px-3 rounded-lg border border-gray-100">
                      {order.paymentMethod || 'Não informado'}
                    </div>
                  </div>
                  <div className="flex flex-col gap-1">
                    <span className="text-xs font-bold text-gray-500 uppercase">ID do Pagamento</span>
                    <div className="h-10 flex items-center text-gray-900 font-mono text-xs bg-gray-50 px-3 rounded-lg border border-gray-100 truncate" title={String(order.mercadopagoPaymentId || 'N/A')}>
                      {order.mercadopagoPaymentId || 'N/A'}
                    </div>
                  </div>
                </div>

                <OrderForm
                  order={order}
                  onStatusChange={(status) => setOrder({ ...order, status })}
                  mode="edit"
                />
              </div>
            </AdminCard>

            <AdminCard title={`Itens do Pedido (${order.items?.length || 0})`}>
              <div className="space-y-4">
                {order.items?.map((item) => (
                  <div key={item.id} className="flex gap-4 p-3 bg-gray-50 rounded-lg border border-gray-100 hover:border-gray-200 transition-colors">
                    <div className="relative w-16 h-16 flex-shrink-0 bg-white rounded-md overflow-hidden border border-gray-200">
                      <Image
                        src={item.productImage || 'https://i.postimg.cc/WzRPmW3r/LOGO-DACC-OFICIAL.png'}
                        alt={item.productName || 'Produto'}
                        fill
                        className="object-cover"
                      />
                    </div>

                    <div className="flex-1 min-w-0">
                      <p className="font-medium text-gray-900 text-sm line-clamp-2" title={item.productName}>
                        {item.productName}
                      </p>
                      <div className="flex flex-wrap gap-2 mt-1 text-xs text-gray-500">
                        <span className="bg-white px-2 py-0.5 rounded border border-gray-200">
                          Tam: {item.variationSize}
                        </span>
                        <span className="bg-white px-2 py-0.5 rounded border border-gray-200 flex items-center gap-1">
                          Cor: <span className="w-2 h-2 rounded-full inline-block bg-gray-400" /> {item.variationColor}
                        </span>
                      </div>
                    </div>

                    <div className="text-right">
                      <p className="font-medium text-gray-900">
                        {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(item.unitPrice)}
                      </p>
                      <p className="text-xs text-gray-500">x {item.quantity}</p>
                      <p className="text-xs font-bold text-gray-700 mt-1">
                        {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(item.unitPrice * item.quantity)}
                      </p>
                    </div>
                  </div>
                ))}
              </div>
            </AdminCard>
          </div>

          {/* Coluna da Direita */}
          <div className="space-y-8">
            <AdminCard title="Cliente">
              <div className="flex items-center gap-4 mb-6">
                <div className="relative w-16 h-16 rounded-full overflow-hidden border-2 border-primary-100">
                  <Image
                    src={order.user.avatar || 'https://i.postimg.cc/WzRPmW3r/LOGO-DACC-OFICIAL.png'}
                    alt={order.user.name}
                    fill
                    className="object-cover"
                  />
                </div>
                <div>
                  <h3 className="font-bold text-gray-900 text-lg">{order.user.name} {order.user.lastName}</h3>
                  <p className="text-sm text-gray-500">{order.user.email}</p>
                </div>
              </div>

              <div className="space-y-3 text-sm">
                <div className="flex items-center gap-2">
                  <span className="w-20 text-gray-500 font-medium">Telefone:</span>
                  <span className="text-gray-900">{order.user.phone || 'Não informado'}</span>
                </div>
                <div className="flex items-center gap-2">
                  <span className="w-20 text-gray-500 font-medium">RA:</span>
                  <span className="text-gray-900 font-mono bg-gray-100 px-2 py-0.5 rounded">{order.user.ra || 'N/A'}</span>
                </div>
                <div className="flex items-center gap-2">
                  <span className="w-20 text-gray-500 font-medium">ID:</span>
                  <span className="text-gray-500 font-mono text-xs truncate" title={order.user.id}>{order.user.id}</span>
                </div>
              </div>
            </AdminCard>

            {order.coupon && (
              <AdminCard title="Cupom Aplicado">
                <div className="bg-green-50 border border-green-100 rounded-lg p-4">
                  <div className="flex justify-between items-start mb-2">
                    <div>
                      <span className="font-mono font-bold text-green-700 text-lg tracking-wider block">
                        {order.coupon.code}
                      </span>
                      <span className="text-xs text-green-600 uppercase font-semibold">
                        {order.coupon.discountType === 'porcentagem' ? 'Desconto %' : 'Desconto Fixo'}
                      </span>
                    </div>
                    <div className="bg-white text-green-700 font-bold px-3 py-1 rounded shadow-sm">
                      {order.coupon.discountType === 'porcentagem' ? `-${order.coupon.value}%` : `-R$ ${order.coupon.value}`}
                    </div>
                  </div>

                  <div className="border-t border-green-200 my-2 pt-2 space-y-1 text-xs text-green-800">
                    <div className="flex justify-between">
                      <span>Validade:</span>
                      <span className="font-medium">{order.coupon.expirationDate ? formatDate(order.coupon.expirationDate) : 'Indeterminado'}</span>
                    </div>
                    <div className="flex justify-between">
                      <span>Usos:</span>
                      <span className="font-medium">{order.coupon.currentUsage} / {order.coupon.usageLimit || '∞'}</span>
                    </div>
                  </div>
                </div>
              </AdminCard>
            )}
          </div>

        </div>
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
