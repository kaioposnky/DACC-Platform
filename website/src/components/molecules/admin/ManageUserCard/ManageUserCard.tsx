"use client";

import { User } from "@/types";
import { AdminCard, Button } from "@/components";
import { UserCircleIcon, PencilIcon, TrashIcon, CheckCircleIcon, XCircleIcon } from "@heroicons/react/24/outline";
import Link from "next/link";
import { formatDate } from "@/utils/formatters";

interface ManageUserCardProps {
    user: User;
}

export const ManageUserCard = ({ user }: ManageUserCardProps) => {
    return (
        <AdminCard title="" className="h-full flex flex-col justify-between group hover:border-primary/50 transition-colors">
            <div className="space-y-4">
                {/* Header: Avatar + Info Principal */}
                <div className="flex items-start justify-between gap-4">
                    <div className="flex items-center gap-3">
                        {user.avatar ? (
                            <img
                                src={user.avatar}
                                alt={user.name}
                                className="w-12 h-12 rounded-full object-cover border border-gray-100"
                            />
                        ) : (
                            <div className="w-12 h-12 rounded-full bg-primary/10 flex items-center justify-center text-primary">
                                <UserCircleIcon className="w-8 h-8" />
                            </div>
                        )}
                        <div>
                            <h3 className="font-bold text-gray-900 line-clamp-1" title={user.name}>
                                {user.name}
                            </h3>
                            <p className="text-sm text-gray-500 line-clamp-1" title={user.email}>
                                {user.email}
                            </p>
                        </div>
                    </div>

                    <div className={`px-2 py-1 rounded-full text-xs font-semibold border ${user.isActive
                        ? 'bg-green-50 text-green-700 border-green-200'
                        : 'bg-red-50 text-red-700 border-red-200'
                        }`}>
                        {user.isActive ? 'Ativo' : 'Inativo'}
                    </div>
                </div>

                {/* Detalhes: Cargo, Curso, Data */}
                <div className="space-y-2 text-sm text-gray-600">
                    <div className="flex justify-between border-b border-gray-50 pb-2">
                        <span>Cargo:</span>
                        <span className="font-medium text-gray-900">{user.role || 'N/A'}</span>
                    </div>
                    <div className="flex justify-between border-b border-gray-50 pb-2">
                        <span>Curso:</span>
                        <span className="font-medium text-gray-900">{user.course || 'N/A'}</span>
                    </div>
                    <div className="flex justify-between border-b border-gray-50 pb-2">
                        <span>Cadastro:</span>
                        <span className="font-medium text-gray-900">{user.createdAt ? formatDate(user.createdAt) : 'N/A'}</span>
                    </div>
                </div>
            </div>

            {/* Ações */}
            <div className="pt-4 mt-4 border-t border-gray-100 flex gap-2">
                <Link href={`/admin/usuarios/${user.id}`} className="flex-1">
                    <Button variant="secondary" className="w-full text-sm">
                        <PencilIcon className="w-4 h-4 mr-2" />
                        Editar
                    </Button>
                </Link>
            </div>
        </AdminCard>
    );
};
