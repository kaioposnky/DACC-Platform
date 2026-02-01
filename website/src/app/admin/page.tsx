"use client";

import { useEffect, useState } from "react";
import { apiService } from "@/services/api";
import { DashboardStats } from "@/types/dashboard";
import { StatCard } from "@/components/atoms/StatCard";
import {
    UsersIcon,
    ShoppingCartIcon,
    CubeIcon,
    StarIcon,
    CalendarIcon,
    NewspaperIcon,
    MegaphoneIcon,
    AcademicCapIcon
} from "@heroicons/react/24/solid";
import { motion } from "framer-motion";

export default function AdminPage() {
    const [stats, setStats] = useState<DashboardStats | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchStats = async () => {
            try {
                setLoading(true);
                const data = await apiService.getDashboardStats();
                setStats(data);
            } catch (err: any) {
                setError(err.message || "Erro ao carregar estatísticas");
            } finally {
                setLoading(false);
            }
        };

        fetchStats();
    }, []);

    if (loading) {
        return (
            <div className="min-h-screen flex items-center justify-center">
                <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
            </div>
        );
    }

    if (error || !stats) {
        return (
            <div className="min-h-screen flex items-center justify-center">
                <div className="text-center">
                    <p className="text-red-600 font-semibold mb-2">Erro ao carregar dashboard</p>
                    <p className="text-gray-500 text-sm">{error}</p>
                </div>
            </div>
        );
    }

    const formatCurrency = (value: number) => {
        return new Intl.NumberFormat('pt-BR', {
            style: 'currency',
            currency: 'BRL',
        }).format(value);
    };

    return (
        <div className="p-6 max-w-7xl mx-auto space-y-8">
            {/* Header */}
            <div>
                <h1 className="text-3xl font-bold text-gray-900">Dashboard</h1>
                <p className="text-gray-500 mt-1">Visão geral do sistema</p>
            </div>

            {/* KPI Cards */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                <StatCard
                    icon={UsersIcon}
                    label="Total de Usuários"
                    value={stats.users.total.toLocaleString('pt-BR')}
                    trend={`+${stats.users.newThisMonth} este mês`}
                    colorClass="bg-blue-500"
                />
                <StatCard
                    icon={ShoppingCartIcon}
                    label="Pedidos"
                    value={stats.orders.total.toLocaleString('pt-BR')}
                    trend={formatCurrency(stats.orders.totalRevenue)}
                    colorClass="bg-green-500"
                />
                <StatCard
                    icon={CubeIcon}
                    label="Produtos Ativos"
                    value={stats.products.totalActive.toLocaleString('pt-BR')}
                    trend={`${stats.products.lowStockCount} em baixo estoque`}
                    colorClass="bg-purple-500"
                />
                <StatCard
                    icon={StarIcon}
                    label="Avaliação Média"
                    value={`${stats.reviews.averageRating} ⭐`}
                    trend={`${stats.reviews.total} avaliações`}
                    colorClass="bg-yellow-500"
                />
            </div>

            {/* Secondary Stats */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                <StatCard
                    icon={CalendarIcon}
                    label="Eventos"
                    value={stats.events.total.toLocaleString('pt-BR')}
                    trend={`${stats.events.upcoming} próximos`}
                    colorClass="bg-orange-500"
                />
                <StatCard
                    icon={NewspaperIcon}
                    label="Notícias"
                    value={stats.news.total.toLocaleString('pt-BR')}
                    colorClass="bg-cyan-500"
                />
                <StatCard
                    icon={MegaphoneIcon}
                    label="Anúncios Ativos"
                    value={stats.ads.totalActive.toLocaleString('pt-BR')}
                    colorClass="bg-pink-500"
                />
                <StatCard
                    icon={AcademicCapIcon}
                    label="Corpo Docente"
                    value={stats.faculty.total.toLocaleString('pt-BR')}
                    colorClass="bg-indigo-500"
                />
            </div>

            {/* Detailed Stats Grid */}
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
                {/* Users by Role */}
                <motion.div
                    initial={{ opacity: 0, y: 20 }}
                    animate={{ opacity: 1, y: 0 }}
                    className="bg-white rounded-xl p-6 shadow-sm border border-gray-100"
                >
                    <h3 className="text-lg font-semibold text-gray-900 mb-4">Usuários por Cargo</h3>
                    <div className="space-y-3">
                        {Object.entries(stats.users.byRole).map(([role, count]) => (
                            <div key={role} className="flex items-center justify-between">
                                <span className="text-sm text-gray-600 capitalize">{role}</span>
                                <span className="text-sm font-semibold text-gray-900">{count}</span>
                            </div>
                        ))}
                    </div>
                </motion.div>

                {/* Orders by Status */}
                <motion.div
                    initial={{ opacity: 0, y: 20 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ delay: 0.1 }}
                    className="bg-white rounded-xl p-6 shadow-sm border border-gray-100"
                >
                    <h3 className="text-lg font-semibold text-gray-900 mb-4">Pedidos por Status</h3>
                    <div className="space-y-3">
                        {Object.entries(stats.orders.byStatus).map(([status, count]) => (
                            <div key={status} className="flex items-center justify-between">
                                <span className="text-sm text-gray-600 capitalize">{status}</span>
                                <span className="text-sm font-semibold text-gray-900">{count}</span>
                            </div>
                        ))}
                    </div>
                </motion.div>

                {/* Products by Category */}
                <motion.div
                    initial={{ opacity: 0, y: 20 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ delay: 0.2 }}
                    className="bg-white rounded-xl p-6 shadow-sm border border-gray-100"
                >
                    <h3 className="text-lg font-semibold text-gray-900 mb-4">Produtos por Categoria</h3>
                    <div className="space-y-3">
                        {Object.entries(stats.products.byCategory).map(([category, count]) => (
                            <div key={category} className="flex items-center justify-between">
                                <span className="text-sm text-gray-600">{category}</span>
                                <span className="text-sm font-semibold text-gray-900">{count}</span>
                            </div>
                        ))}
                    </div>
                </motion.div>

                {/* Reviews Distribution */}
                <motion.div
                    initial={{ opacity: 0, y: 20 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ delay: 0.3 }}
                    className="bg-white rounded-xl p-6 shadow-sm border border-gray-100"
                >
                    <h3 className="text-lg font-semibold text-gray-900 mb-4">Distribuição de Avaliações</h3>
                    <div className="space-y-3">
                        {Object.entries(stats.reviews.ratingDistribution)
                            .sort((a, b) => Number(b[0]) - Number(a[0]))
                            .map(([rating, count]) => (
                                <div key={rating} className="flex items-center justify-between">
                                    <div className="flex items-center gap-1">
                                        <span className="text-sm text-gray-600">{rating}</span>
                                        <StarIcon className="w-4 h-4 text-yellow-400" />
                                    </div>
                                    <span className="text-sm font-semibold text-gray-900">{count}</span>
                                </div>
                            ))}
                    </div>
                </motion.div>
            </div>
        </div>
    );
}
