"use client";

import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { SearchInput } from '@/components/molecules';

interface PageBannerProps {
  title: string;
  subtitle?: string;
  showSearch?: boolean;
  searchPlaceholder?: string;
  onSearch?: (query: string) => void;
  backgroundColor?: 'primary' | 'secondary' | 'dark' | 'blue' | 'gradient';
  className?: string;
  showDecorations?: boolean;
}

export const PageBanner = ({
  title,
  subtitle,
  showSearch = false,
  searchPlaceholder = "Search...",
  onSearch,
  backgroundColor = 'primary',
  className = '',
  showDecorations = true
}: PageBannerProps) => {
  
  const getBackgroundClasses = () => {
    switch (backgroundColor) {
      case 'primary':
        return 'bg-primary';
      case 'secondary':
        return 'bg-secondary';
      case 'dark':
        return 'bg-slate-800';
      case 'blue':
        return 'bg-blue-900';
      case 'gradient':
        return 'bg-gradient-to-br from-blue-900 via-blue-800 to-blue-700';
      default:
        return 'bg-primary';
    }
  };

  const getDecorationIcons = () => ({
    topLeft: (
      <svg className="w-20 h-20 text-white/5" fill="currentColor" viewBox="0 0 24 24">
        <path d="M9.4 16.6L4.8 12l4.6-4.6L8 6l-6 6 6 6 1.4-1.4zm5.2 0L19.2 12l-4.6-4.6L16 6l6 6-6 6-1.4-1.4z"/>
      </svg>
    ),
    topRight: (
      <svg className="w-16 h-16 text-white/5" fill="currentColor" viewBox="0 0 24 24">
        <path d="M12 2l3.09 6.26L22 9l-5 4.74L18.18 21L12 17.27L5.82 21L7 13.74L2 9l6.91-1.26L12 2z"/>
      </svg>
    ),
    bottomLeft: (
      <svg className="w-24 h-24 text-white/5" fill="currentColor" viewBox="0 0 24 24">
        <path d="M9.5 7.5c0 .83-.67 1.5-1.5 1.5s-1.5-.67-1.5-1.5S7.17 6 8 6s1.5.67 1.5 1.5zM9 17l3-3.16c.31-.33.85-.33 1.16 0L16 17H9z"/>
        <path d="M20 6h-8l-2-2H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V8c0-1.1-.9-2-2-2z"/>
      </svg>
    ),
    bottomRight: (
      <svg className="w-18 h-18 text-white/5" fill="currentColor" viewBox="0 0 24 24">
        <path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zM9 17H7v-7h2v7zm4 0h-2V7h2v10zm4 0h-2v-4h2v4z"/>
      </svg>
    )
  });

  const decorations = getDecorationIcons();

  return (
    <section className={`relative py-20 lg:py-28 overflow-hidden ${getBackgroundClasses()} ${className}`}>
      {/* Decorative Icons */}
      {showDecorations && (
        <>
          <motion.div
            className="absolute top-8 left-8 hidden lg:block"
            initial={{ opacity: 0, x: -50, y: -50 }}
            animate={{ opacity: 1, x: 0, y: 0 }}
            transition={{ duration: 1, delay: 0.5 }}
          >
            {decorations.topLeft}
          </motion.div>
          
          <motion.div
            className="absolute top-12 right-12 hidden lg:block"
            initial={{ opacity: 0, x: 50, y: -50 }}
            animate={{ opacity: 1, x: 0, y: 0 }}
            transition={{ duration: 1, delay: 0.7 }}
          >
            {decorations.topRight}
          </motion.div>

          <motion.div
            className="absolute bottom-8 left-12 hidden lg:block"
            initial={{ opacity: 0, x: -50, y: 50 }}
            animate={{ opacity: 1, x: 0, y: 0 }}
            transition={{ duration: 1, delay: 0.9 }}
          >
            {decorations.bottomLeft}
          </motion.div>

          <motion.div
            className="absolute bottom-12 right-8 hidden lg:block"
            initial={{ opacity: 0, x: 50, y: 50 }}
            animate={{ opacity: 1, x: 0, y: 0 }}
            transition={{ duration: 1, delay: 1.1 }}
          >
            {decorations.bottomRight}
          </motion.div>
        </>
      )}

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 relative z-10">
        {/* Content */}
        <div className="text-center">
          {/* Title */}
          <motion.div
            initial={{ opacity: 0, y: 30 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8 }}
            className="mb-6"
          >
            <Typography 
              variant="h1" 
              color="white"
              className="font-bold text-4xl md:text-5xl lg:text-6xl"
              align="center"
            >
              {title}
            </Typography>
          </motion.div>

          {/* Subtitle */}
          {subtitle && (
            <motion.div
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.8, delay: 0.2 }}
              className="mb-12"
            >
              <Typography 
                variant="subtitle" 
                color="white"
                className="max-w-4xl mx-auto text-lg md:text-xl opacity-90"
                align="center"
              >
                {subtitle}
              </Typography>
            </motion.div>
          )}

          {/* Search Input */}
          {showSearch && (
            <div className="flex justify-center">
              <SearchInput
                placeholder={searchPlaceholder}
                onSearch={onSearch}
                className="w-full max-w-2xl"
              />
            </div>
          )}
        </div>
      </div>
    </section>
  );
}; 