'use client';

import { useState } from 'react';
import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { Input } from '@/components/atoms/Input';
import { TruckIcon } from '@heroicons/react/24/outline';

export interface DeliveryFormData {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
}

interface DeliveryInformationProps {
  onFormChange?: (data: DeliveryFormData) => void;
  className?: string;
}

export const DeliveryInformation = ({ onFormChange, className = '' }: DeliveryInformationProps) => {
  const [formData, setFormData] = useState<DeliveryFormData>({
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
  });

  const [errors, setErrors] = useState<Partial<DeliveryFormData>>({});

  const handleInputChange = (field: keyof DeliveryFormData, value: string) => {
    const updatedData = { ...formData, [field]: value };
    setFormData(updatedData);
    
    // Clear error when user starts typing
    if (errors[field]) {
      setErrors(prev => ({ ...prev, [field]: '' }));
    }
    
    // Call parent callback
    if (onFormChange) {
      onFormChange(updatedData);
    }
  };

  const formatPhone = (value: string) => {
    // Remove all non-digits
    const digits = value.replace(/\D/g, '');
    
    // Apply format: (00) 00000-0000
    if (digits.length <= 2) {
      return digits;
    } else if (digits.length <= 7) {
      return `(${digits.slice(0, 2)}) ${digits.slice(2)}`;
    } else {
      return `(${digits.slice(0, 2)}) ${digits.slice(2, 7)}-${digits.slice(7, 11)}`;
    }
  };

  const handlePhoneChange = (value: string) => {
    const formatted = formatPhone(value);
    handleInputChange('phone', formatted);
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
          Informações de Entrega
        </Typography>
      </div>

      {/* Form */}
      <form className="space-y-4">
        {/* First Name and Last Name */}
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <Input
            label="Nome *"
            type="text"
            value={formData.firstName}
            onChange={(e) => handleInputChange('firstName', e.target.value)}
            error={errors.firstName}
            required
          />
          <Input
            label="Sobrenome *"
            type="text"
            value={formData.lastName}
            onChange={(e) => handleInputChange('lastName', e.target.value)}
            error={errors.lastName}
            required
          />
        </div>

        {/* Email Address */}
        <Input
          label="Endereço de Email *"
          type="email"
          value={formData.email}
          onChange={(e) => handleInputChange('email', e.target.value)}
          error={errors.email}
          required
        />

        {/* Phone Number */}
        <Input
          label="Número de Telefone *"
          type="tel"
          value={formData.phone}
          onChange={(e) => handlePhoneChange(e.target.value)}
          error={errors.phone}
          placeholder="(00) 00000-0000"
          maxLength={15}
          required
        />
      </form>
    </motion.div>
  );
}; 