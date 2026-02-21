'use client';

import { Suspense, useEffect, useState } from 'react';
import { useSearchParams, useRouter } from 'next/navigation';
import { motion, AnimatePresence } from 'framer-motion';
import { Input, Button, Typography } from '@/components';
import { apiService } from '@/services/api';
import { toast } from 'sonner';
import { KeyIcon, CheckCircleIcon, ExclamationTriangleIcon, ArrowPathIcon } from '@heroicons/react/24/outline';

function ResetPasswordContent() {
    const searchParams = useSearchParams();
    const router = useRouter();
    const token = searchParams.get('token');

    const [isValidating, setIsValidating] = useState(true);
    const [isValid, setIsValid] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [isSuccess, setIsSuccess] = useState(false);

    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [errors, setErrors] = useState<{ password?: string; confirm?: string }>({});

    useEffect(() => {
        if (!token) {
            setIsValidating(false);
            setIsValid(false);
            return;
        }

        const validateToken = async () => {
            try {
                await apiService.validateResetToken(token);
                setIsValid(true);
            } catch (error) {
                console.error('Invalid or expired token', error);
                setIsValid(false);
            } finally {
                setIsValidating(false);
            }
        };

        validateToken();
    }, [token]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        // Validation
        const newErrors: { password?: string; confirm?: string } = {};

        // Backend requirement: 8 chars, upper, lower, digit
        if (password.length < 8) {
            newErrors.password = 'A senha deve ter pelo menos 8 caracteres';
        } else if (!/[A-Z]/.test(password)) {
            newErrors.password = 'A senha deve conter pelo menos uma letra maiúscula';
        } else if (!/[a-z]/.test(password)) {
            newErrors.password = 'A senha deve conter pelo menos uma letra minúscula';
        } else if (!/[0-9]/.test(password)) {
            newErrors.password = 'A senha deve conter pelo menos um número';
        }

        if (password !== confirmPassword) {
            newErrors.confirm = 'As senhas não coincidem';
        }

        if (Object.keys(newErrors).length > 0) {
            setErrors(newErrors);
            return;
        }

        setErrors({});
        setIsSubmitting(true);

        try {
            await apiService.resetPassword({
                token: token!,
                newPassword: password
            });
            setIsSuccess(true);
            toast.success('Senha redefinida com sucesso!');

            // Redirect to login after 3 seconds
            setTimeout(() => {
                router.push('/login');
            }, 3000);
        } catch (error: any) {
            toast.error(error.message || 'Falha ao redefinir senha. Tente novamente.');
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div className="min-h-screen bg-gray-50 flex flex-col justify-center py-12 sm:px-6 lg:px-8">
            <div className="sm:mx-auto sm:w-full sm:max-w-md">
                <div className="flex justify-center">
                    <div className="w-12 h-12 bg-primary rounded-xl flex items-center justify-center shadow-lg transform -rotate-6">
                        <KeyIcon className="w-7 h-7 text-white" />
                    </div>
                </div>
                <h2 className="mt-6 text-center text-3xl font-extrabold text-gray-900">
                    Redefinir sua senha
                </h2>
                <p className="mt-2 text-center text-sm text-gray-600">
                    Crie uma nova senha segura para sua conta
                </p>
            </div>

            <div className="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
                <div className="bg-white py-8 px-4 shadow-xl sm:rounded-xl sm:px-10 border border-gray-100">
                    <AnimatePresence mode="wait">
                        {isValidating ? (
                            <motion.div
                                key="loading"
                                initial={{ opacity: 0 }}
                                animate={{ opacity: 1 }}
                                exit={{ opacity: 0 }}
                                className="flex flex-col items-center justify-center py-8"
                            >
                                <ArrowPathIcon className="w-10 h-10 text-primary animate-spin mb-4" />
                                <Typography variant="body" color="gray">
                                    Validando seu token...
                                </Typography>
                            </motion.div>
                        ) : !isValid ? (
                            <motion.div
                                key="invalid"
                                initial={{ opacity: 0, y: 20 }}
                                animate={{ opacity: 1, y: 0 }}
                                className="text-center py-4"
                            >
                                <div className="mx-auto flex items-center justify-center h-12 w-12 rounded-full bg-red-100 mb-4">
                                    <ExclamationTriangleIcon className="h-6 w-6 text-red-600" aria-hidden="true" />
                                </div>
                                <Typography variant="h4" className="text-red-600 mb-2">
                                    Link Inválido ou Expirado
                                </Typography>
                                <Typography variant="body" color="gray" className="mb-6">
                                    Este link de redefinição de senha não é mais válido ou já foi utilizado.
                                </Typography>
                                <Button
                                    onClick={() => router.push('/login')}
                                    className="w-full bg-primary hover:bg-blue-800"
                                >
                                    Voltar para o Login
                                </Button>
                            </motion.div>
                        ) : isSuccess ? (
                            <motion.div
                                key="success"
                                initial={{ opacity: 0, scale: 0.9 }}
                                animate={{ opacity: 1, scale: 1 }}
                                className="text-center py-8"
                            >
                                <div className="mx-auto flex items-center justify-center h-16 w-16 rounded-full bg-green-100 mb-6">
                                    <CheckCircleIcon className="h-10 w-10 text-green-600" aria-hidden="true" />
                                </div>
                                <Typography variant="h4" className="text-green-800 mb-2">
                                    Senha Alterada!
                                </Typography>
                                <Typography variant="body" color="gray" className="mb-4">
                                    Sua senha foi redefinida com sucesso. Você será redirecionado para o login em breve.
                                </Typography>
                                <Button
                                    onClick={() => router.push('/login')}
                                    className="w-full bg-green-600 hover:bg-green-700 border-none"
                                >
                                    Fazer Login Agora
                                </Button>
                            </motion.div>
                        ) : (
                            <motion.form
                                key="form"
                                initial={{ opacity: 0 }}
                                animate={{ opacity: 1 }}
                                onSubmit={handleSubmit}
                                className="space-y-6"
                            >
                                <div>
                                    <Input
                                        label="Nova Senha"
                                        type="password"
                                        placeholder="••••••••"
                                        value={password}
                                        onChange={(e) => setPassword(e.target.value)}
                                        error={errors.password}
                                        disabled={isSubmitting}
                                        required
                                    />
                                </div>

                                <div>
                                    <Input
                                        label="Confirmar Nova Senha"
                                        type="password"
                                        placeholder="••••••••"
                                        value={confirmPassword}
                                        onChange={(e) => setConfirmPassword(e.target.value)}
                                        error={errors.confirm}
                                        disabled={isSubmitting}
                                        required
                                    />
                                </div>

                                <div>
                                    <Button
                                        type="submit"
                                        className="w-full bg-primary hover:bg-blue-800"
                                        loading={isSubmitting}
                                    >
                                        Redefinir Senha
                                    </Button>
                                </div>

                                <div className="text-center">
                                    <button
                                        type="button"
                                        onClick={() => router.push('/login')}
                                        className="text-sm text-gray-500 hover:text-primary transition-colors"
                                    >
                                        Cancelar e voltar ao login
                                    </button>
                                </div>
                            </motion.form>
                        )}
                    </AnimatePresence>
                </div>
            </div>
        </div>
    );
}

export default function ResetPasswordPage() {
    return (
        <Suspense fallback={
            <div className="min-h-screen bg-gray-50 flex items-center justify-center">
                <ArrowPathIcon className="w-10 h-10 text-primary animate-spin" />
            </div>
        }>
            <ResetPasswordContent />
        </Suspense>
    );
}
