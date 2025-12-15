'use client'

import { motion } from 'framer-motion'
import { ChevronLeftIcon, ChevronRightIcon } from '@heroicons/react/24/outline'

export interface OrderHistoryPaginationProps {
  className?: string
  currentPage: number
  totalPages: number
  onPageChange: (page: number) => void
}

export const OrderHistoryPagination: React.FC<OrderHistoryPaginationProps> = ({ 
  className = '',
  currentPage,
  totalPages,
  onPageChange
}) => {
  const handlePreviousPage = () => {
    if (currentPage > 1) {
      onPageChange(currentPage - 1)
    }
  }

  const handleNextPage = () => {
    if (currentPage < totalPages) {
      onPageChange(currentPage + 1)
    }
  }

  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
      className={`bg-white rounded-lg shadow-sm border border-gray-200 p-6 my-6 ${className} max-w-6xl mx-auto px-4 sm:px-6`}
    >
      <div className="flex items-center justify-between">
        {/* Previous Button */}
        <motion.button
          whileHover={currentPage > 1 ? { scale: 1.05 } : {}}
          whileTap={currentPage > 1 ? { scale: 0.95 } : {}}
          onClick={handlePreviousPage}
          disabled={currentPage === 1}
          className={`flex items-center space-x-2 px-4 py-2 rounded-md transition-colors ${
            currentPage === 1
              ? 'text-gray-400 cursor-not-allowed'
              : 'text-gray-700 hover:bg-gray-50'
          }`}
        >
          <ChevronLeftIcon className="h-5 w-5" />
          <span>Anterior</span>
        </motion.button>

        {/* Page Info */}
        <div className="flex items-center space-x-2">
          <span className="text-sm text-gray-600">
            Página {currentPage} de {totalPages}
          </span>
        </div>

        {/* Next Button */}
        <motion.button
          whileHover={currentPage < totalPages ? { scale: 1.05 } : {}}
          whileTap={currentPage < totalPages ? { scale: 0.95 } : {}}
          onClick={handleNextPage}
          disabled={currentPage === totalPages}
          className={`flex items-center space-x-2 px-4 py-2 rounded-md transition-colors ${
            currentPage === totalPages
              ? 'text-gray-400 cursor-not-allowed'
              : 'text-gray-700 hover:bg-gray-50'
          }`}
        >
          <span>Próximo</span>
          <ChevronRightIcon className="h-5 w-5" />
        </motion.button>
      </div>
    </motion.div>
  )
} 