import React from 'react';
import { Button } from '@/components';
import { ArrowLeftIcon } from '@heroicons/react/24/solid';

interface EditPageHeaderProps {
    title: string;
    id?: string;
    label?: string; // Ex: "Editando", "Criando"
    status?: {
        text: string;
        colorClass?: string;
    };
    onSave: () => void;
    onBack: () => void;
    saveButtonText?: string;
}

export const EditPageHeader = ({
    title,
    id,
    label = "Editando",
    status,
    onSave,
    onBack,
    saveButtonText = "Salvar Alterações"
}: EditPageHeaderProps) => {
    return (
        <section className="sticky top-0 z-30 bg-primary/80 backdrop-blur-md border-b border-white/5 p-4 sm:p-6">
            <div className="max-w-7xl mx-auto flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
                <div className="flex items-center gap-4">
                    <button
                        onClick={onBack}
                        className="p-2 hover:bg-white/10 rounded-lg text-white/70 hover:text-white transition-colors"
                        title="Voltar"
                    >
                        <ArrowLeftIcon className="w-6 h-6" />
                    </button>

                    <div>
                        <div className="flex flex-wrap items-center gap-2 sm:gap-3">
                            <h2 className="text-xl sm:text-2xl text-white font-bold leading-tight">
                                {label} <span className="text-secondary-500">{title}</span>
                            </h2>
                            {status && (
                                <span className={`px-2 py-0.5 rounded-full text-[10px] font-bold uppercase border ${status.colorClass || 'bg-blue-500/10 text-blue-400 border-blue-500/20'
                                    }`}>
                                    {status.text}
                                </span>
                            )}
                        </div>
                        {id && <p className="text-[10px] text-zinc-500 font-mono mt-1">ID: {id}</p>}
                    </div>
                </div>

                <div className="w-full sm:w-auto flex justify-end">
                    <Button
                        variant="hero-outline"
                        onClick={onSave}
                        className="w-full sm:w-auto font-semibold py-2 px-6 rounded-lg transition-all active:scale-95"
                    >
                        {saveButtonText}
                    </Button>
                </div>
            </div>
        </section>
    );
};
