"use client";

import { News } from "@/types";
import { motion } from "framer-motion";
import Image from "next/image";
import { useRouter } from "next/navigation";
import { PencilIcon, TrashIcon, UserIcon, CalendarDaysIcon } from "@heroicons/react/24/solid";

interface ManageNewsCardProps {
    news: News;
    onDelete: (news: News) => void;
    className?: string;
}

export default function ManageNewsCard({
    news,
    onDelete,
    className = "",
}: ManageNewsCardProps) {
    const router = useRouter();

    const handleEdit = () => {
        router.push(`/admin/noticias/${news.id}`);
    };

    return (
        <motion.div
            initial={{ opacity: 0, y: 10 }}
            animate={{ opacity: 1, y: 0 }}
            whileHover={{ y: -2 }}
            className={`bg-white border border-gray-100 rounded-xl p-3 shadow-sm hover:shadow-md transition-all flex items-center gap-4 ${className}`}
        >
            {/* Thumbnail */}
            <div className="relative w-20 h-20 rounded-lg overflow-hidden bg-gray-100 flex-shrink-0 border border-gray-200">
                {news.image ? (
                    <Image
                        src={news.image}
                        alt={news.title}
                        fill
                        className="object-cover"
                    />
                ) : (
                    <div className="w-full h-full flex items-center justify-center text-gray-400">
                        <Image
                            src="https://gerenciador.fei.edu.br/Content/Arquivos/logo_fei_color-01.svg"
                            alt="Logo FEI"
                            width={40}
                            height={40}
                            className="opacity-20 grayscale"
                        />
                    </div>
                )}
            </div>

            {/* Content */}
            <div className="flex-1 min-w-0">
                <div className="flex items-center gap-2 mb-1">
                    <span className="text-[10px] font-bold uppercase tracking-wider bg-secondary/10 text-secondary px-2 py-0.5 rounded">
                        {news.category || "Notícia"}
                    </span>
                    {news.tags?.[0] && (
                        <span className="text-[10px] font-medium text-gray-400">
                            #{news.tags[0]}
                        </span>
                    )}
                </div>
                <h3 className="font-bold text-gray-900 truncate" title={news.title}>
                    {news.title}
                </h3>
                <div className="flex items-center gap-4 mt-1">
                    <div className="flex items-center text-[11px] text-gray-500 gap-1">
                        <UserIcon className="w-3.5 h-3.5 text-gray-400" />
                        {news.author || "DACC"}
                    </div>
                    <div className="flex items-center text-[11px] text-gray-500 gap-1">
                        <CalendarDaysIcon className="w-3.5 h-3.5 text-gray-400" />
                        {new Date(news.date).toLocaleDateString('pt-BR')}
                    </div>
                </div>
            </div>

            {/* Actions */}
            <div className="flex items-center gap-2 pr-1">
                <button
                    onClick={handleEdit}
                    className="p-2 bg-blue-50 text-blue-600 rounded-lg hover:bg-blue-100 transition-colors"
                    title="Editar Notícia"
                >
                    <PencilIcon className="w-4 h-4" />
                </button>
                <button
                    onClick={() => onDelete(news)}
                    className="p-2 bg-red-50 text-red-600 rounded-lg hover:bg-red-100 transition-colors"
                    title="Excluir Notícia"
                >
                    <TrashIcon className="w-4 h-4" />
                </button>
            </div>
        </motion.div>
    );
}
