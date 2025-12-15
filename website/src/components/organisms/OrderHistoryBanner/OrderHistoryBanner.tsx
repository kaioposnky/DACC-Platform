'use client'

import { motion } from 'framer-motion'
import { ChevronRightIcon } from '@heroicons/react/24/outline'
import Link from 'next/link'

export interface OrderHistoryBannerProps {
  className?: string
}

export const OrderHistoryBanner: React.FC<OrderHistoryBannerProps> = ({ 
  className = '' 
}) => {
  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
      className={`bg-primary text-white py-12 ${className}`}
    >
      <div className="max-w-6xl px-4 sm:px-6 lg:px-8 mx-auto flex items-center justify-between">
       

        {/* Title and Subtitle */}
        <div className="space-y-4">
          <motion.h1
            initial={{ opacity: 0, x: -20 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.5, delay: 0.2 }}
            className="text-4xl md:text-5xl font-bold"
          >
            Meus pedidos
          </motion.h1>
          
          <motion.p
            initial={{ opacity: 0, x: -20 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.5, delay: 0.4 }}
            className="text-lg text-blue-200 max-w-2xl"
          >
            Acompanhe e gerencie seus pedidos
          </motion.p>
        </div>

         {/* Breadcrumb */}
         <nav className="flex items-center space-x-2 text-sm mb-6">
          <Link 
            href="/perfil" 
            className="text-blue-200 hover:text-white transition-colors"
          >
            Perfil
          </Link>
          <ChevronRightIcon className="h-4 w-4 text-blue-300" />
          <span className="text-white">Meus pedidos</span>
        </nav>
      </div>
    </motion.div>
  )
} 