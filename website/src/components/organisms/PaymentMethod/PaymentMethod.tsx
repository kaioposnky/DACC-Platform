'use client';

import { useState } from 'react';
import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { Input } from '@/components/atoms/Input';
import { 
  CreditCardIcon, 
  BuildingLibraryIcon,
  QrCodeIcon
} from '@heroicons/react/24/outline';

export interface PaymentData {
  method: 'credit' | 'debit' | 'pix' | 'transfer';
  cardNumber?: string;
  expiryDate?: string;
  cvv?: string;
  nameOnCard?: string;
  pixKey?: string;
  bankCode?: string;
}

interface PaymentMethodProps {
  onPaymentDataChange?: (data: PaymentData) => void;
  className?: string;
}

export const PaymentMethod = ({ onPaymentDataChange, className = '' }: PaymentMethodProps) => {
  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
      className={`bg-white rounded-lg shadow-sm border border-gray-200 p-6 ${className}`}
    >
      {/* Header */}
      <div className="flex items-center gap-3 mb-6">
        <CreditCardIcon className="w-6 h-6 text-primary" />
        <Typography variant="h4" className="text-gray-900 font-semibold">
        Pagamento
        </Typography>
      </div>

      {/* Informativo de Checkout Externo */}
      <div className="bg-gray-50 rounded-xl p-8 text-center">
        
        <Typography variant="h5" className="text-gray-900 font-semibold mb-2">
          Finalização Externa
        </Typography>
        
        <Typography variant="body" className="text-gray-600 mb-6 max-w-md mx-auto">
          Para sua segurança, a escolha do método de pagamento (PIX, Cartão ou Boleto) será realizada em nosso ambiente de checkout seguro após a confirmação do pedido.
        </Typography>
      </div>

      <div className="mt-6 pt-6 border-t border-gray-100">
        <Typography variant="caption" className="text-gray-500 flex items-center justify-center gap-2">
          <span className="w-2 h-2 bg-green-500 rounded-full" />
          Ambiente criptografado e seguro
        </Typography>
      </div>
    </motion.div>
  );
}; 