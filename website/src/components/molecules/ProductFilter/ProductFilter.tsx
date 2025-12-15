"use client";

import { useState } from 'react';
import { motion } from 'framer-motion';

export interface ProductFilterOptions {
  category: string;
  sortBy: string;
  searchQuery: string;
}

interface ProductFilterProps {
  onFilterChange?: (filters: ProductFilterOptions) => void;
  className?: string;
}

export const ProductFilter = ({ 
  onFilterChange,
  className = '' 
}: ProductFilterProps) => {
  const [filters, setFilters] = useState<ProductFilterOptions>({
    category: 'all',
    sortBy: 'featured',
    searchQuery: ''
  });

  const categoryOptions = [
    { value: 'all', label: 'Todos os Produtos' },
    { value: 'tshirts', label: 'Camisetas' },
    { value: 'hoodies', label: 'Moletons' },
    { value: 'cups', label: 'Canecas' },
    { value: 'stickers', label: 'Adesivos' },
    { value: 'accessories', label: 'Acessórios' }
  ];

  const sortOptions = [
    { value: 'featured', label: 'Destaque' },
    { value: 'price-low', label: 'Preço: Baixo para Alto' },
    { value: 'price-high', label: 'Preço: Alto para Baixo' },
    { value: 'newest', label: 'Mais Recentes' },
    { value: 'popular', label: 'Mais Populares' },
    { value: 'name', label: 'Nome A-Z' }
  ];

  const handleFilterChange = (key: keyof ProductFilterOptions, value: string) => {
    const newFilters = { ...filters, [key]: value };
    setFilters(newFilters);
    if (onFilterChange) {
      onFilterChange(newFilters);
    }
  };

  const handleSearchSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    // Search is handled by the onChange, but we can add additional logic here if needed
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

  const SearchInput = () => (
    <div className="flex flex-col gap-2">
      <label className="text-sm font-bold text-primary min-w-fit">
        Pesquisar:
      </label>
      <form onSubmit={handleSearchSubmit} className="relative">
        <div className="relative flex items-center">
          <input
            type="text"
            value={filters.searchQuery}
            onChange={(e) => handleFilterChange('searchQuery', e.target.value)}
            placeholder="Pesquisar produtos..."
            className="w-full pl-4 pr-12 py-3 border border-gray-300 rounded-l-lg focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent text-gray-700"
          />
          <button
            type="submit"
            className="px-4 py-3 bg-primary hover:bg-blue-700 text-white rounded-r-lg transition-colors duration-200 flex items-center justify-center border-l border-gray-300"
          >
            <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
            </svg>
          </button>
        </div>
      </form>
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
        <div className="flex flex-col lg:flex-row lg:items-end lg:justify-between gap-4">
          {/* Filter Controls */}
          <div className="flex flex-col sm:flex-row items-start sm:items-end gap-4 lg:gap-6">
            <SelectDropdown
              label="Categoria"
              value={filters.category}
              options={categoryOptions}
              onChange={(value) => handleFilterChange('category', value)}
            />
            
            <SelectDropdown
              label="Ordenar por"
              value={filters.sortBy}
              options={sortOptions}
              onChange={(value) => handleFilterChange('sortBy', value)}
            />
          </div>

          {/* Search Input */}
          <div className="w-full lg:w-auto lg:min-w-[300px]">
            <SearchInput />
          </div>
        </div>
      </div>
    </motion.div>
  );
}; 