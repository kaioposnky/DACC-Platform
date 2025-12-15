'use client';

import { useState } from 'react';
import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { TruckIcon } from '@heroicons/react/24/outline';

export interface DeliveryOption {
  id: string;
  name: string;
  description: string;
  cost: number;
  displayCost: string;
}

interface DeliveryOptionsProps {
  onDeliveryOptionChange?: (option: DeliveryOption) => void;
  className?: string;
}

export const DeliveryOptions = ({ onDeliveryOptionChange, className = '' }: DeliveryOptionsProps) => {
  const deliveryOptions: DeliveryOption[] = [
    {
      id: 'campus',
      name: 'Entrega no Campus',
      description: 'Entrega no ponto de coleta do campus (3-5 dias úteis)',
      cost: 0,
      displayCost: 'GRÁTIS'
    },
    {
      id: 'standard',
      name: 'Entrega Padrão',
      description: 'Entrega em domicílio (5-7 dias úteis)',
      cost: 5.99,
      displayCost: 'R$ 5,99'
    },
    {
      id: 'express',
      name: 'Entrega Expressa',
      description: 'Entrega rápida em domicílio (2-3 dias úteis)',
      cost: 12.99,
      displayCost: 'R$ 12,99'
    }
  ];

  const [selectedOption, setSelectedOption] = useState<string>(deliveryOptions[0].id);

  const handleOptionChange = (optionId: string) => {
    setSelectedOption(optionId);
    const option = deliveryOptions.find(opt => opt.id === optionId);
    if (option && onDeliveryOptionChange) {
      onDeliveryOptionChange(option);
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
        <TruckIcon className="w-6 h-6 text-primary" />
        <Typography variant="h4" className="text-gray-900 font-semibold">
          Opções de Entrega
        </Typography>
      </div>

      {/* Delivery Options */}
      <div className="space-y-4">
        {deliveryOptions.map((option, index) => (
          <motion.div
            key={option.id}
            initial={{ opacity: 0, x: -20 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.3, delay: index * 0.1 }}
            className={`
              relative border rounded-lg p-4 cursor-pointer transition-all duration-200
              ${selectedOption === option.id 
                ? 'border-primary bg-blue-50 shadow-sm' 
                : 'border-gray-200 hover:border-gray-300'
              }
            `}
            onClick={() => handleOptionChange(option.id)}
          >
            {/* Radio Button */}
            <div className="flex items-start gap-4">
              <div className="flex items-center h-6">
                <input
                  type="radio"
                  name="delivery-option"
                  value={option.id}
                  checked={selectedOption === option.id}
                  onChange={() => handleOptionChange(option.id)}
                  className="w-4 h-4 text-primary border-gray-300 focus:ring-primary focus:ring-2"
                />
              </div>
              
              {/* Option Content */}
              <div className="flex-1 min-w-0">
                <div className="flex items-center justify-between">
                  <Typography variant="h6" className="text-gray-900 font-medium">
                    {option.name}
                  </Typography>
                  <Typography 
                    variant="body" 
                    className={`font-semibold ${option.cost === 0 ? 'text-green-600' : 'text-gray-900'}`}
                  >
                    {option.displayCost}
                  </Typography>
                </div>
                <Typography variant="body" className="text-gray-600 mt-1">
                  {option.description}
                </Typography>
              </div>
            </div>
          </motion.div>
        ))}
      </div>

      {/* Additional Info */}
      <div className="mt-6 p-4 bg-gray-50 rounded-lg">
        <Typography variant="caption" className="text-gray-600">
          <strong>Nota:</strong> Os prazos de entrega são contados em dias úteis a partir da confirmação do pagamento.
        </Typography>
      </div>
    </motion.div>
  );
}; 