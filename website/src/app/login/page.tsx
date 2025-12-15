'use client';

import { useState } from 'react';
import { motion } from 'framer-motion';
import { FormInput, SocialLoginButton } from '@/components/molecules';
import { CommunityBenefits, Footer } from '@/components/organisms';
import { ArrowRightIcon } from '@heroicons/react/24/outline';
import Link from 'next/link';
import { Navigation } from '@/components/organisms/Navigation';

export default function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError(null);

    try {
      // Login logic would be implemented here
      // For now, just simulate a login process
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // Redirect to dashboard or home page after successful login
      window.location.href = '/';
    } catch {
      setError('Login failed. Please check your credentials and try again.');
    } finally {
      setIsLoading(false);
    }
  };

  const handleSocialLogin = async (provider: 'google' | 'github') => {
    setIsLoading(true);
    setError(null);

    try {
      // Social login logic would be implemented here
      // For now, just simulate the process
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // Redirect after successful social login
      window.location.href = '/';
    } catch {
      setError(`${provider} login failed. Please try again.`);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <>
      <Navigation />

      <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 flex gap-12 items-center justify-center my-12">
        <div className="flex-1 flex items-center justify-center p-8 shadow-xl rounded-xl bg-white">
          <motion.div
            initial={{ opacity: 0, x: -20 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.5 }}
            className="max-w-md w-full space-y-8"
          >
            {/* Header */}
            <div className="text-center">
              <motion.h1
                initial={{ opacity: 0, y: -20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.5, delay: 0.1 }}
                className="text-3xl font-bold text-gray-900"
              >
                Bem-vindo de volta!
              </motion.h1>
              <motion.p
                initial={{ opacity: 0, y: -20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.5, delay: 0.2 }}
                className="mt-2 text-gray-600"
              >
                Faça login para continuar
              </motion.p>
            </div>

            {/* Error Message */}
            {error && (
              <motion.div
                initial={{ opacity: 0, y: -10 }}
                animate={{ opacity: 1, y: 0 }}
                className="bg-red-50 border border-red-200 rounded-lg p-4"
              >
                <p className="text-sm text-red-600">{error}</p>
              </motion.div>
            )}

            {/* Login Form */}
            <motion.form
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.5, delay: 0.3 }}
              onSubmit={handleSubmit}
              className="space-y-6"
            >
              <FormInput
                id="email"
                type="email"
                label="Endereço de email"
                placeholder="Digite seu email"
                value={email}
                onChange={setEmail}
                required
              />

              <FormInput
                id="password"
                type="password"
                label="Senha"
                placeholder="Digite sua senha"
                value={password}
                onChange={setPassword}
                required
              />

              <div className="flex items-center justify-between">
                <label className="flex items-center">
                  <input
                    type="checkbox"
                    checked={rememberMe}
                    onChange={(e) => setRememberMe(e.target.checked)}
                    disabled={isLoading}
                    className="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded"
                  />
                  <span className="ml-2 text-sm text-gray-600">Lembrar-me</span>
                </label>
                <Link href="/forgot-password" className="text-sm text-blue-600 hover:text-blue-500 transition-colors duration-200">
                  Esqueceu sua senha?
                </Link>
              </div>

              <motion.button
                whileHover={{ scale: 1.02 }}
                whileTap={{ scale: 0.98 }}
                type="submit"
                disabled={isLoading}
                className="w-full flex items-center justify-center gap-2 bg-blue-600 text-white py-3 px-4 rounded-lg hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 transition-colors duration-200 font-semibold disabled:opacity-50 disabled:cursor-not-allowed"
              >
                {isLoading ? (
                  <div className="animate-spin rounded-full h-5 w-5 border-b-2 border-white"></div>
                ) : (
                  <ArrowRightIcon className="w-5 h-5" />
                )}
                {isLoading ? 'Entrando...' : 'Entrar'}
              </motion.button>
            </motion.form>

            {/* Social Login */}
            <motion.div
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.5, delay: 0.4 }}
            >
              <div className="relative">
                <div className="absolute inset-0 flex items-center">
                  <div className="w-full border-t border-gray-300" />
                </div>
                <div className="relative flex justify-center text-sm">
                  <span className="px-2 bg-gray-50 text-gray-500">ou continue com</span>
                </div>
              </div>

              <div className="mt-6 grid grid-cols-2 gap-3">
                <SocialLoginButton
                  provider="google"
                  onClick={() => handleSocialLogin('google')}
                />
                <SocialLoginButton
                  provider="github"
                  onClick={() => handleSocialLogin('github')}
                />
              </div>
            </motion.div>

            {/* Sign Up Link */}
            <motion.div
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.5, delay: 0.5 }}
              className="text-center"
            >
              <p className="text-sm text-gray-600">
                Não tem uma conta?{' '}
                <Link href="/register" className="text-blue-600 hover:text-blue-500 font-medium transition-colors duration-200">
                  Crie uma aqui
                </Link>
              </p>
            </motion.div>
          </motion.div>
        </div>
    
        <div className="lg:flex lg:flex-1 bg-blue-900 items-center justify-center px-8 rounded-xl shadow-xl p-8">
          <motion.div
            initial={{ opacity: 0, x: 20 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.6, delay: 0.2 }}
            className="max-w-md w-full"
          >
            <CommunityBenefits />
          </motion.div>
        </div>
      </div>

      <Footer />
    </>
  );
} 