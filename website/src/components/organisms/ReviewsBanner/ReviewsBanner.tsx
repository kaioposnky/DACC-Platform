'use client';

import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { ChevronRightIcon } from '@heroicons/react/24/outline';
import Link from 'next/link';

interface ReviewsBannerProps {
  className?: string;
}

export const ReviewsBanner = ({ className = '' }: ReviewsBannerProps) => {
  return (
    <section className={`relative py-16 bg-primary overflow-hidden ${className}`}>
      <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 relative z-10">
        <div className="flex flex-col lg:flex-row lg:items-center lg:justify-between">
          
          {/* Left Content - Title and Subtitle */}
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5 }}
            className="flex-1"
          >
            <Typography 
              variant="h1" 
              color="white"
              className="font-bold text-3xl md:text-4xl lg:text-5xl mb-4"
            >
              Minhas Avaliações
            </Typography>
            <Typography 
              variant="body" 
              color="white"
              className="opacity-90 text-lg"
            >
              Gerencie suas avaliações e classificações de produtos
            </Typography>
          </motion.div>

          {/* Right Content - Breadcrumb */}
          <motion.div
            initial={{ opacity: 0, x: 20 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.5, delay: 0.2 }}
            className="mt-6 lg:mt-0 flex-shrink-0"
          >
            <nav className="flex items-center text-white/80 text-sm">
              <Link 
                href="/perfil" 
                className="hover:text-white transition-colors duration-200"
              >
                Perfil
              </Link>
              <ChevronRightIcon className="w-4 h-4 mx-2" />
              <span className="text-white font-medium">Minhas Avaliações</span>
            </nav>
          </motion.div>
        </div>
      </div>
    </section>
  );
}; 