"use client";

import { useState } from 'react';
import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';

interface NewsletterSignupProps {
  onSubmit?: (email: string) => void;
  className?: string;
}

export const NewsletterSignup = ({
  onSubmit,
  className = ''
}: NewsletterSignupProps) => {
  const [email, setEmail] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isSuccess, setIsSuccess] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!email.trim()) return;

    setIsSubmitting(true);

    try {
      if (onSubmit) {
        await onSubmit(email);
      }

      setIsSuccess(true);
      setEmail('');

      // Reset success state after 3 seconds
      setTimeout(() => {
        setIsSuccess(false);
      }, 3000);
    } catch (error) {
      console.error('Error submitting newsletter:', error);
    } finally {
      setIsSubmitting(false);
    }
  };

  if (isSuccess) {
    return (
      <motion.div
        className={`text-center ${className}`}
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
      >
        <div className="flex items-center justify-center mb-4">
          <div className="w-12 h-12 bg-green-100 rounded-full flex items-center justify-center">
            <svg className="w-6 h-6 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
            </svg>
          </div>
        </div>
        <Typography variant="h4" className="mb-2 text-green-600 font-semibold">
          Inscrito com sucesso!
        </Typography>
        <Typography variant="body" color="gray" className="text-sm">
          Você receberá nossas últimas notícias em breve.
        </Typography>
      </motion.div>
    );
  }

  return (
    <div className={className}>
      <div className="mb-6">
        <Typography variant="h5" className="mb-2 text-primary opacity-90 font-semibold">
          Newsletter
        </Typography>
        <Typography variant="body" color="gray" className="text-sm leading-relaxed">
          Fique atualizado com nossas últimas notícias e comunicados
        </Typography>
      </div>

      <form onSubmit={handleSubmit} className="space-y-4">
        <div className="relative">
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            placeholder="Digite seu email"
            required
            className="w-full px-4 py-3 pr-12 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent transition-all duration-200 text-gray-700"
          />
          <button
            type="submit"
            disabled={isSubmitting || !email.trim()}
            className="absolute right-2 top-1/2 transform -translate-y-1/2 p-2 bg-primary text-white rounded-md hover:bg-blue-700 disabled:bg-gray-300 transition-colors duration-200"
          >
            {isSubmitting ? (
              <svg className="w-5 h-5 animate-spin" fill="none" viewBox="0 0 24 24">
                <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" />
                <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" />
              </svg>
            ) : (
              <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 19l9 2-9-18-9 18 9-2zm0 0v-8" />
              </svg>
            )}
          </button>
        </div>
      </form>
    </div>
  );
}; 