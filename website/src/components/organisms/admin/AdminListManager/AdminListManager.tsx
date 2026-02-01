"use client";

import { ReactNode } from "react";
import { Pagination } from "@/components/molecules/Pagination";
import { motion } from "framer-motion";

interface AdminListManagerProps {
    filters?: ReactNode;
    children: ReactNode;
    totalItems: number;
    currentPage: number;
    totalPages: number;
    onPageChange: (page: number) => void;
    isLoading: boolean;
    resourceName?: string;
    emptyMessage?: string;
    gridClassName?: string;
    skeleton?: ReactNode;
    className?: string;
}

export const AdminListManager = ({
    filters,
    children,
    totalItems,
    currentPage,
    totalPages,
    onPageChange,
    isLoading,
    resourceName = "itens",
    emptyMessage = "Nenhum item encontrado",
    gridClassName = "grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6",
    skeleton,
    className = "",
}: AdminListManagerProps) => {
    return (
        <div className={`flex flex-col gap-0 ${className}`}>
            {/* 1. SEÇÃO DE FILTROS */}
            {filters && (
                <div className="w-full">
                    {filters}
                </div>
            )}

            {/* 2. CONTEÚDO PRINCIPAL */}
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8 w-full space-y-6">
                {/* Sumário de Resultados */}
                {!isLoading && totalItems > 0 && (
                    <div className="flex flex-col sm:flex-row sm:justify-between sm:items-center gap-2 text-sm text-gray-500 bg-white p-4 rounded-xl border border-gray-100 shadow-sm">
                        <p>
                            Encontramos <span className="font-bold text-primary">{totalItems}</span> {resourceName}
                        </p>
                        {totalPages > 1 && (
                            <p>
                                Página <span className="font-bold text-primary">{currentPage}</span> de{" "}
                                <span className="font-bold text-primary">{totalPages}</span>
                            </p>
                        )}
                    </div>
                )}

                {/* Lista / Grid */}
                <div className="relative min-h-[400px]">
                    {isLoading ? (
                        <div className={gridClassName}>
                            {skeleton || (
                                // Default Skeleton (Cards)
                                [...Array(6)].map((_, i) => (
                                    <div key={i} className="animate-pulse bg-white p-6 rounded-2xl border border-gray-100 shadow-sm space-y-4">
                                        <div className="bg-gray-200 aspect-video rounded-xl" />
                                        <div className="h-5 bg-gray-200 rounded w-3/4" />
                                        <div className="h-4 bg-gray-200 rounded w-1/2" />
                                        <div className="flex justify-between items-center pt-4 border-t border-gray-50">
                                            <div className="h-6 bg-gray-200 rounded w-20" />
                                            <div className="h-8 bg-gray-200 rounded w-24" />
                                        </div>
                                    </div>
                                ))
                            )}
                        </div>
                    ) : totalItems > 0 ? (
                        <motion.div
                            layout
                            initial={{ opacity: 0 }}
                            animate={{ opacity: 1 }}
                            className={gridClassName}
                        >
                            {children}
                        </motion.div>
                    ) : (
                        <div className="flex flex-col items-center justify-center py-20 text-center bg-white rounded-2xl border border-dashed border-gray-200 shadow-sm">
                            <div className="w-16 h-16 bg-gray-50 rounded-full flex items-center justify-center mb-4">
                                <svg className="w-8 h-8 text-gray-300" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4" />
                                </svg>
                            </div>
                            <h3 className="text-lg font-bold text-gray-900 mb-1">{emptyMessage}</h3>
                            <p className="text-gray-500 max-w-xs mx-auto">
                                Não encontramos resultados para os filtros aplicados. Tente ajustar sua busca.
                            </p>
                        </div>
                    )}
                </div>

                {/* 3. PAGINAÇÃO */}
                {!isLoading && totalPages > 1 && (
                    <div className="pt-8 border-t border-gray-100">
                        <Pagination
                            currentPage={currentPage}
                            totalPages={totalPages}
                            onPageChange={onPageChange}
                        />
                    </div>
                )}
            </div>
        </div>
    );
};
