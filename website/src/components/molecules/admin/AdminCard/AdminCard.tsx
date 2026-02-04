import React from "react";

interface AdminCardProps {
    title: string;
    children: React.ReactNode;
    className?: string;
    actions?: React.ReactNode;
    icon?: React.ReactNode;
}

export const AdminCard = ({ title, children, className = "", actions, icon }: AdminCardProps) => {
    return (
        <div className={`bg-white rounded-xl border border-gray-200 shadow-sm overflow-hidden ${className}`}>
            <div className="px-6 py-4 border-b border-gray-100 flex justify-between items-center bg-gray-50/50">
                <div className="flex items-center gap-2">
                    {icon && <div className="text-primary">{icon}</div>}
                    <h3 className="text-sm font-bold text-gray-900 uppercase tracking-wide">
                        {title}
                    </h3>
                </div>
                {actions && <div>{actions}</div>}
            </div>
            <div className="p-6">
                {children}
            </div>
        </div>
    );
};
