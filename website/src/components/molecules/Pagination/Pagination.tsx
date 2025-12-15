"use client";

import { motion } from 'framer-motion';

interface PaginationProps {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
  className?: string;
}

export const Pagination = ({ 
  currentPage, 
  totalPages, 
  onPageChange,
  className = '' 
}: PaginationProps) => {
  
  const hasPrevious = currentPage > 1;
  const hasNext = currentPage < totalPages;

  const handlePrevious = () => {
    if (hasPrevious) {
      onPageChange(currentPage - 1);
    }
  };

  const handleNext = () => {
    if (hasNext) {
      onPageChange(currentPage + 1);
    }
  };

  // Don't render pagination if there's only one page or no pages
  if (totalPages <= 1) {
    return null;
  }

  return (
    <motion.div
      className={`flex items-center justify-center gap-8 py-8 ${className}`}
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.6 }}
    >
      {/* Previous Button */}
      <button
        onClick={handlePrevious}
        disabled={!hasPrevious}
        className={`flex items-center gap-2 px-4 py-2 rounded-lg transition-all duration-200 ${
          hasPrevious 
            ? 'text-gray-600 hover:text-primary hover:bg-gray-100' 
            : 'text-gray-400 cursor-not-allowed'
        }`}
      >
        <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" />
        </svg>
        <span className="font-medium">Anterior</span>
      </button>

      {/* Page Info */}
      <div className="flex items-center gap-2 text-sm text-gray-600">
        <span>Página</span>
        <span className="font-semibold text-primary">{currentPage}</span>
        <span>de</span>
        <span className="font-semibold text-primary">{totalPages}</span>
      </div>

      {/* Next Button */}
      <button
        onClick={handleNext}
        disabled={!hasNext}
        className={`flex items-center gap-2 px-4 py-2 rounded-lg transition-all duration-200 ${
          hasNext 
            ? 'text-gray-600 hover:text-primary hover:bg-gray-100' 
            : 'text-gray-400 cursor-not-allowed'
        }`}
      >
        <span className="font-medium">Próximo</span>
        <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5l7 7-7 7" />
        </svg>
      </button>
    </motion.div>
  );
}; 