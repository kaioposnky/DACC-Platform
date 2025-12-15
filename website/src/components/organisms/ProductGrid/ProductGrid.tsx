'use client';

import { useState, useEffect, useCallback } from 'react';
import { Product } from '@/types';
import { apiService } from '@/services/api';
import ProductCard from '@/components/molecules/ProductCard';
import { motion } from 'framer-motion';
import { ArrowPathIcon } from '@heroicons/react/24/outline';
import { useCart } from '@/context/CartContext';

interface ProductGridProps {
  filters?: {
    category?: string;
    sortBy?: string;
    search?: string;
  };
  className?: string;
}

const PRODUCTS_PER_PAGE = 9;

export default function ProductGrid({ filters = {}, className = '' }: ProductGridProps) {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [loadingMore, setLoadingMore] = useState(false);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [favorites, setFavorites] = useState<string[]>([]);
  const { addToCart } = useCart();

  const loadProducts = useCallback(async (currentPage: number, isReset: boolean = false) => {
    try {
      if (isReset) {
        setLoading(true);
      } else {
        setLoadingMore(true);
      }

      const response = await apiService.getProducts({
        ...filters,
        page: currentPage,
        limit: PRODUCTS_PER_PAGE,
      });

      if (isReset) {
        setProducts(response);
      } else {
        setProducts(prev => [...prev, ...response]);
      }

      // Check if we have more products
      setHasMore(response.length === PRODUCTS_PER_PAGE);
      setError(null);
    } catch (err) {
      setError('Erro ao carregar produtos. Tente novamente.');
      console.error('Error loading products:', err);
    } finally {
      setLoading(false);
      setLoadingMore(false);
    }
  }, [filters]);

  // Reset when filters change
  useEffect(() => {
    setPage(1);
    setProducts([]);
    setHasMore(true);
    setError(null);
    loadProducts(1, true);
  }, [filters.category, filters.sortBy, filters.search, loadProducts]);

  // Load more products when page changes
  useEffect(() => {
    if (page > 1) {
      loadProducts(page, false);
    }
  }, [page, loadProducts]);

  const handleLoadMore = () => {
    if (!loadingMore && hasMore) {
      setPage(prev => prev + 1);
    }
  };

  const handleAddToCart = (product: Product) => {
    addToCart(product, 1);
  };

  const handleToggleFavorite = (product: Product) => {
    setFavorites(prev => {
      if (prev.includes(product.id)) {
        return prev.filter(id => id !== product.id);
      } else {
        return [...prev, product.id];
      }
    });
  };

  const containerVariants = {
    hidden: { opacity: 0 },
    visible: {
      opacity: 1,
      transition: {
        staggerChildren: 0.1,
      },
    },
  };

  const itemVariants = {
    hidden: { opacity: 0, y: 20 },
    visible: {
      opacity: 1,
      y: 0,
      transition: {
        duration: 0.3,
      },
    },
  };

  if (loading) {
    return (
      <div className={`${className}`}>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {[...Array(PRODUCTS_PER_PAGE)].map((_, index) => (
            <div key={index} className="animate-pulse">
              <div className="bg-gray-200 aspect-square rounded-xl mb-4"></div>
              <div className="space-y-2">
                <div className="h-4 bg-gray-200 rounded w-3/4"></div>
                <div className="h-4 bg-gray-200 rounded w-1/2"></div>
                <div className="h-6 bg-gray-200 rounded w-1/4"></div>
              </div>
            </div>
          ))}
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className={`text-center py-12 ${className}`}>
        <div className="max-w-md mx-auto">
          <div className="text-red-500 mb-4">
            <svg className="mx-auto h-12 w-12" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.996-.833-2.464 0L3.34 16.5c-.77.833.192 2.5 1.732 2.5z" />
            </svg>
          </div>
          <h3 className="text-lg font-semibold text-gray-900 mb-2">Erro ao carregar produtos</h3>
          <p className="text-gray-600 mb-4">{error}</p>
          <button
            onClick={() => loadProducts(1, true)}
            className="bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 px-4 rounded-lg transition-colors duration-200"
          >
            Tentar novamente
          </button>
        </div>
      </div>
    );
  }

  if (products.length === 0) {
    return (
      <div className={`text-center py-12 ${className}`}>
        <div className="max-w-md mx-auto">
          <div className="text-gray-400 mb-4">
            <svg className="mx-auto h-12 w-12" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4" />
            </svg>
          </div>
          <h3 className="text-lg font-semibold text-gray-900 mb-2">Nenhum produto encontrado</h3>
          <p className="text-gray-600">
            Não encontramos produtos que correspondam aos seus critérios de pesquisa. 
            Tente ajustar os filtros ou fazer uma nova busca.
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className={className}>
      <motion.div
        variants={containerVariants}
        initial="hidden"
        animate="visible"
        className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6"
      >
        {products.map((product) => (
          <motion.div key={product.id} variants={itemVariants}>
            <ProductCard
              product={product}
              onAddToCart={handleAddToCart}
              onToggleFavorite={handleToggleFavorite}
              isFavorite={favorites.includes(product.id)}
            />
          </motion.div>
        ))}
      </motion.div>

      {/* Load More Button */}
      {hasMore && (
        <div className="text-center mt-8">
          <motion.button
            whileHover={{ scale: 1.02 }}
            whileTap={{ scale: 0.98 }}
            onClick={handleLoadMore}
            disabled={loadingMore}
            className="bg-blue-600 hover:bg-blue-700 disabled:bg-blue-400 text-white font-semibold py-3 px-8 rounded-lg transition-colors duration-200 flex items-center gap-2 mx-auto"
          >
            {loadingMore ? (
              <>
                <ArrowPathIcon className="w-5 h-5 animate-spin" />
                Carregando mais produtos...
              </>
            ) : (
              <>
                Carregar mais produtos
              </>
            )}
          </motion.button>
        </div>
      )}

      {/* No More Products Message */}
      {!hasMore && products.length > 0 && (
        <div className="text-center mt-8 py-6">
          <p className="text-gray-500">
            Você viu todos os {products.length} produtos disponíveis.
          </p>
        </div>
      )}
    </div>
  );
} 