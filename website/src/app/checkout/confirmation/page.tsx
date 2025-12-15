'use client';

import { useEffect } from 'react';
import { motion } from 'framer-motion';
import { Footer, Navigation } from "@/components";
import { CheckoutBanner } from "@/components/organisms/CheckoutBanner";
import { Typography, Button } from "@/components/atoms";
import { CheckCircleIcon } from '@heroicons/react/24/solid';
import Link from 'next/link';

export default function OrderConfirmation() {
  useEffect(() => {
    // You could track order completion here
    console.log('Order confirmation page viewed');
  }, []);

  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />
      <CheckoutBanner currentStep={4} />
      
      <div className="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5 }}
          className="bg-white rounded-lg shadow-sm border border-gray-200 p-8 text-center"
        >
          {/* Success Icon */}
          <motion.div
            initial={{ scale: 0 }}
            animate={{ scale: 1 }}
            transition={{ duration: 0.5, delay: 0.2 }}
            className="flex justify-center mb-6"
          >
            <CheckCircleIcon className="w-16 h-16 text-green-500" />
          </motion.div>

          {/* Success Message */}
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.4 }}
            className="mb-8"
          >
            <Typography variant="h2" className="text-gray-900 font-bold mb-4">
              Pedido Confirmado!
            </Typography>
            <Typography variant="body" className="text-gray-600 mb-6">
              Obrigado pela sua compra! Seu pedido foi processado com sucesso e você receberá um email de confirmação em breve.
            </Typography>
            <Typography variant="body" className="text-gray-500 text-sm">
              Número do pedido: <span className="font-semibold">#{Math.random().toString(36).substr(2, 9).toUpperCase()}</span>
            </Typography>
          </motion.div>

          {/* Actions */}
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.6 }}
            className="flex flex-col sm:flex-row gap-4 justify-center"
          >
            <Link href="/">
              <Button variant="primary" className="w-full sm:w-auto">
                Voltar ao Início
              </Button>
            </Link>
            <Link href="/loja">
              <Button variant="secondary" className="w-full sm:w-auto">
                Continuar Comprando
              </Button>
            </Link>
          </motion.div>
        </motion.div>
      </div>
      
      <Footer />
    </div>
  );
} 