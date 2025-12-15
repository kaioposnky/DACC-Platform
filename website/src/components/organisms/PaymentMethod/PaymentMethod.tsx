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
  const [selectedMethod, setSelectedMethod] = useState<PaymentData['method']>('credit');
  const [paymentData, setPaymentData] = useState<PaymentData>({
    method: 'credit',
    cardNumber: '',
    expiryDate: '',
    cvv: '',
    nameOnCard: '',
    pixKey: '',
    bankCode: ''
  });

  const paymentOptions = [
    {
      id: 'credit' as const,
      name: 'Cartão de Crédito',
      icon: <CreditCardIcon className="w-8 h-8" />
    },
    {
      id: 'debit' as const,
      name: 'Cartão de Débito',
      icon: <CreditCardIcon className="w-8 h-8" />
    },
    {
      id: 'pix' as const,
      name: 'PIX',
      icon: <QrCodeIcon className="w-8 h-8" />
    },
    {
      id: 'transfer' as const,
      name: 'Transferência Bancária',
      icon: <BuildingLibraryIcon className="w-8 h-8" />
    }
  ];

  const handleMethodChange = (method: PaymentData['method']) => {
    setSelectedMethod(method);
    const updatedData = { ...paymentData, method };
    setPaymentData(updatedData);
    if (onPaymentDataChange) {
      onPaymentDataChange(updatedData);
    }
  };

  const handleInputChange = (field: keyof PaymentData, value: string) => {
    const updatedData = { ...paymentData, [field]: value };
    setPaymentData(updatedData);
    if (onPaymentDataChange) {
      onPaymentDataChange(updatedData);
    }
  };

  const formatCardNumber = (value: string) => {
    // Remove all non-digits
    const digits = value.replace(/\D/g, '');
    // Add spaces every 4 digits
    return digits.replace(/(\d{4})(?=\d)/g, '$1 ');
  };

  const formatExpiryDate = (value: string) => {
    // Remove all non-digits
    const digits = value.replace(/\D/g, '');
    // Add slash after 2 digits
    if (digits.length >= 2) {
      return `${digits.slice(0, 2)}/${digits.slice(2, 4)}`;
    }
    return digits;
  };

  const handleCardNumberChange = (value: string) => {
    const formatted = formatCardNumber(value);
    if (formatted.replace(/\s/g, '').length <= 16) {
      handleInputChange('cardNumber', formatted);
    }
  };

  const handleExpiryChange = (value: string) => {
    const formatted = formatExpiryDate(value);
    if (formatted.replace(/\D/g, '').length <= 4) {
      handleInputChange('expiryDate', formatted);
    }
  };

  const handleCvvChange = (value: string) => {
    const digits = value.replace(/\D/g, '');
    if (digits.length <= 4) {
      handleInputChange('cvv', digits);
    }
  };

  const renderPaymentForm = () => {
    switch (selectedMethod) {
      case 'credit':
      case 'debit':
        return (
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.3 }}
            className="space-y-4"
          >
            {/* Card Number */}
            <Input
              label="Número do Cartão *"
              type="text"
              value={paymentData.cardNumber || ''}
              onChange={(e) => handleCardNumberChange(e.target.value)}
              placeholder="1234 5678 9012 3456"
              maxLength={19}
              required
            />

            {/* Expiry Date and CVV */}
            <div className="grid grid-cols-2 gap-4">
              <Input
                label="Data de Validade *"
                type="text"
                value={paymentData.expiryDate || ''}
                onChange={(e) => handleExpiryChange(e.target.value)}
                placeholder="MM/AA"
                maxLength={5}
                required
              />
              <Input
                label="CVV *"
                type="text"
                value={paymentData.cvv || ''}
                onChange={(e) => handleCvvChange(e.target.value)}
                placeholder="123"
                maxLength={4}
                required
              />
            </div>

            {/* Name on Card */}
            <Input
              label="Nome no Cartão *"
              type="text"
              value={paymentData.nameOnCard || ''}
              onChange={(e) => handleInputChange('nameOnCard', e.target.value)}
              placeholder="João Silva"
              required
            />
          </motion.div>
        );

      case 'pix':
        return (
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.3 }}
            className="text-center py-8"
          >
            <QrCodeIcon className="w-24 h-24 mx-auto text-gray-400 mb-4" />
            <Typography variant="h5" className="text-gray-900 font-semibold mb-2">
              Pagamento via PIX
            </Typography>
            <Typography variant="body" className="text-gray-600 mb-4">
              Após confirmar o pedido, você receberá um QR Code para realizar o pagamento via PIX.
            </Typography>
            <div className="bg-blue-50 border border-blue-200 rounded-lg p-4">
              <Typography variant="caption" className="text-blue-800">
                <strong>Vantagens do PIX:</strong><br />
                • Pagamento instantâneo<br />
                • Disponível 24/7<br />
                • Sem taxas adicionais
              </Typography>
            </div>
          </motion.div>
        );

      case 'transfer':
        return (
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.3 }}
            className="text-center py-8"
          >
            <BuildingLibraryIcon className="w-24 h-24 mx-auto text-gray-400 mb-4" />
            <Typography variant="h5" className="text-gray-900 font-semibold mb-2">
              Transferência Bancária
            </Typography>
            <Typography variant="body" className="text-gray-600 mb-4">
              Após confirmar o pedido, você receberá os dados bancários para realizar a transferência.
            </Typography>
            <div className="bg-yellow-50 border border-yellow-200 rounded-lg p-4">
              <Typography variant="caption" className="text-yellow-800">
                <strong>Importante:</strong><br />
                • Prazo de até 2 dias úteis para compensação<br />
                • Envie o comprovante por email<br />
                • Pedido processado após confirmação do pagamento
              </Typography>
            </div>
          </motion.div>
        );

      default:
        return null;
    }
  };

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
          Método de Pagamento
        </Typography>
      </div>

      {/* Payment Options Grid */}
      <div className="grid grid-cols-2 gap-4 mb-6">
        {paymentOptions.map((option, index) => (
          <motion.div
            key={option.id}
            initial={{ opacity: 0, scale: 0.95 }}
            animate={{ opacity: 1, scale: 1 }}
            transition={{ duration: 0.3, delay: index * 0.1 }}
            className={`
              relative border rounded-lg p-6 cursor-pointer transition-all duration-200 text-center
              ${selectedMethod === option.id 
                ? 'border-primary bg-blue-50 shadow-sm' 
                : 'border-gray-200 hover:border-gray-300'
              }
            `}
            onClick={() => handleMethodChange(option.id)}
          >
            <div className="flex flex-col items-center gap-3">
              <div className={`${selectedMethod === option.id ? 'text-primary' : 'text-gray-600'}`}>
                {option.icon}
              </div>
              <Typography 
                variant="body" 
                className={`font-medium ${selectedMethod === option.id ? 'text-primary' : 'text-gray-900'}`}
              >
                {option.name}
              </Typography>
            </div>
          </motion.div>
        ))}
      </div>

      {/* Payment Form */}
      <div className="border-t border-gray-200 pt-6">
        {renderPaymentForm()}
      </div>
    </motion.div>
  );
}; 