"use client";

import Link from "next/link";
import { motion } from "framer-motion";
import { Button } from "@/components";

export default function NotFound() {
    return (
        <div className="min-h-[80vh] flex flex-col items-center justify-center p-4 text-center">
            <motion.div
                initial={{ opacity: 0, scale: 0.8 }}
                animate={{ opacity: 1, scale: 1 }}
                transition={{ duration: 0.5 }}
                className="space-y-6 max-w-lg"
            >
                {/* 404 Illustration/Text */}
                <h1 className="text-9xl font-black text-transparent bg-clip-text bg-gradient-to-r from-blue-600 to-cyan-500">
                    404
                </h1>

                <div className="space-y-4">
                    <h2 className="text-3xl font-bold text-gray-900">
                        Página não encontrada
                    </h2>
                    <p className="text-gray-600 text-lg">
                        Ops! Parece que a página que você está procurando não existe ou foi movida para outro lugar.
                    </p>
                </div>

                <div className="pt-8 flex flex-col sm:flex-row gap-4 justify-center items-center">
                    <Link href="/">
                        <Button variant="primary" size="lg" className="min-w-[200px]">
                            Voltar ao Início
                        </Button>
                    </Link>
                </div>
            </motion.div>
        </div>
    );
}
