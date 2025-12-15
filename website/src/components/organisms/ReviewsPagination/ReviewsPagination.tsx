'use client';

import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { ChevronLeftIcon, ChevronRightIcon } from '@heroicons/react/24/outline';

interface ReviewsPaginationProps {
  currentPage: number;
  totalPages: number;
  onPageChange?: (page: number) => void;
  className?: string;
}

export const ReviewsPagination = ({ 
  currentPage, 
  totalPages, 
  onPageChange,
  className = '' 
}: ReviewsPaginationProps) => {
  
  const handlePrevious = () => {
    if (currentPage > 1 && onPageChange) {
      onPageChange(currentPage - 1);
    }
  };

  const handleNext = () => {
    if (currentPage < totalPages && onPageChange) {
      onPageChange(currentPage + 1);
    }
  };

  const isPreviousDisabled = currentPage <= 1;
  const isNextDisabled = currentPage >= totalPages;

  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
      className={`max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8 ${className}`}
    >
      <div className="flex items-center justify-center gap-6">
        
        {/* Previous Button */}
        <button
          onClick={handlePrevious}
          disabled={isPreviousDisabled}
          className={`
            flex items-center gap-2 px-4 py-2 rounded-md transition-colors duration-200
            ${isPreviousDisabled 
              ? 'text-gray-400 cursor-not-allowed' 
              : 'text-gray-600 hover:text-gray-900 hover:bg-gray-50'
            }
          `}
        >
          <ChevronLeftIcon className="w-4 h-4" />
          <Typography variant="body" className="font-medium">
            Anterior
          </Typography>
        </button>

        {/* Page Info */}
        <div className="flex items-center gap-2">
          <Typography variant="body" className="text-gray-600">
            Página
          </Typography>
          <Typography variant="body" className="text-gray-900 font-bold">
            {currentPage}
          </Typography>
          <Typography variant="body" className="text-gray-600">
            de
          </Typography>
          <Typography variant="body" className="text-gray-900 font-bold">
            {totalPages}
          </Typography>
        </div>

        {/* Next Button */}
        <button
          onClick={handleNext}
          disabled={isNextDisabled}
          className={`
            flex items-center gap-2 px-4 py-2 rounded-md transition-colors duration-200
            ${isNextDisabled 
              ? 'text-gray-400 cursor-not-allowed' 
              : 'text-gray-600 hover:text-gray-900 hover:bg-gray-50'
            }
          `}
        >
          <Typography variant="body" className="font-medium">
            Próximo
          </Typography>
          <ChevronRightIcon className="w-4 h-4" />
        </button>
      </div>
    </motion.div>
  );
}; 