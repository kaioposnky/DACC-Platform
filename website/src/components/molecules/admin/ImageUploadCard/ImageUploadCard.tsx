"use client";

import { useState } from "react";
import { AdminCard, ImageGalleryEditor, Modal } from "@/components";
import { PhotoIcon } from "@heroicons/react/24/outline";

interface ImageUploadCardProps {
    title?: string;
    description?: string;
    icon?: React.ReactNode;
    image?: string;
    onSetImage: (url: string) => void;
    onRemoveImage: () => void;
    multiple?: boolean;
    images?: string[];
    onAddImage?: (url: string) => void;
    galleryTitle?: string;
    galleryDescription?: string;
    previewClassName?: string;
    showModal?: boolean;
}

export default function ImageUploadCard({
    title = "Imagem",
    description = "Clique para alterar a imagem.",
    icon,
    image,
    onSetImage,
    onRemoveImage,
    multiple = false,
    images = [],
    onAddImage,
    galleryTitle = "Gerenciar Imagem",
    galleryDescription = "Esta imagem será usada como visual principal.",
    previewClassName = "aspect-square w-32 h-32",
    showModal = true,
}: ImageUploadCardProps) {
    const [isGalleryOpen, setIsGalleryOpen] = useState(false);

    const handleOpenGallery = () => setIsGalleryOpen(true);
    const handleCloseGallery = () => setIsGalleryOpen(false);

    const currentImages = multiple ? images : image ? [image] : [];

    const handleImageAction = (url: string) => {
        if (multiple && onAddImage) {
            onAddImage(url);
        } else {
            onSetImage(url);
        }
        if (showModal) {
            handleCloseGallery();
        }
    };

    const content = (
        <div className="space-y-4 text-center">
            <div
                onClick={showModal ? handleOpenGallery : undefined}
                className={`relative ${previewClassName} mx-auto bg-gray-50 rounded-2xl border-2 border-dashed border-gray-200 flex flex-col items-center justify-center ${showModal ? 'cursor-pointer hover:bg-gray-100 hover:border-primary/30' : ''} transition-all group overflow-hidden`}
            >
                {image ? (
                    <img
                        src={image}
                        alt="Preview"
                        className="w-full h-full object-cover"
                    />
                ) : (
                    <>
                        {icon || <PhotoIcon className="w-10 h-10 text-gray-300 group-hover:text-primary/40" />}
                    </>
                )}
                {showModal && (
                    <div className="absolute inset-0 bg-black/40 opacity-0 group-hover:opacity-100 flex items-center justify-center transition-opacity">
                        <span className="text-white text-xs font-bold">
                            Alterar Imagem
                        </span>
                    </div>
                )}
            </div>
            <p className="text-[10px] text-gray-400">{description}</p>

            {!showModal && (
                <ImageGalleryEditor
                    images={currentImages}
                    multiple={multiple}
                    onAddImage={handleImageAction}
                    onRemoveImage={onRemoveImage}
                />
            )}
        </div>
    );

    return (
        <>
            <AdminCard
                icon={icon || <PhotoIcon className="w-5 h-5 text-primary" />}
                title={title}
            >
                {content}
            </AdminCard>

            {showModal && (
                <Modal
                    isOpen={isGalleryOpen}
                    onClose={handleCloseGallery}
                    className="max-w-3xl"
                >
                    <ImageGalleryEditor
                        title={galleryTitle}
                        description={galleryDescription}
                        images={currentImages}
                        multiple={multiple}
                        onAddImage={handleImageAction}
                        onRemoveImage={onRemoveImage}
                    />
                </Modal>
            )}
        </>
    );
}
