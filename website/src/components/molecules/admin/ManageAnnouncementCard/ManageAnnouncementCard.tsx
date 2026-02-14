"use client";

import { Announcement } from "@/types";
import { motion } from "framer-motion";
import { useRouter } from "next/navigation";
import { PencilIcon, TrashIcon, MegaphoneIcon, CalendarDaysIcon } from "@heroicons/react/24/solid";
import Image from "next/image";

interface ManageAnnouncementCardProps {
    announcement: Announcement;
    onDelete: (announcement: Announcement) => void;
    className?: string;
}

export default function ManageAnnouncementCard({
    announcement,
    onDelete,
    className = "",
}: ManageAnnouncementCardProps) {
    const router = useRouter();

    const handleEdit = () => {
        router.push(`/admin/conteudo/anuncios/${announcement.id}`);
    };

    const isEvent = announcement.type === 'event';

    return (
        <motion.div
            initial={{ opacity: 0, y: 10 }}
            animate={{ opacity: 1, y: 0 }}
            whileHover={{ y: -2 }}
            className={`bg-white border border-gray-100 rounded-xl p-4 shadow-sm hover:shadow-md transition-all flex items-center gap-4 ${className}`}
        >
            {/* Image/Icon Area */}
            <div className="w-16 h-16 rounded-lg overflow-hidden shrink-0 bg-gray-50 flex items-center justify-center relative">
                {announcement.imageSrc ? (
                    <Image
                        src={announcement.imageSrc}
                        alt={announcement.imageAlt || announcement.title}
                        fill
                        className="object-cover"
                    />
                ) : (
                    <div className={`w-full h-full flex items-center justify-center ${isEvent ? 'bg-purple-50 text-purple-500' : 'bg-blue-50 text-blue-500'}`}>
                        {isEvent ? <CalendarDaysIcon className="w-8 h-8" /> : <MegaphoneIcon className="w-8 h-8" />}
                    </div>
                )}
            </div>

            {/* Info */}
            <div className="flex-1 min-w-0">
                <div className="flex items-center gap-2 mb-1">
                    <span className={`text-[10px] font-bold uppercase tracking-wider px-2 py-0.5 rounded ${isEvent ? 'bg-purple-100 text-purple-700' : 'bg-blue-100 text-blue-700'
                        }`}>
                        {isEvent ? 'Evento' : 'Destaque'}
                    </span>
                    <span className="text-[10px] font-medium text-gray-400">
                        {new Date(announcement.createdAt).toLocaleDateString('pt-BR')}
                    </span>
                </div>
                <h3 className="font-bold text-gray-900 truncate" title={announcement.title}>
                    {announcement.title}
                </h3>
                <p className="text-sm text-gray-500 truncate mt-1">
                    {announcement.content}
                </p>
            </div>

            {/* Actions */}
            <div className="flex items-center gap-2">
                <button
                    onClick={handleEdit}
                    className="p-2 bg-blue-50 text-blue-600 rounded-lg hover:bg-blue-100 transition-colors"
                    title="Editar Anúncio"
                >
                    <PencilIcon className="w-4 h-4" />
                </button>
                <button
                    onClick={() => onDelete(announcement)}
                    className="p-2 bg-red-50 text-red-600 rounded-lg hover:bg-red-100 transition-colors"
                    title="Excluir Anúncio"
                >
                    <TrashIcon className="w-4 h-4" />
                </button>
            </div>
        </motion.div>
    );
}
