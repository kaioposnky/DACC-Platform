"use client";

import { useState } from 'react';
import { motion } from 'framer-motion';

export interface FilterOptions {
  category: string;
  date: string;
  sortBy: string;
  viewMode: 'grid' | 'list';
}

interface NewsFilterProps {
  onFilterChange?: (filters: FilterOptions) => void;
  className?: string;
}

export const NewsFilter = ({ 
  onFilterChange,
  className = '' 
}: NewsFilterProps) => {
  const [filters, setFilters] = useState<FilterOptions>({
    category: 'all',
    date: 'all',
    sortBy: 'latest',
    viewMode: 'grid'
  });

  const categoryOptions = [
    { value: 'all', label: 'Todas as Categorias' },
    { value: 'technology', label: 'Tecnologia' },
    { value: 'education', label: 'Educação' },
    { value: 'research', label: 'Pesquisa' },
    { value: 'events', label: 'Eventos' },
    { value: 'announcements', label: 'Comunicados' }
  ];

  const dateOptions = [
    { value: 'all', label: 'Todos os Tempos' },
    { value: 'today', label: 'Hoje' },
    { value: 'week', label: 'Esta Semana' },
    { value: 'month', label: 'Este Mês' },
    { value: 'year', label: 'Este Ano' }
  ];

  const sortOptions = [
    { value: 'latest', label: 'Mais Recentes' },
    { value: 'oldest', label: 'Mais Antigas' },
    { value: 'popular', label: 'Mais Populares' },
    { value: 'title', label: 'Alfabético' }
  ];

  const handleFilterChange = (key: keyof FilterOptions, value: string | 'grid' | 'list') => {
    const newFilters = { ...filters, [key]: value };
    setFilters(newFilters);
    if (onFilterChange) {
      onFilterChange(newFilters);
    }
  };

  const SelectDropdown = ({ 
    label, 
    value, 
    options, 
    onChange 
  }: { 
    label: string; 
    value: string; 
    options: { value: string; label: string }[]; 
    onChange: (value: string) => void; 
  }) => (
    <div className="flex flex-col gap-2">
      <label className="text-sm font-bold text-primary min-w-fit">
        {label}:
      </label>

      <select
        value={value}
        onChange={(e) => onChange(e.target.value)}
        className="px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent bg-white text-gray-700 min-w-[140px]"
      >
        {options.map((option) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
    </div>
  );

  const ViewToggle = () => (
    <div className="flex items-center gap-2">
      <button
        onClick={() => handleFilterChange('viewMode', 'grid')}
        className={`p-2 rounded-lg transition-all duration-200 ${
          filters.viewMode === 'grid'
            ? 'bg-primary text-white'
            : 'bg-gray-100 text-gray-600 hover:bg-gray-200'
        }`}
        title="Visualização em Grid"
      >
        <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
          <path d="M4 4h4v4H4V4zm6 0h4v4h-4V4zm6 0h4v4h-4V4zM4 10h4v4H4v-4zm6 0h4v4h-4v-4zm6 0h4v4h-4v-4zM4 16h4v4H4v-4zm6 0h4v4h-4v-4zm6 0h4v4h-4v-4z"/>
        </svg>
      </button>
      <button
        onClick={() => handleFilterChange('viewMode', 'list')}
        className={`p-2 rounded-lg transition-all duration-200 ${
          filters.viewMode === 'list'
            ? 'bg-primary text-white'
            : 'bg-gray-100 text-gray-600 hover:bg-gray-200'
        }`}
        title="Visualização em Lista"
      >
        <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 24 24">
          <path d="M4 6h16v2H4V6zm0 5h16v2H4v-2zm0 5h16v2H4v-2z"/>
        </svg>
      </button>
    </div>
  );

  return (
    <motion.div
      className={`bg-gray-50 border-b border-gray-200 py-8 ${className}`}
      initial={{ opacity: 0, y: -20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.6 }}
    >
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-4">
          {/* Filter Controls */}
          <div className="flex flex-col sm:flex-row items-start sm:items-center gap-4 lg:gap-6">
            <SelectDropdown
              label="Categoria"
              value={filters.category}
              options={categoryOptions}
              onChange={(value) => handleFilterChange('category', value)}
            />
            
            <SelectDropdown
              label="Data"
              value={filters.date}
              options={dateOptions}
              onChange={(value) => handleFilterChange('date', value)}
            />
            
            <SelectDropdown
              label="Ordenar por"
              value={filters.sortBy}
              options={sortOptions}
              onChange={(value) => handleFilterChange('sortBy', value)}
            />
          </div>

          {/* View Toggle */}
          <div className="flex justify-end">
            <ViewToggle />
          </div>
        </div>
      </div>
    </motion.div>
  );
}; 