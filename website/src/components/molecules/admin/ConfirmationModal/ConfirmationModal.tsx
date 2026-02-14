"use client";

import { Modal } from "@/components/molecules/Modal";
import { Button } from "@/components/atoms/Button";
import { ExclamationTriangleIcon } from "@heroicons/react/24/outline";

interface ConfirmationModalProps {
    isOpen: boolean;
    onClose: () => void;
    onConfirm: () => void;
    title: string;
    message: string;
    confirmLabel?: string;
    cancelLabel?: string;
    isLoading?: boolean;
    variant?: "danger" | "warning" | "info";
}

export const ConfirmationModal = ({
    isOpen,
    onClose,
    onConfirm,
    title,
    message,
    confirmLabel = "Confirmar",
    cancelLabel = "Cancelar",
    isLoading = false,
    variant = "danger",
}: ConfirmationModalProps) => {
    const variantClasses = {
        danger: "bg-red-100 text-red-600",
        warning: "bg-yellow-100 text-yellow-600",
        info: "bg-blue-100 text-blue-600",
    };

    const buttonVariants = {
        danger: "bg-red-600 hover:bg-red-700 text-white",
        warning: "bg-yellow-600 hover:bg-yellow-700 text-white",
        info: "bg-blue-600 hover:bg-blue-700 text-white",
    };

    return (
        <Modal isOpen={isOpen} onClose={onClose} className="max-w-md">
            <div className="flex flex-col items-center text-center p-2">
                <div className={`p-3 rounded-full mb-4 ${variantClasses[variant]}`}>
                    <ExclamationTriangleIcon className="w-8 h-8" />
                </div>

                <h3 className="text-xl font-bold text-gray-900 mb-2">{title}</h3>
                <p className="text-gray-500 mb-8">{message}</p>

                <div className="flex gap-3 w-full">
                    <Button
                        className="flex-1 bg-gray-100 hover:bg-gray-200 text-gray-700 border-none"
                        onClick={onClose}
                        disabled={isLoading}
                    >
                        {cancelLabel}
                    </Button>
                    <Button
                        className={`flex-1 border-none ${buttonVariants[variant]}`}
                        onClick={onConfirm}
                        loading={isLoading}
                    >
                        {confirmLabel}
                    </Button>
                </div>
            </div>
        </Modal>
    );
};
