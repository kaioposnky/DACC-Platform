import React from "react";

interface AdminCardProps {
    title: string;
    children: React.ReactNode;
    className?: string;
    actions?: React.ReactNode;
}

export const AdminCard = ({ title, children, className = "", actions }: AdminCardProps) => {
    return (
        <div className={`bg-white rounded-xl border border-gray-200 shadow-sm overflow-hidden ${className}`}>
            <div className="px-6 py-4 border-b border-gray-100 flex justify-between items-center bg-gray-50/50">
                <h3 className="text-sm font-bold text-gray-900 uppercase tracking-wide">
                    {title}
                </h3>
                {actions && <div>{actions}</div>}
            </div>
            <div className="p-6">
                {children}
            </div>
        </div>
    );
};
