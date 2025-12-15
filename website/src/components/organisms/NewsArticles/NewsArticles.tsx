"use client";

import { useState, useEffect, useMemo } from 'react';
import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { Pagination } from '@/components/molecules';
import { NewsGrid } from '@/components/organisms';
import { News } from '@/types';
import { apiService } from '@/services/api';
import type { FilterOptions } from '@/components/molecules/NewsFilter/NewsFilter';

interface NewsArticlesProps {
  filters: FilterOptions;
  searchQuery?: string;
  className?: string;
}

const ITEMS_PER_PAGE = 9;

export const NewsArticles = ({ 
  filters, 
  searchQuery = '',
  className = '' 
}: NewsArticlesProps) => {
  const [allNews, setAllNews] = useState<News[]>([]);
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);

  useEffect(() => {
    fetchAllNews();
  }, []);

  useEffect(() => {
    // Reset to first page when filters change
    setCurrentPage(1);
  }, [filters, searchQuery]);

  const fetchAllNews = async () => {
    try {
      setLoading(true);
      const fetchedNews = await apiService.getNews();
      setAllNews(fetchedNews);
    } catch (error) {
      console.error('Error fetching news:', error);
    } finally {
      setLoading(false);
    }
  };

  // Filter and search news
  const filteredNews = useMemo(() => {
    let filtered = [...allNews];

    // Apply category filter
    if (filters.category !== 'all') {
      filtered = filtered.filter(news => news.category === filters.category);
    }

    // Apply date filter
    if (filters.date !== 'all') {
      const now = new Date();
      const filterDate = new Date();
      
      switch (filters.date) {
        case 'today':
          filterDate.setHours(0, 0, 0, 0);
          filtered = filtered.filter(news => new Date(news.date) >= filterDate);
          break;
        case 'week':
          filterDate.setDate(now.getDate() - 7);
          filtered = filtered.filter(news => new Date(news.date) >= filterDate);
          break;
        case 'month':
          filterDate.setMonth(now.getMonth() - 1);
          filtered = filtered.filter(news => new Date(news.date) >= filterDate);
          break;
        case 'year':
          filterDate.setFullYear(now.getFullYear() - 1);
          filtered = filtered.filter(news => new Date(news.date) >= filterDate);
          break;
      }
    }

    // Apply search filter
    if (searchQuery.trim()) {
      const query = searchQuery.toLowerCase().trim();
      filtered = filtered.filter(news => 
        news.title.toLowerCase().includes(query) ||
        (news.content?.toLowerCase().includes(query)) ||
        news.category.toLowerCase().includes(query)
      );
    }

    // Apply sorting
    switch (filters.sortBy) {
      case 'latest':
        filtered.sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime());
        break;
      case 'oldest':
        filtered.sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime());
        break;
      case 'title':
        filtered.sort((a, b) => a.title.localeCompare(b.title));
        break;
      case 'popular':
        // For now, sort by latest since we don't have view counts
        filtered.sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime());
        break;
    }

    return filtered;
  }, [allNews, filters, searchQuery]);

  // Pagination
  const totalPages = Math.ceil(filteredNews.length / ITEMS_PER_PAGE);
  const startIndex = (currentPage - 1) * ITEMS_PER_PAGE;
  const endIndex = startIndex + ITEMS_PER_PAGE;
  const paginatedNews = filteredNews.slice(startIndex, endIndex);

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
    // Scroll to top when changing pages
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  return (
    <section className={`py-16 bg-white ${className}`}>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Section Header */}
        <motion.div
          className="flex items-center justify-between mb-12"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6 }}
        >
          <div>
            <Typography variant="h1" className="mb-2 text-primary font-bold">
              Últimas Notícias
            </Typography>
            <Typography variant="body" color="gray" className="text-sm">
              {loading ? 'Carregando...' : `${filteredNews.length} artigos encontrados`}
            </Typography>
          </div>
        </motion.div>

        {/* News Grid */}
        <NewsGrid 
          news={paginatedNews}
          loading={loading}
          layout={filters.viewMode === 'grid' ? 'grid' : 'list'}
          emptyMessage={
            searchQuery.trim() 
              ? `Nenhum artigo encontrado para "${searchQuery}"`
              : "Nenhum artigo encontrado com os filtros atuais."
          }
        />

        {/* Pagination */}
        {!loading && filteredNews.length > 0 && (
          <Pagination
            currentPage={currentPage}
            totalPages={totalPages}
            onPageChange={handlePageChange}
            className="mt-8"
          />
        )}
      </div>
    </section>
  );
}; 