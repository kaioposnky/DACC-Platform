"use client";

import { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { apiService } from '@/services/api';
import { Input, Select } from '@/components';
import { useDebounce } from '@/hooks/useDebounce';

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
  const [categories, setCategories] = useState<{ value: string; label: string }[]>([
    { value: 'all', label: 'Todos os Produtos' }
  ]);

  // Estado local para busca
  const [searchQuery, setSearchQuery] = useState('');

  // Debounce da busca
  const debouncedSearch = useDebounce(searchQuery, 600);

  const [filters, setFilters] = useState<Omit<ProductFilterOptions, 'searchQuery'>>({
    category: 'all',
    sortBy: 'featured',
  });

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const subcategories = await apiService.getSubcategories();
        const options = [
          { value: 'all', label: 'Todos os Produtos' },
          ...subcategories.map(sub => ({
            value: sub.name,
            label: sub.name
          }))
        ];
        setCategories(options);
      } catch (error) {
        console.error('Error fetching categories:', error);
      }
    };

    fetchCategories();
  }, []);

  // Propagar mudança na busca quando debounce terminar
  useEffect(() => {
    if (onFilterChange) {
      onFilterChange({
        ...filters,
        searchQuery: debouncedSearch
      });
    }
  }, [debouncedSearch, filters]);

  const sortOptions = [
    { value: 'featured', label: 'Destaque' },
    { value: 'price-low', label: 'Preço: Baixo para Alto' },
    { value: 'price-high', label: 'Preço: Alto para Baixo' },
    { value: 'newest', label: 'Mais Recentes' },
    { value: 'popular', label: 'Mais Populares' },
    { value: 'name', label: 'Nome A-Z' }
  ];

  const handleFilterChange = (key: keyof Omit<ProductFilterOptions, 'searchQuery'>, value: string) => {
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
          {/* Search Input */}
          <div className="w-full lg:w-auto lg:min-w-87.5">
            <Input
              label="Pesquisar"
              type="text"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              placeholder="Pesquisar produtos..."
              className="h-11"
            />
          </div>

          {/* Filter Controls */}
          <div className="flex flex-col sm:flex-row items-start sm:items-end gap-4 lg:gap-6 flex-1">
            <div className="min-w-50 w-full sm:w-auto">
              <Select
                label="Categoria"
                value={filters.category}
                options={categories}
                onChange={(e) => handleFilterChange('category', e.target.value)}
                className="h-11"
              />
            </div>

            <div className="min-w-50 w-full sm:w-auto">
              <Select
                label="Ordenar por"
                value={filters.sortBy}
                options={sortOptions}
                onChange={(e) => handleFilterChange('sortBy', e.target.value)}
                className="h-11"
              />
            </div>
          </div>

        </div>
      </div>
    </motion.div>
  );
};
