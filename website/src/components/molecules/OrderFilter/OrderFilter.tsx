"use client";

import { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { OrderStatus } from '@/types';
import { Input, Select } from '@/components';
import { useDebounce } from '@/hooks/useDebounce';

export interface OrderFilterOptions {
  searchQuery: string;
  status: OrderStatus | 'all';
  startDate: string;
  endDate: string;
}

interface OrderFilterProps {
  onFilterChange: (filters: OrderFilterOptions) => void;
  className?: string;
}

const statusOptions: { value: OrderStatus | 'all'; label: string }[] = [
  { value: 'all', label: 'Todos os Pedidos' },
  { value: 'created', label: 'Criado' },
  { value: 'pending', label: 'Pendente' },
  { value: 'approved', label: 'Aprovado' },
  { value: 'rejected', label: 'Rejeitado' },
  { value: 'delivered', label: 'Entregue' },
  { value: 'cancelled', label: 'Cancelado' },
];

export const OrderFilter = ({
  onFilterChange,
  className = ''
}: OrderFilterProps) => {
  // Estado local para o input de busca (atualização imediata para o usuário)
  const [searchQuery, setSearchQuery] = useState('');

  // Valor com debounce (atraso na propagação)
  const debouncedSearch = useDebounce(searchQuery, 600);

  const [filters, setFilters] = useState<Omit<OrderFilterOptions, 'searchQuery'>>({
    status: 'all',
    startDate: '',
    endDate: '',
  });

  // Efeito para propagar mudança na busca quando o debounce terminar
  useEffect(() => {
    onFilterChange({
      ...filters,
      searchQuery: debouncedSearch
    });
  }, [debouncedSearch, filters]);

  const handleFilterChange = (key: keyof Omit<OrderFilterOptions, 'searchQuery'>, value: string) => {
    const newFilters = { ...filters, [key]: value };
    setFilters(newFilters);
  };

  return (
    <motion.div
      className={`bg-gray-50 border-b border-gray-200 py-8 ${className}`}
      initial={{ opacity: 0, y: -20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.6 }}
    >
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex flex-col lg:flex-row lg:items-end lg:justify-between gap-6">
          {/* Filter Controls */}
          <div className="flex flex-wrap items-start sm:items-end gap-4 lg:gap-6 flex-1">
            <div className="min-w-[180px]">
              <Select
                label="Status"
                value={filters.status}
                options={statusOptions}
                onChange={(e) => handleFilterChange('status', e.target.value)}
              />
            </div>

            <div className="min-w-[160px]">
              <Input
                label="Início"
                type="date"
                value={filters.startDate}
                onChange={(e) => handleFilterChange('startDate', e.target.value)}
              />
            </div>

            <div className="min-w-[160px]">
              <Input
                label="Fim"
                type="date"
                value={filters.endDate}
                onChange={(e) => handleFilterChange('endDate', e.target.value)}
              />
            </div>
          </div>

          {/* Search Input */}
          <div className="w-full lg:w-auto lg:min-w-[350px]">
            <Input
              label="Pesquisar"
              type="text"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              placeholder="ID, Nome ou E-mail..."
            />
          </div>
        </div>
      </div>
    </motion.div>
  );
};
