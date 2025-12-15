'use client';

import { useState } from 'react';
import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { MagnifyingGlassIcon } from '@heroicons/react/24/outline';

export interface ReviewsFilterOptions {
  rating: string;
  sortBy: string;
  searchQuery: string;
}

interface ReviewsFilterProps {
  onFilterChange?: (filters: ReviewsFilterOptions) => void;
  className?: string;
}

export const ReviewsFilter = ({ onFilterChange, className = '' }: ReviewsFilterProps) => {
  const [filters, setFilters] = useState<ReviewsFilterOptions>({
    rating: 'all',
    sortBy: 'newest',
    searchQuery: ''
  });

  const ratingOptions = [
    { value: 'all', label: 'Todas as Classificações' },
    { value: '5', label: '5 Estrelas' },
    { value: '4', label: '4 Estrelas' },
    { value: '3', label: '3 Estrelas' },
    { value: '2', label: '2 Estrelas' },
    { value: '1', label: '1 Estrela' }
  ];

  const sortOptions = [
    { value: 'newest', label: 'Mais Recentes' },
    { value: 'oldest', label: 'Mais Antigas' },
    { value: 'highest', label: 'Classificação Mais Alta' },
    { value: 'lowest', label: 'Classificação Mais Baixa' },
    { value: 'helpful', label: 'Mais Úteis' }
  ];

  const handleFilterChange = (field: keyof ReviewsFilterOptions, value: string) => {
    const updatedFilters = { ...filters, [field]: value };
    setFilters(updatedFilters);
    if (onFilterChange) {
      onFilterChange(updatedFilters);
    }
  };

  const handleSearch = () => {
    if (onFilterChange) {
      onFilterChange(filters);
    }
  };

  const handleSearchKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      handleSearch();
    }
  };

  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
      className={`max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-6 ${className}`}
    >
      <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
        <div className="flex flex-col lg:flex-row lg:items-center gap-6">
          
          {/* Rating Filter */}
          <div className="flex items-center gap-3">
            <Typography variant="body" className="text-gray-700 font-medium whitespace-nowrap">
              Classificação:
            </Typography>
            <select
              value={filters.rating}
              onChange={(e) => handleFilterChange('rating', e.target.value)}
              className="px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-colors duration-200"
            >
              {ratingOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </div>

          {/* Sort By */}
          <div className="flex items-center gap-3">
            <Typography variant="body" className="text-gray-700 font-medium whitespace-nowrap">
              Ordenar por:
            </Typography>
            <select
              value={filters.sortBy}
              onChange={(e) => handleFilterChange('sortBy', e.target.value)}
              className="px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-colors duration-200"
            >
              {sortOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </div>

          {/* Search */}
          <div className="flex-1 max-w-md">
            <div className="relative">
              <input
                type="text"
                value={filters.searchQuery}
                onChange={(e) => handleFilterChange('searchQuery', e.target.value)}
                onKeyPress={handleSearchKeyPress}
                placeholder="Pesquisar avaliações..."
                className="w-full pl-4 pr-12 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 transition-colors duration-200"
              />
              <button
                onClick={handleSearch}
                className="absolute right-2 top-1/2 transform -translate-y-1/2 p-2 bg-blue-900 text-white rounded-md hover:bg-blue-800 transition-colors duration-200"
              >
                <MagnifyingGlassIcon className="w-4 h-4" />
              </button>
            </div>
          </div>
        </div>
      </div>
    </motion.div>
  );
}; 