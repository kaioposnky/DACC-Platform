'use client'

import { useState } from 'react'
import { motion } from 'framer-motion'
import { MagnifyingGlassIcon, ChevronDownIcon } from '@heroicons/react/24/outline'

export interface OrderHistoryFilterProps {
  className?: string
  onFilterChange?: (filters: {
    status: string
    dateRange: string
    searchTerm: string
  }) => void
}

export const OrderHistoryFilter: React.FC<OrderHistoryFilterProps> = ({ 
  className = '',
  onFilterChange
}) => {
  const [status, setStatus] = useState('all')
  const [dateRange, setDateRange] = useState('all')
  const [searchTerm, setSearchTerm] = useState('')

  const statusOptions = [
    { value: 'all', label: 'Todos' },
    { value: 'pending', label: 'Pendente' },
    { value: 'processing', label: 'Em processamento' },
    { value: 'shipped', label: 'Enviado' },
    { value: 'delivered', label: 'Entregue' },
    { value: 'cancelled', label: 'Cancelado' }
  ]

  const dateRangeOptions = [
    { value: 'all', label: 'Todos' },
    { value: 'last7', label: 'Últimos 7 dias' },
    { value: 'last30', label: 'Últimos 30 dias' },
    { value: 'last90', label: 'Últimos 90 dias' },
    { value: 'thisYear', label: 'Este ano' },
    { value: 'custom', label: 'Faixa de data' }
  ]

  const notifyFilterChange = (newStatus?: string, newDateRange?: string, newSearchTerm?: string) => {
    const filters = {
      status: newStatus ?? status,
      dateRange: newDateRange ?? dateRange,
      searchTerm: newSearchTerm ?? searchTerm
    }
    
    if (onFilterChange) {
      onFilterChange(filters)
    }
  }

  const handleStatusChange = (newStatus: string) => {
    setStatus(newStatus)
    notifyFilterChange(newStatus)
  }

  const handleDateRangeChange = (newDateRange: string) => {
    setDateRange(newDateRange)
    notifyFilterChange(undefined, newDateRange)
  }

  const handleSearch = () => {
    notifyFilterChange()
  }

  const handleSearchTermChange = (newSearchTerm: string) => {
    setSearchTerm(newSearchTerm)
  }

  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
      className={`bg-white rounded-lg shadow-sm border border-gray-200 p-6 ${className} max-w-6xl mx-auto px-4 sm:px-6 my-6`}
    >
      <div className="flex flex-col md:flex-row gap-4 items-start md:items-center">
        {/* Status Filter */}
        <div className="flex items-center space-x-2">
          <label className="text-sm font-medium text-gray-700">Status:</label>
          <div className="relative">
            <select
              value={status}
              onChange={(e) => handleStatusChange(e.target.value)}
              className="appearance-none bg-white border border-gray-300 rounded-md px-4 py-2 pr-8 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent cursor-pointer"
            >
              {statusOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
            <ChevronDownIcon className="absolute right-2 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400 pointer-events-none" />
          </div>
        </div>

        {/* Date Range Filter */}
        <div className="flex items-center space-x-2">
          <label className="text-sm font-medium text-gray-700">Faixa de data:</label>
          <div className="relative">
            <select
              value={dateRange}
              onChange={(e) => handleDateRangeChange(e.target.value)}
              className="appearance-none bg-white border border-gray-300 rounded-md px-4 py-2 pr-8 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent cursor-pointer"
            >
              {dateRangeOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
            <ChevronDownIcon className="absolute right-2 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400 pointer-events-none" />
          </div>
        </div>

        {/* Search */}
        <div className="flex items-center space-x-2 ml-auto">
          <div className="relative flex-1 min-w-64">
            <input
              type="text"
              placeholder="Pesquisar pedidos..."
              value={searchTerm}
              onChange={(e) => handleSearchTermChange(e.target.value)}
              className="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent text-sm"
              onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
            />
          </div>
          <motion.button
            whileHover={{ scale: 1.05 }}
            whileTap={{ scale: 0.95 }}
            onClick={handleSearch}
            className="bg-blue-600 text-white px-4 py-2 rounded-md hover:bg-blue-700 transition-colors flex items-center space-x-2"
          >
            <MagnifyingGlassIcon className="h-4 w-4" />
          </motion.button>
        </div>
      </div>
    </motion.div>
  )
} 