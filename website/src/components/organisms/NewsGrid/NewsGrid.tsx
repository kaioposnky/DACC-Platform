"use client";

import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { NewsCard } from '@/components/molecules';
import { News } from '@/types';

interface NewsGridProps {
  news: News[];
  loading?: boolean;
  layout?: 'featured' | 'grid' | 'list';
  className?: string;
  emptyMessage?: string;
}

export const NewsGrid = ({ 
  news, 
  loading = false, 
  layout = 'featured',
  className = '',
  emptyMessage = "No news available at the moment."
}: NewsGridProps) => {

  // Loading skeleton
  if (loading) {
    const skeletonCount = layout === 'featured' ? 3 : 6;
    const gridCols = layout === 'list' ? 'grid-cols-1' : 
                    layout === 'featured' ? 'grid-cols-1 lg:grid-cols-3' : 
                    'grid-cols-1 md:grid-cols-2 lg:grid-cols-3';

    return (
      <div className={`grid ${gridCols} gap-8 ${className}`}>
        {[...Array(skeletonCount)].map((_, index) => (
          <div 
            key={index} 
            className={`animate-pulse ${
              layout === 'featured' && index === 0 ? 'lg:col-span-2' : ''
            }`}
          >
            <div className="bg-white rounded-lg shadow-md overflow-hidden h-96">
              <div className="h-48 bg-gray-200"></div>
              <div className="p-6">
                <div className="h-4 bg-gray-200 rounded w-3/4 mb-3"></div>
                <div className="h-6 bg-gray-200 rounded w-full mb-3"></div>
                <div className="h-4 bg-gray-200 rounded w-full mb-2"></div>
                <div className="h-4 bg-gray-200 rounded w-2/3"></div>
              </div>
            </div>
          </div>
        ))}
      </div>
    );
  }

  // Empty state
  if (news.length === 0) {
    return (
      <motion.div
        className={`text-center py-12 ${className}`}
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ duration: 0.6 }}
      >
        <Typography variant="body" color="gray">
          {emptyMessage}
        </Typography>
      </motion.div>
    );
  }

  // Featured layout (first item larger, rest smaller)
  if (layout === 'featured') {
    return (
      <div className={`grid grid-cols-1 lg:grid-cols-3 gap-8 ${className}`}>
        {/* First news item - larger */}
        {news[0] && (
          <motion.div
            key={news[0].id}
            initial={{ opacity: 0, y: 30 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.6, delay: 0.2 }}
            className="lg:col-span-2"
          >
            <NewsCard news={news[0]} />
          </motion.div>
        )}

        {/* Remaining news items - smaller, in right column */}
        {news.length > 1 && 
          news.slice(1).map((newsItem, index) => (
            <motion.div
              key={newsItem.id}
              initial={{ opacity: 0, y: 30 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.6, delay: 0.3 + index * 0.1 }}
              className="lg:col-span-1"
            >
              <NewsCard news={newsItem} />
            </motion.div>
          ))
        }
      </div>
    );
  }

  // Grid layout (regular grid)
  if (layout === 'grid') {
    return (
      <div className={`grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 ${className}`}>
        {news.map((newsItem, index) => (
          <motion.div
            key={newsItem.id}
            initial={{ opacity: 0, y: 30 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.6, delay: index * 0.1 }}
          >
            <NewsCard news={newsItem} />
          </motion.div>
        ))}
      </div>
    );
  }

  // List layout (single column)
  if (layout === 'list') {
    return (
      <div className={`grid grid-cols-1 gap-6 ${className}`}>
        {news.map((newsItem, index) => (
          <motion.div
            key={newsItem.id}
            initial={{ opacity: 0, y: 30 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.6, delay: index * 0.05 }}
          >
            <NewsCard news={newsItem} />
          </motion.div>
        ))}
      </div>
    );
  }

  return null;
}; 