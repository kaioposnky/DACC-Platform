'use client';

import { useState } from 'react';
import { motion } from 'framer-motion';
import { Typography, Button } from '@/components/atoms';
import { Input } from '@/components/atoms/Input';
import { toast } from 'sonner';

export interface UserSecurityFormData {
    currentPassword: '';
    newPassword: '';
    confirmPassword: '';
}

interface ProfileSecurityFormProps {
    onSave?: (currentPassword: string, newPassword: string) => Promise<void>;
    className?: string;
}

export const ProfileSecurityForm = ({
    onSave,
    className = ''
}: ProfileSecurityFormProps) => {

    const [formData, setFormData] = useState({
        currentPassword: '',
        newPassword: '',
        confirmPassword: ''
    });

    const [errors, setErrors] = useState<Partial<Record<keyof typeof formData, string>>>({});
    const [isSubmitting, setIsSubmitting] = useState(false);

    const isDirty = formData.currentPassword !== '' || formData.newPassword !== '' || formData.confirmPassword !== '';

    const handleInputChange = (field: keyof typeof formData, value: string) => {
        setFormData(prev => ({ ...prev, [field]: value }));

        // Clear error when user starts typing
        if (errors[field]) {
            setErrors(prev => ({ ...prev, [field]: '' }));
        }
    };

    const validateForm = (): boolean => {
        const newErrors: Partial<Record<keyof typeof formData, string>> = {};

        if (!formData.currentPassword) {
            newErrors.currentPassword = 'A senha atual é obrigatória';
        }

        if (!formData.newPassword) {
            newErrors.newPassword = 'A nova senha é obrigatória';
        } else if (formData.newPassword.length < 6) {
            newErrors.newPassword = 'A nova senha deve ter no mínimo 6 caracteres';
        }

        if (!formData.confirmPassword) {
            newErrors.confirmPassword = 'A confirmação de senha é obrigatória';
        } else if (formData.newPassword !== formData.confirmPassword) {
            newErrors.confirmPassword = 'As senhas não coincidem';
        }

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSave = async () => {
        if (validateForm()) {
            setIsSubmitting(true);
            try {
                if (onSave) {
                    await onSave(formData.currentPassword, formData.newPassword);
                    toast.success("Senha atualizada com sucesso!");
                    handleReset();
                }
            } catch (error: any) {
                toast.error(error.message || "Erro ao atualizar a senha");
            } finally {
                setIsSubmitting(false);
            }
        }
    };

    const handleReset = () => {
        setFormData({
            currentPassword: '',
            newPassword: '',
            confirmPassword: ''
        });
        setErrors({});
    };

    return (
        <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5 }}
            className={`bg-white rounded-lg shadow-sm border border-gray-200 p-8 ${className}`}
        >
            {/* Header */}
            <div className="mb-8">
                <Typography variant="h2" className="text-gray-900 font-bold mb-2">
                    Segurança
                </Typography>
                <Typography variant="body" className="text-gray-600">
                    Atualize sua senha para manter sua conta segura.
                </Typography>
            </div>

            <div className="mb-8">
                <Typography variant="h4" className="text-gray-900 font-semibold mb-6">
                    Alterar Senha
                </Typography>

                <div className="space-y-6 max-w-md">
                    <Input
                        label="Senha Atual"
                        type="password"
                        value={formData.currentPassword}
                        onChange={(e) => handleInputChange('currentPassword', e.target.value)}
                        error={errors.currentPassword}
                        placeholder="Digite sua senha atual"
                        required
                    />

                    <Input
                        label="Nova Senha"
                        type="password"
                        value={formData.newPassword}
                        onChange={(e) => handleInputChange('newPassword', e.target.value)}
                        error={errors.newPassword}
                        placeholder="Digite sua nova senha"
                        required
                    />

                    <Input
                        label="Confirmar Nova Senha"
                        type="password"
                        value={formData.confirmPassword}
                        onChange={(e) => handleInputChange('confirmPassword', e.target.value)}
                        error={errors.confirmPassword}
                        placeholder="Confirme sua nova senha"
                        required
                    />
                </div>
            </div>

            {/* Action Buttons */}
            <div className="flex flex-col sm:flex-row gap-4 justify-end pt-6 border-t border-gray-200">
                <Button
                    variant="secondary"
                    onClick={handleReset}
                    disabled={!isDirty || isSubmitting}
                    className="w-full sm:w-auto disabled:opacity-50"
                >
                    Cancelar
                </Button>
                <Button
                    variant="primary"
                    onClick={handleSave}
                    disabled={!isDirty || isSubmitting}
                    className="w-full sm:w-auto disabled:opacity-50"
                >
                    {isSubmitting ? 'Atualizando...' : 'Atualizar Senha'}
                </Button>
            </div>
        </motion.div>
    );
};
