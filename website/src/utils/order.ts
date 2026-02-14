import { OrderStatus } from "@/types";

export const statusColors: Record<OrderStatus, string> = {
    created: "bg-blue-50 text-blue-700 border border-blue-200",
    pending: "bg-amber-50 text-amber-700 border border-amber-200",
    approved: "bg-indigo-50 text-indigo-700 border border-indigo-200",
    delivered: "bg-emerald-50 text-emerald-700 border border-emerald-200",
    cancelled: "bg-slate-50 text-slate-600 border border-slate-200",
    rejected: "bg-rose-50 text-rose-700 border border-rose-200",
};

export const statusLabels: Record<OrderStatus, string> = {
    created: "Novo",
    pending: "Pendente",
    approved: "Aprovado",
    delivered: "Concluído",
    cancelled: "Cancelado",
    rejected: "Recusado",
};
