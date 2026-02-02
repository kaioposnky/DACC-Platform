import { motion } from "framer-motion";
import { LoadingSpinner } from "@/components/atoms/LoadingSpinner/LoadingSpinner";

interface PageLoaderProps {
    message?: string;
}

export const PageLoader = ({ message = "Carregando..." }: PageLoaderProps) => {
    return (
        <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            className="fixed inset-0 z-50 flex flex-col items-center justify-center bg-white/80 backdrop-blur-sm"
        >
            <LoadingSpinner size="lg" color="text-blue-600" />
            {message && (
                <motion.p
                    initial={{ opacity: 0, y: 10 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ delay: 0.2 }}
                    className="mt-4 text-gray-600 font-medium"
                >
                    {message}
                </motion.p>
            )}
        </motion.div>
    );
};
