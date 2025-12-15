'use client';

import { useState } from 'react';
import { motion } from 'framer-motion';
import { Typography, Button } from '@/components/atoms';
import { Input } from '@/components/atoms/Input';

export interface UserFormData {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  studentId: string;
  course: string;
  academicYear: string;
  expectedGraduation: string;
}

interface ProfileSettingsFormProps {
  initialData?: Partial<UserFormData>;
  onSave?: (data: UserFormData) => void;
  onReset?: () => void;
  className?: string;
}

export const ProfileSettingsForm = ({
  initialData = {},
  onSave,
  onReset,
  className = ''
}: ProfileSettingsFormProps) => {
  
  const [formData, setFormData] = useState<UserFormData>({
    firstName: initialData.firstName || '',
    lastName: initialData.lastName || '',
    email: initialData.email || '',
    phone: initialData.phone || '',
    studentId: initialData.studentId || '',
    course: initialData.course || '',
    academicYear: initialData.academicYear || '',
    expectedGraduation: initialData.expectedGraduation || ''
  });

  const [errors, setErrors] = useState<Partial<UserFormData>>({});
  const [isDirty, setIsDirty] = useState(false);

  const courses = [
    'Ciência da Computação',
    'Engenharia de Software',
    'Sistemas de Informação',
    'Engenharia da Computação',
    'Análise e Desenvolvimento de Sistemas'
  ];

  const academicYears = [
    '1º Ano',
    '2º Ano', 
    '3º Ano',
    '4º Ano',
    '5º Ano'
  ];

  const handleInputChange = (field: keyof UserFormData, value: string) => {
    setFormData(prev => ({ ...prev, [field]: value }));
    setIsDirty(true);
    
    // Clear error when user starts typing
    if (errors[field]) {
      setErrors(prev => ({ ...prev, [field]: '' }));
    }
  };

  const formatPhone = (value: string) => {
    // Remove all non-digits
    const digits = value.replace(/\D/g, '');
    
    // Apply Brazilian phone format: +55 (00) 00000-0000
    if (digits.length <= 2) {
      return `+${digits}`;
    } else if (digits.length <= 4) {
      return `+${digits.slice(0, 2)} (${digits.slice(2)})`;
    } else if (digits.length <= 9) {
      return `+${digits.slice(0, 2)} (${digits.slice(2, 4)}) ${digits.slice(4)}`;
    } else {
      return `+${digits.slice(0, 2)} (${digits.slice(2, 4)}) ${digits.slice(4, 9)}-${digits.slice(9, 13)}`;
    }
  };

  const handlePhoneChange = (value: string) => {
    const formatted = formatPhone(value);
    handleInputChange('phone', formatted);
  };

  const validateForm = (): boolean => {
    const newErrors: Partial<UserFormData> = {};

    if (!formData.firstName.trim()) newErrors.firstName = 'Nome é obrigatório';
    if (!formData.lastName.trim()) newErrors.lastName = 'Sobrenome é obrigatório';
    if (!formData.email.trim()) newErrors.email = 'Email é obrigatório';
    if (!formData.email.includes('@')) newErrors.email = 'Email inválido';
    if (!formData.phone.trim()) newErrors.phone = 'Telefone é obrigatório';
    if (!formData.studentId.trim()) newErrors.studentId = 'ID do estudante é obrigatório';
    if (!formData.course) newErrors.course = 'Curso é obrigatório';
    if (!formData.academicYear) newErrors.academicYear = 'Ano acadêmico é obrigatório';
    if (!formData.expectedGraduation.trim()) newErrors.expectedGraduation = 'Data de formatura é obrigatória';

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSave = () => {
    if (validateForm()) {
      onSave?.(formData);
      setIsDirty(false);
    }
  };

  const handleReset = () => {
    setFormData({
      firstName: initialData.firstName || '',
      lastName: initialData.lastName || '',
      email: initialData.email || '',
      phone: initialData.phone || '',
      studentId: initialData.studentId || '',
      course: initialData.course || '',
      academicYear: initialData.academicYear || '',
      expectedGraduation: initialData.expectedGraduation || ''
    });
    setErrors({});
    setIsDirty(false);
    onReset?.();
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
          Configurações da Conta
        </Typography>
        <Typography variant="body" className="text-gray-600">
          Gerencie suas informações pessoais e detalhes da conta
        </Typography>
      </div>

      {/* Personal Information Section */}
      <div className="mb-8">
        <Typography variant="h4" className="text-gray-900 font-semibold mb-6">
          Informações Pessoais
        </Typography>
        
        <div className="space-y-6">
          {/* First Name and Last Name */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <Input
              label="Nome"
              type="text"
              value={formData.firstName}
              onChange={(e) => handleInputChange('firstName', e.target.value)}
              error={errors.firstName}
              placeholder="João"
              required
            />
            <Input
              label="Sobrenome"
              type="text"
              value={formData.lastName}
              onChange={(e) => handleInputChange('lastName', e.target.value)}
              error={errors.lastName}
              placeholder="Silva"
              required
            />
          </div>

          {/* Email Address */}
          <Input
            label="Endereço de Email"
            type="email"
            value={formData.email}
            onChange={(e) => handleInputChange('email', e.target.value)}
            error={errors.email}
            placeholder="joao.silva@student.university.edu"
            required
          />

          {/* Phone Number */}
          <Input
            label="Número de Telefone"
            type="tel"
            value={formData.phone}
            onChange={(e) => handlePhoneChange(e.target.value)}
            error={errors.phone}
            placeholder="+55 (11) 99999-9999"
            maxLength={20}
            required
          />
        </div>
      </div>

      {/* Academic Information Section */}
      <div className="mb-8">
        <Typography variant="h4" className="text-gray-900 font-semibold mb-6">
          Informações Acadêmicas
        </Typography>
        
        <div className="space-y-6">
          {/* Student ID and Course */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <Input
              label="ID do Estudante"
              type="text"
              value={formData.studentId}
              onChange={(e) => handleInputChange('studentId', e.target.value)}
              error={errors.studentId}
              placeholder="CS2022001"
              required
            />
            
            <div className="w-full">
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Curso *
              </label>
              <select
                value={formData.course}
                onChange={(e) => handleInputChange('course', e.target.value)}
                className="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-colors duration-200"
                required
              >
                <option value="">Selecionar Curso</option>
                {courses.map((course) => (
                  <option key={course} value={course}>
                    {course}
                  </option>
                ))}
              </select>
              {errors.course && (
                <p className="mt-1 text-sm text-red-600">{errors.course}</p>
              )}
            </div>
          </div>

          {/* Academic Year and Expected Graduation */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="w-full">
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Ano Acadêmico *
              </label>
              <select
                value={formData.academicYear}
                onChange={(e) => handleInputChange('academicYear', e.target.value)}
                className="block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-colors duration-200"
                required
              >
                <option value="">Selecionar Ano</option>
                {academicYears.map((year) => (
                  <option key={year} value={year}>
                    {year}
                  </option>
                ))}
              </select>
              {errors.academicYear && (
                <p className="mt-1 text-sm text-red-600">{errors.academicYear}</p>
              )}
            </div>

            <Input
              label="Previsão de Formatura"
              type="month"
              value={formData.expectedGraduation}
              onChange={(e) => handleInputChange('expectedGraduation', e.target.value)}
              error={errors.expectedGraduation}
              required
            />
          </div>
        </div>
      </div>

      {/* Action Buttons */}
      <div className="flex flex-col sm:flex-row gap-4 justify-end pt-6 border-t border-gray-200">
        <Button
          variant="secondary"
          onClick={handleReset}
          disabled={!isDirty}
          className="w-full sm:w-auto"
        >
          Redefinir Alterações
        </Button>
        <Button
          variant="primary"
          onClick={handleSave}
          disabled={!isDirty}
          className="w-full sm:w-auto"
        >
          Salvar Alterações
        </Button>
      </div>
    </motion.div>
  );
}; 