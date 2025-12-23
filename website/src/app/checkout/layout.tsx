'use client';

import React, {useEffect} from "react";
import {useAuth} from "@/context/AuthContext";
import {useRouter} from "next/navigation";
import { LoadingSpinner } from "@/components/atoms";

export default function CheckoutLayout({children}: Readonly<{ children: React.ReactNode; }>) {
    const router = useRouter();
    const { isLoading, isAuthenticated} = useAuth();

    useEffect(() => {
        if (!isLoading && !isAuthenticated) {
            router.push('/login');
        }
    }, [isLoading, isAuthenticated, router]);

    if (isLoading) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-gray-50">
                <LoadingSpinner size="lg" />
            </div>
        );
    }

    if (!isAuthenticated) {
        return null;
    }

    return (
        <>
            {children}
        </>
    );
}