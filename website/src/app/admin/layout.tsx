"use client"

import { NavigationAdmin } from "@/components/organisms/admin/NavigationAdmin";
import React, {useEffect} from "react";
import {useAuth} from "@/context/AuthContext";
import {useRouter} from "next/navigation";
import {toast} from "sonner";

export default function AdminLayout({
    children,
}: {
    children: React.ReactNode;
}) {
    const router = useRouter();
    const { user, isLoading } = useAuth();

    useEffect(() => {
        if (isLoading) return;

        if (!user){
            router.push("/login");
            return;
        }

        if (!["administrador", "diretor"].includes(user.role)){
            toast.error("Sem permissão para acessar a área admin!");
            router.push("/")
        }
    }, [isLoading]);

    if (isLoading || !user || !["administrador", "diretor"].includes(user.role)){
        return (
            // Spinner de carregamento enquanto usuário não carregou ou o cargo não foi informado
            <div className="min-h-screen bg-gray-50 flex items-center justify-center">
                <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
            </div>
        )
    }

    return (
        <div className="min-h-screen bg-gray-50">
            <NavigationAdmin />

            <main className="ml-64">
                {children}
            </main>
        </div>
    );
}
