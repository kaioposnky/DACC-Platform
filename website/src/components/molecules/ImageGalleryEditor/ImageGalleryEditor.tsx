import { TrashIcon } from "@heroicons/react/24/outline";
import { useEffect, useState } from "react";
import { Input } from "../../atoms/Input";

interface ImageGalleryEditorProps {
    title?: string;
    description?: string;
    images: string[];
    onAddImage: (imageUrl: string) => void;
    onRemoveImage: (index: number) => void;
}

export const ImageGalleryEditor = ({
    title,
    description,
    images,
    onAddImage,
    onRemoveImage
}: ImageGalleryEditorProps) => {
    const [tempImageUrl, setTempImageUrl] = useState("");

    // Efeito para capturar colar imagem (Paste)
    useEffect(() => {
        const handlePaste = async (e: ClipboardEvent) => {
            // Só ativa se o foco estiver na janela/documento geral, ou no input dentro deste componente
            // Como é um componente reutilizável, o ideal é que ele capture eventos globais APENAS se estiver visível/montado.
            // O componente pai deve controlar se este componente está sendo exibido (ex: dentro de um modal).

            const items = e.clipboardData?.items;
            if (!items) return;

            for (let i = 0; i < items.length; i++) {
                if (items[i].type.indexOf('image') !== -1) {
                    e.preventDefault();
                    const blob = items[i].getAsFile();
                    if (blob) {
                        const reader = new FileReader();
                        reader.onload = (event) => {
                            if (event.target?.result) {
                                setTempImageUrl(event.target.result as string);
                            }
                        };
                        reader.readAsDataURL(blob);
                    }
                    break;
                }
            }
        };

        window.addEventListener('paste', handlePaste);
        return () => window.removeEventListener('paste', handlePaste);
    }, []);

    const handleConfirmAdd = () => {
        if (tempImageUrl.trim()) {
            onAddImage(tempImageUrl);
            setTempImageUrl("");
        }
    };

    return (
        <div className="space-y-6">
            {(title || description) && (
                <div className="border-b pb-4">
                    {title && <h3 className="text-lg font-bold text-gray-900">{title}</h3>}
                    {description && <p className="text-sm text-gray-500">{description}</p>}
                </div>
            )}

            {/* Grid de Imagens */}
            <div className="grid grid-cols-3 sm:grid-cols-4 gap-4">
                {images.map((img, idx) => (
                    <div key={idx} className="group relative aspect-square bg-gray-100 rounded-lg overflow-hidden border border-gray-200">
                        <img src={img} alt={`Foto ${idx}`} className="w-full h-full object-cover" />
                        <div className="absolute inset-0 bg-black/50 opacity-0 group-hover:opacity-100 transition-opacity flex items-center justify-center gap-2">
                            <button
                                onClick={() => onRemoveImage(idx)}
                                className="p-1.5 bg-red-500 text-white rounded-full hover:bg-red-600 transition-colors"
                                title="Remover imagem"
                            >
                                <TrashIcon className="w-4 h-4" />
                            </button>
                        </div>
                    </div>
                ))}

                {/* Dropzone / Upload Button */}
                <label className="aspect-square border-2 border-dashed border-gray-300 rounded-lg flex flex-col items-center justify-center text-gray-400 bg-gray-50 hover:bg-gray-100 hover:border-blue-400 hover:text-blue-500 cursor-pointer transition-all group">
                    <span className="text-2xl mb-1 font-light group-hover:scale-110 transition-transform">+</span>
                    <span className="text-[10px] text-center px-1 leading-tight">
                        Selecione ou<br /><strong className="text-inherit">Cole (Ctrl+V)</strong>
                    </span>
                    <input
                        type="file"
                        accept="image/*"
                        className="hidden"
                        onChange={(e) => {
                            const file = e.target.files?.[0];
                            if (file) {
                                const reader = new FileReader();
                                reader.onload = (ev) => {
                                    if (ev.target?.result) setTempImageUrl(ev.target.result as string);
                                };
                                reader.readAsDataURL(file);
                            }
                        }}
                    />
                </label>
            </div>

            {/* Preview da Imagem em Staging */}
            {tempImageUrl && (
                <div className="p-4 border rounded-lg bg-blue-50 flex items-center justify-between animate-in fade-in slide-in-from-top-2">
                    <div className="flex items-center gap-4">
                        <div className="w-12 h-12 bg-white rounded border overflow-hidden flex-shrink-0">
                            <img src={tempImageUrl} className="w-full h-full object-cover" alt="Preview a adicionar" />
                        </div>
                        <div>
                            <p className="text-sm font-semibold text-blue-900">Imagem pronta para adicionar</p>
                            <p className="text-xs text-blue-700">Clique em "Adicionar" para confirmar.</p>
                        </div>
                    </div>
                    <button
                        onClick={() => setTempImageUrl("")}
                        className="text-xs text-red-600 hover:text-red-800 font-medium px-2 py-1 hover:bg-red-50 rounded"
                    >
                        Cancelar
                    </button>
                </div>
            )}

            {/* Input de URL e Ação Principal */}
            <div className="flex gap-2 pt-4 border-t items-end">
                <div className="flex-1">
                    <Input
                        label="Ou adicione URL da Imagem"
                        placeholder="https://..."
                        value={tempImageUrl}
                        onChange={(e) => setTempImageUrl(e.target.value)}
                    />
                </div>
                <button
                    onClick={handleConfirmAdd}
                    disabled={!tempImageUrl}
                    className="px-6 py-2.5 bg-blue-600 text-white rounded-md font-semibold text-sm hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed mb-0.5 shadow-sm transition-all active:scale-95"
                >
                    Adicionar
                </button>
            </div>
        </div>
    );
};
