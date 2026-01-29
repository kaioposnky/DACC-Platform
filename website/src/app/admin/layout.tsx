import { NavigationAdmin } from "@/components/organisms/admin/NavigationAdmin";
import React from "react";

export default function AdminLayout({
    children,
}: {
    children: React.ReactNode;
}) {
    return (
        <div className="min-h-screen bg-gray-50">
            <NavigationAdmin />

            <main className="ml-64">
                {children}
            </main>
        </div>
    );
}
