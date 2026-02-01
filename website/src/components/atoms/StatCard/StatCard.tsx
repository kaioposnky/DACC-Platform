import React from 'react';
import { motion } from 'framer-motion';

interface StatCardProps {
    icon: React.ComponentType<{ className?: string }>;
    label: string;
    value: string | number;
    trend?: string;
    colorClass?: string;
}

export const StatCard = ({ icon: Icon, label, value, trend, colorClass = 'bg-blue-500' }: StatCardProps) => {
    return (
        <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            className="bg-white rounded-xl p-6 shadow-sm border border-gray-100 hover:shadow-md transition-shadow"
        >
            <div className="flex items-center justify-between">
                <div className="flex-1">
                    <p className="text-sm font-medium text-gray-500 mb-1">{label}</p>
                    <p className="text-3xl font-bold text-gray-900">{value}</p>
                    {trend && (
                        <p className="text-xs text-gray-500 mt-2">{trend}</p>
                    )}
                </div>
                <div className={`p-3 rounded-lg ${colorClass} bg-opacity-10`}>
                    <Icon className={`w-8 h-8 ${colorClass.replace('bg-', 'text-')}`} />
                </div>
            </div>
        </motion.div>
    );
};
