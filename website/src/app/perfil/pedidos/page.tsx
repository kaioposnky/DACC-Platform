'use client'

import { useState } from 'react'
import { motion } from 'framer-motion'
import { Footer, Navigation } from "@/components"
import { OrderHistoryBanner } from '@/components/organisms/OrderHistoryBanner'
import { OrderHistoryFilter } from '@/components/organisms/OrderHistoryFilter'
import { OrderHistoryPagination } from '@/components/organisms/OrderHistoryPagination'

export default function OrderHistoryPage() {
  const [currentPage, setCurrentPage] = useState(1)
  const totalPages = 3

  const handlePageChange = (page: number) => {
    setCurrentPage(page)
    // Here you would typically fetch orders for the new page
  }

  const handleFilterChange = (filters: {
    status: string
    dateRange: string
    searchTerm: string
  }) => {
    // Filter logic would go here
    console.log('Filters changed:', filters)
    setCurrentPage(1); // Reset to first page when filters change
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />
      
      <OrderHistoryBanner />
      <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-8">

      <OrderHistoryFilter onFilterChange={handleFilterChange} />

      {/* Orders List Placeholder */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5, delay: 0.2 }}
          className="bg-white rounded-lg shadow-sm border border-gray-200 p-8"
        >
          <div className="text-center">
            <div className="text-gray-400 mb-4">
              <svg className="h-16 w-16 mx-auto" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
              </svg>
            </div>
            <h3 className="text-lg font-medium text-gray-900 mb-2">Histórico de pedidos</h3>
            <p className="text-gray-600">
              Seu histórico de pedidos aparecerá aqui assim que você começar a fazer pedidos.
            </p>
          </div>
        </motion.div>

      <OrderHistoryPagination 
        currentPage={currentPage}
        totalPages={totalPages}
        onPageChange={handlePageChange}
      />
      </div>

      
      <Footer />
    </div>
  )
}