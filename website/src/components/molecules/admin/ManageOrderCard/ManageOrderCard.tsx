"use client";

import { Order, OrderStatus } from "@/types";
import { motion } from "framer-motion";
import {
    ClipboardDocumentListIcon,
    MapPinIcon,
    CreditCardIcon,
    ChevronRightIcon,
    TrashIcon
} from "@heroicons/react/24/solid";
import Image from "next/image";

interface ManageOrderCardProps {
    order: Order;
    onViewDetails: (order: Order) => void;
    onDeleteOrder: (order: Order) => void;
    className?: string;
}

const statusColors: Record<OrderStatus, string> = {
    created: "bg-gray-100 text-gray-700",
    pending: "bg-yellow-100 text-yellow-800",
    approved: "bg-blue-100 text-blue-800",
    rejected: "bg-red-100 text-red-800",
    delivered: "bg-green-100 text-green-800",
    cancelled: "bg-gray-200 text-gray-600",
};

const statusLabels: Record<OrderStatus, string> = {
    created: "Criado",
    pending: "Pendente",
    approved: "Aprovado",
    rejected: "Rejeitado",
    delivered: "Entregue",
    cancelled: "Cancelado",
};

export default function ManageOrderCard({
    order,
    onViewDetails,
    onDeleteOrder,
    className = "",
}: ManageOrderCardProps) {

    const formatCurrency = (value: number) => {
        return new Intl.NumberFormat("pt-BR", {
            style: "currency",
            currency: "BRL",
        }).format(value);
    };

    const formatDate = (dateString: string) => {
        try {
            const date = new Date(dateString);
            return new Intl.DateTimeFormat("pt-BR", {
                day: "2-digit",
                month: "short",
                year: "numeric",
                hour: "2-digit",
                minute: "2-digit",
            }).format(date);
        } catch {
            return dateString;
        }
    };

    return (
        <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            whileHover={{ y: -4 }}
            onClick={() => onViewDetails(order)}
            className={`bg-white rounded-xl shadow-sm border border-gray-100 p-5 cursor-pointer hover:shadow-md transition-all duration-200 flex flex-col md:flex-row items-center gap-6 ${className}`}
        >
            {/* User Info Section */}
            <div className="flex items-center gap-4 flex-1 w-full md:w-auto">
                <div className="relative w-12 h-12 rounded-full overflow-hidden bg-gray-100 border border-gray-200 shrink-0">
                    {order.user?.avatar ? (
                        <Image
                            src={order.user.avatar}
                            alt={order.user.name || "User"}
                            fill
                            className="object-cover"
                        />
                    ) : (
                        <div className="w-full h-full flex items-center justify-center bg-blue-100 text-blue-600 font-bold text-lg">
                            {order.user?.name?.charAt(0) || "U"}
                        </div>
                    )}
                </div>
                <div>
                    <h3 className="font-semibold text-gray-900 line-clamp-1">
                        {order.user?.name || "Usuário Desconhecido"} {order.user?.lastName}
                    </h3>
                    <p className="text-xs text-gray-500">
                        Pedido #{order.id.slice(0, 8).toUpperCase()}
                    </p>
                </div>
            </div>

            {/* Date & Items Count */}
            <div className="flex flex-col items-start md:items-center w-full md:w-auto min-w-35">
                <div className="flex items-center gap-1.5 text-xs text-gray-500 mb-1">
                    <ClipboardDocumentListIcon className="w-4 h-4" />
                    <span>{formatDate(order.orderDate)}</span>
                </div>
                <span className="text-sm font-medium text-gray-700 bg-gray-50 px-2 py-0.5 rounded text-center w-full md:w-auto">
                    {order.items?.length || 0} itens
                </span>
            </div>

            {/* Payment & Total */}
            <div className="flex flex-col items-start md:items-end w-full md:w-auto min-w-30">
                <span className="text-lg font-bold text-gray-900">
                    {formatCurrency(order.totalAmount)}
                </span>
                <div className="flex items-center gap-1 text-xs text-gray-500">
                    <CreditCardIcon className="w-3 h-3" />
                    <span className="capitalize">{order.paymentMethod || "Pagamento"}</span>
                </div>
            </div>

            {/* Status Badge */}
            <div className="w-full md:w-auto flex justify-between md:justify-center items-center">
                <span className={`px-3 py-1 rounded-full text-xs font-bold uppercase tracking-wide ${statusColors[order.status] || "bg-gray-100 text-gray-800"}`}>
                    {statusLabels[order.status] || order.status}
                </span>

                {/* Mobile Arrow only */}
                <ChevronRightIcon className="w-5 h-5 text-gray-400 md:hidden" />
            </div>

            {/* Action Arrow (Desktop) */}
            <div className="hidden md:flex items-center justify-center gap-3 pl-2 border-l border-gray-100 h-10">
                <motion.button
                    whileHover={{ scale: 1.1 }}
                    whileTap={{ scale: 0.9 }}
                    onClick={(e) => { e.stopPropagation(); onDeleteOrder(order); }}
                    className="p-2 rounded-xl bg-neutral-100 hover:bg-red-100 text-red-600 transition-colors"
                >
                    <TrashIcon className="w-5 h-5" />
                </motion.button>
                <div className="p-2 rounded-full bg-gray-50 text-gray-400 group-hover:bg-blue-50 group-hover:text-blue-600 transition-colors">
                    <ChevronRightIcon className="w-5 h-5" />
                </div>
            </div>

        </motion.div>
    );
}
