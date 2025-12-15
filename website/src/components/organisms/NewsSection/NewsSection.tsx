"use client";

import { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { NewsGrid } from '@/components/organisms';
import { News } from '@/types';
import { apiService } from '@/services/api';

interface NewsSectionProps {
  className?: string;
}

export const NewsSection = ({ className = '' }: NewsSectionProps) => {
  const [news, setNews] = useState<News[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchNews();
  }, []);

  const fetchNews = async () => {
    try {
      setLoading(true);
      const fetchedNews = await apiService.getNews();
      setNews(fetchedNews);
    } catch (error) {
      console.error('Error fetching news:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <section className={`py-16 bg-white ${className}`}>
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-12">
            <div className="animate-pulse">
              <div className="h-8 bg-gray-200 rounded w-64 mx-auto mb-4"></div>
              <div className="h-4 bg-gray-200 rounded w-96 mx-auto mb-8"></div>
            </div>
          </div>
          <NewsGrid news={[]} loading={true} layout="featured" />
        </div>
      </section>
    );
  }

  return (
    <section className={`py-16 bg-white ${className}`}>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Section Header */}
        <motion.div
          className="text-center mb-12"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6 }}
        >
          <Typography variant="h1" className="mb-4 text-primary font-bold" align="center">
            Últimas Notícias
          </Typography>
          <Typography variant="subtitle" color="gray" className="max-w-2xl mx-auto" align="center">
            Acompanhe as últimas notícias do DACC
          </Typography>
        </motion.div>

        {/* News Grid */}
        <NewsGrid 
          news={news} 
          loading={false} 
          layout="featured"
          emptyMessage="Nenhuma notícia disponível no momento."
        />
      </div>
    </section>
  );
}; 