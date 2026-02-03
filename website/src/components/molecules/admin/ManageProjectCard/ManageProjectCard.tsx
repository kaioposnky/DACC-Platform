"use client";

import { Project } from "@/types";
import { motion } from "framer-motion";
import { useRouter } from "next/navigation";
import { PencilIcon, TrashIcon, RocketLaunchIcon, CheckCircleIcon, PlayCircleIcon } from "@heroicons/react/24/solid";

interface ManageProjectCardProps {
    project: Project;
    onDelete: (project: Project) => void;
    className?: string;
}

const statusConfig = {
    in_progress: {
        label: "Em Andamento",
        color: "bg-blue-100 text-blue-700",
        icon: PlayCircleIcon,
    },
    completed: {
        label: "Concluído",
        color: "bg-green-100 text-green-700",
        icon: CheckCircleIcon,
    },
    planned: {
        label: "Planejado",
        color: "bg-gray-100 text-gray-700",
        icon: RocketLaunchIcon,
    },
};

export default function ManageProjectCard({
    project,
    onDelete,
    className = "",
}: ManageProjectCardProps) {
    const router = useRouter();
    const config = statusConfig[project.status] || statusConfig.planned;

    const handleEdit = () => {
        router.push(`/admin/projetos/${project.id}`);
    };

    return (
        <motion.div
            initial={{ opacity: 0, y: 10 }}
            animate={{ opacity: 1, y: 0 }}
            whileHover={{ y: -2 }}
            className={`bg-white border border-gray-100 rounded-xl p-4 shadow-sm hover:shadow-md transition-all flex items-center gap-4 ${className}`}
        >
            {/* Icon Area */}
            <div className={`w-12 h-12 rounded-xl flex items-center justify-center flex-shrink-0 ${config.color.replace('text-', 'bg-').replace('100', '10')}`}>
                <config.icon className={`w-7 h-7 ${config.color.split(' ')[1]}`} />
            </div>

            {/* Info */}
            <div className="flex-1 min-w-0">
                <div className="flex items-center gap-2 mb-1">
                    <span className={`text-[10px] font-bold uppercase tracking-wider px-2 py-0.5 rounded ${config.color}`}>
                        {config.label}
                    </span>
                    <span className="text-[10px] font-medium text-gray-400">
                        {project.technologies?.slice(0, 2).join(' • ')}
                    </span>
                </div>
                <h3 className="font-bold text-gray-900 truncate" title={project.title}>
                    {project.title}
                </h3>

                {/* Progress Bar */}
                <div className="mt-2 w-full max-w-[200px]">
                    <div className="flex justify-between items-center mb-1">
                        <span className="text-[10px] text-gray-500 font-medium">Progresso</span>
                        <span className="text-[10px] text-gray-700 font-bold">{project.progress}%</span>
                    </div>
                    <div className="w-full h-1.5 bg-gray-100 rounded-full overflow-hidden">
                        <motion.div
                            initial={{ width: 0 }}
                            animate={{ width: `${project.progress}%` }}
                            className={`h-full ${config.color.split(' ')[1].replace('text-', 'bg-')}`}
                        />
                    </div>
                </div>
            </div>

            {/* Actions */}
            <div className="flex items-center gap-2">
                <button
                    onClick={handleEdit}
                    className="p-2 bg-blue-50 text-blue-600 rounded-lg hover:bg-blue-100 transition-colors"
                    title="Editar Projeto"
                >
                    <PencilIcon className="w-4 h-4" />
                </button>
                <button
                    onClick={() => onDelete(project)}
                    className="p-2 bg-red-50 text-red-600 rounded-lg hover:bg-red-100 transition-colors"
                    title="Excluir Projeto"
                >
                    <TrashIcon className="w-4 h-4" />
                </button>
            </div>
        </motion.div>
    );
}
