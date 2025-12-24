'use client';

import React, { useState, useRef } from "react";
import { Modal } from "@/components/molecules/Modal";
import { Typography, Button } from "@/components/atoms";
import { PhotoIcon, ArrowUpTrayIcon } from "@heroicons/react/24/outline";
import { motion, AnimatePresence } from "framer-motion";
import Image from "next/image";

export interface ImageInputModalProps {
    title?: string;
    isOpen: boolean;
    onClose: () => void;
    onImageUpload: (image: File) => void;
}

export const ImageInputModal: React.FC<ImageInputModalProps> = ({ 
    title = "Atualizar Foto de Perfil", 
    isOpen, 
    onClose, 
    onImageUpload 
}) => {
    const [imageFile, setImageFile] = useState<File | null>(null);
    const [previewUrl, setPreviewUrl] = useState<string | null>(null);
    const [isDragging, setIsDragging] = useState(false);
    const [isUploading, setIsUploading] = useState(false);
    const fileInputRef = useRef<HTMLInputElement>(null);

    const handleFile = (file: File) => {
        if (file && file.type.startsWith('image/')) {
            setImageFile(file);
            const reader = new FileReader();
            reader.onloadend = () => {
                setPreviewUrl(reader.result as string);
            };
            reader.readAsDataURL(file);
        }
    };

    const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (file) handleFile(file);
    };

    const handleDragOver = (e: React.DragEvent) => {
        e.preventDefault();
        setIsDragging(true);
    };

    const handleDragLeave = () => {
        setIsDragging(false);
    };

    const handleDrop = (e: React.DragEvent) => {
        e.preventDefault();
        setIsDragging(false);
        const file = e.dataTransfer.files?.[0];
        if (file) handleFile(file);
    };

    const handleUpload = async () => {
        if (!imageFile) return;
        
        setIsUploading(true);
        try {
            await onImageUpload(imageFile);
            handleClose();
        } catch (error) {
            console.error("Erro ao fazer upload:", error);
        } finally {
            setIsUploading(false);
        }
    };

    const handleClose = () => {
        setImageFile(null);
        setPreviewUrl(null);
        onClose();
    };

    const triggerFileInput = () => {
        fileInputRef.current?.click();
    };

    return (
        <Modal isOpen={isOpen} onClose={handleClose}>
            <div className="p-6 max-w-md w-full">
                <div className="flex justify-between items-center mb-6">
                    <Typography variant="h3" className="font-bold text-gray-900">
                        {title}
                    </Typography>
                </div>

                <div
                    onDragOver={handleDragOver}
                    onDragLeave={handleDragLeave}
                    onDrop={handleDrop}
                    className={`
                        relative border-2 border-dashed rounded-xl p-8
                        flex flex-col items-center justify-center transition-all duration-200
                        ${isDragging ? 'border-blue-500 bg-blue-50' : 'border-gray-300 hover:border-gray-400 bg-gray-50'}
                        ${previewUrl ? 'py-4' : 'py-12'}
                    `}
                >
                    <AnimatePresence mode="wait">
                        {previewUrl ? (
                            <motion.div
                                initial={{ opacity: 0, scale: 0.9 }}
                                animate={{ opacity: 1, scale: 1 }}
                                exit={{ opacity: 0, scale: 0.9 }}
                                className="relative w-48 h-48"
                            >
                                <Image
                                    src={previewUrl}
                                    alt="Preview"
                                    fill
                                    className="object-cover rounded-full border-4 border-white shadow-md"
                                />
                            </motion.div>
                        ) : (
                            <motion.div
                                initial={{ opacity: 0 }}
                                animate={{ opacity: 1 }}
                                exit={{ opacity: 0 }}
                                className="flex flex-col items-center text-center"
                            >
                                <div className="bg-white p-4 rounded-full shadow-sm mb-4">
                                    <PhotoIcon className="w-10 h-10 text-gray-400" />
                                </div>
                                
                                <Typography variant="body" className="text-gray-600">
                                    Arraste uma imagem ou
                                </Typography>
                                
                                <button
                                    onClick={triggerFileInput}
                                    className="text-blue-600 font-semibold hover:text-blue-700 transition-colors mt-1"
                                >
                                    escolha um arquivo
                                </button>
                                
                                <Typography variant="subtitle" className="text-gray-400 mt-4 block">
                                    PNG, JPG ou GIF (Máx. 5MB)
                                </Typography>
                            </motion.div>
                        )}
                    </AnimatePresence>

                    <input
                        ref={fileInputRef}
                        type="file"
                        accept="image/*"
                        onChange={handleImageChange}
                        className="hidden"
                    />
                </div>

                <div className="mt-8 flex gap-3">
                    <Button
                        variant="secondary"
                        className="flex-1"
                        onClick={handleClose}
                        disabled={isUploading}
                    >
                        Cancelar
                    </Button>
                    <Button
                        variant="primary"
                        className="flex-1"
                        onClick={handleUpload}
                        disabled={!imageFile || isUploading}
                    >
                        {isUploading ? (
                            <div className="flex items-center gap-2">
                                <div className="w-4 h-4 border-2 border-white border-t-transparent rounded-full animate-spin" />
                                Enviando...
                            </div>
                        ) : (
                            <div className="flex items-center gap-2">
                                <ArrowUpTrayIcon className="w-4 h-4" />
                                Salvar Foto
                            </div>
                        )}
                    </Button>
                </div>
            </div>
        </Modal>
    );
};
