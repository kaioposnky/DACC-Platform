"use client";

import { motion } from 'framer-motion';
import Link from 'next/link';
import { Card, Typography } from '@/components/atoms';
import { News } from '@/types';

interface NewsCardProps {
  news: News;
  className?: string;
}

export const NewsCard = ({ news, className = '' }: NewsCardProps) => {
  const getIcon = (iconName: string) => {
    switch (iconName) {
      case 'trophy':
        return (
          <svg className="w-12 h-12" fill="currentColor" viewBox="0 0 24 24">
            <path d="M7 4V2a1 1 0 011-1h8a1 1 0 011 1v2h4a1 1 0 011 1v3a3 3 0 01-3 3h-.78l-.267 4.798A2 2 0 0116.002 18H7.998a2 2 0 01-1.951-1.602L5.78 11H5a3 3 0 01-3-3V5a1 1 0 011-1h4zm0 2H4v2a1 1 0 001 1h2V6zm10 0v3h2a1 1 0 001-1V6h-3zM9 3v3h6V3H9z"/>
          </svg>
        );
      case 'graduation':
        return (
          <svg className="w-12 h-12" fill="currentColor" viewBox="0 0 24 24">
            <path d="M12 3L1 9l4 2.18v6L12 21l7-3.82v-6l2-1.09V17h2V9L12 3zm6.82 6L12 12.72 5.18 9 12 5.28 18.82 9zM17 15.99l-5 2.73-5-2.73v-3.72L12 15l5-2.73v3.72z"/>
          </svg>
        );
      case 'laptop':
        return (
          <svg className="w-12 h-12" fill="currentColor" viewBox="0 0 24 24">
            <path d="M20 18c1.1 0 1.99-.9 1.99-2L22 5c0-1.1-.9-2-2-2H4c-1.1 0-2 .9-2 2v11c0 1.1.9 2 2 2H0c0 1.1.9 2 2 2h20c1.1 0 2-.9 2-2h-4zM4 5h16v11H4V5z"/>
          </svg>
        );
      default:
        return (
          <svg className="w-12 h-12" fill="currentColor" viewBox="0 0 24 24">
            <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/>
          </svg>
        );
    }
  };

  const getCategoryColor = (category: string) => {
    switch (category.toLowerCase()) {
      case 'achievement':
        return 'bg-blue-900 text-white';
      case 'alumni':
        return 'bg-blue-900 text-white';
      case 'infrastructure':
        return 'bg-blue-900 text-white';
      default:
        return 'bg-gray-800 text-white';
    }
  };

  return (
    <motion.div
      className={className}
      initial={{ opacity: 0, y: 30 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.6, delay: 0.2 }}
    >
      <Card className="overflow-hidden h-full hover:shadow-xl transition-shadow duration-300 !p-0">
        {/* Gradient Header with Icon */}
        <div className={`h-48 bg-gradient-to-r ${news.gradient} flex items-center justify-center text-white relative`}>
          {getIcon(news.icon)}
        </div>

        {/* Content */}
        <div className="p-6 h-full">
          {/* Date and Category */}
          <div className="flex items-center justify-between mb-4">
            <Typography variant="caption" color="gray">
              {news.date}
            </Typography>
            <span className={`px-3 py-1 rounded-full text-sm font-medium ${getCategoryColor(news.category)}`}>
              {news.category}
            </span>
          </div>

          {/* Title */}
          <Typography variant="h4" className="mb-3 text-primary font-bold line-clamp-2">
            {news.title}
          </Typography>

          {/* Description */}
          <Typography variant="body" color="gray" className="mb-4 line-clamp-3">
            {news.description}
          </Typography>

          {/* Read More Link */}
          <Link
            href={`/noticias/${news.id}`}
            className="inline-flex items-center text-primary hover:text-blue-700 font-medium transition-colors duration-200"
          >
            Leia Mais
            <svg className="w-4 h-4 ml-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5l7 7-7 7" />
            </svg>
          </Link>
        </div>
      </Card>
    </motion.div>
  );
}; 