"use client";

import { Event } from "@/types";
import { motion } from "framer-motion";
import { useRouter } from "next/navigation";
import { PencilIcon, TrashIcon, CalendarIcon, ClockIcon } from "@heroicons/react/24/solid";

interface ManageEventCardProps {
    event: Event;
    onDelete: (event: Event) => void;
    className?: string;
}

export default function ManageEventCard({
    event,
    onDelete,
    className = "",
}: ManageEventCardProps) {
    const router = useRouter();

    const formatDate = (dateString: string) => {
        try {
            const date = new Date(dateString);
            const day = date.getDate().toString().padStart(2, "0");
            const month = date.toLocaleDateString("pt-BR", { month: "short" }).replace(".", "").toUpperCase();
            return { day, month };
        } catch (e) {
            return { day: "--", month: "---" };
        }
    };

    const { day, month } = formatDate(event.date);

    const handleEdit = () => {
        router.push(`/admin/conteudo/eventos/${event.id}`);
    };

    return (
        <motion.div
            initial={{ opacity: 0, y: 10 }}
            animate={{ opacity: 1, y: 0 }}
            whileHover={{ y: -2 }}
            className={`bg-white border border-gray-100 rounded-xl p-4 shadow-sm hover:shadow-md transition-all flex items-center gap-4 ${className}`}
        >
            {/* Date Box */}
            <div className="bg-primary/5 rounded-lg p-2 min-w-15 flex flex-col items-center justify-center border border-primary/10">
                <span className="text-xl font-bold text-primary">{day}</span>
                <span className="text-[10px] font-bold text-primary/70">{month}</span>
            </div>

            {/* Info */}
            <div className="flex-1 min-w-0">
                <div className="flex items-center gap-2 mb-1">
                    <span className="text-[10px] font-bold uppercase tracking-wider bg-gray-100 text-gray-600 px-2 py-0.5 rounded">
                        {event.type || "Evento"}
                    </span>
                </div>
                <h3 className="font-bold text-gray-900 truncate" title={event.title}>
                    {event.title}
                </h3>
                <div className="flex items-center gap-3 mt-1 underline-none">
                    <div className="flex items-center text-xs text-gray-500 gap-1">
                        <ClockIcon className="w-3.5 h-3.5" />
                        {event.time}
                    </div>
                    <div className="flex items-center text-xs text-gray-500 gap-1">
                        <CalendarIcon className="w-3.5 h-3.5" />
                        {new Date(event.date).toLocaleDateString('pt-BR')}
                    </div>
                </div>
            </div>

            {/* Actions */}
            <div className="flex items-center gap-2">
                <button
                    onClick={handleEdit}
                    className="p-2 bg-blue-50 text-blue-600 rounded-lg hover:bg-blue-100 transition-colors"
                    title="Editar Evento"
                >
                    <PencilIcon className="w-4 h-4" />
                </button>
                <button
                    onClick={() => onDelete(event)}
                    className="p-2 bg-red-50 text-red-600 rounded-lg hover:bg-red-100 transition-colors"
                    title="Excluir Evento"
                >
                    <TrashIcon className="w-4 h-4" />
                </button>
            </div>
        </motion.div>
    );
}
