import { AdminCard, Select } from "@/components";
import { Order, OrderStatus, ValidStatus } from "@/types";
import { statusLabels } from "@/utils/order";

interface OrderFormProps {
    order: Partial<Order>;
    onStatusChange: (status: OrderStatus) => void;
    mode?: 'create' | 'edit' | 'view';
}

export default function OrderForm({ order, onStatusChange, mode = 'edit' }: OrderFormProps) {
    const isReadonly = mode === 'view';

    if (!order) return null;

    return (
        <div className="space-y-6">
            <AdminCard title="Status do Pedido">
                <div className="space-y-4">
                    <p className="text-sm text-gray-500">
                        Altere o status do pedido para atualizar o cliente sobre o andamento.
                    </p>
                    <Select
                        label="Status Atual"
                        value={order.status || ''}
                        onChange={(e) => onStatusChange(e.target.value as OrderStatus)}
                        options={ValidStatus.map(status => ({
                            label: statusLabels[status as OrderStatus] || status,
                            value: status
                        }))}
                        disabled={isReadonly}
                    />
                </div>
            </AdminCard>
        </div>
    );
}
