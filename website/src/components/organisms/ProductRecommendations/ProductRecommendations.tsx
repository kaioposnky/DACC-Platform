'use client';

import { useState, useEffect } from 'react';
import { Product } from '@/types';
import { apiService } from '@/services/api';
import RecommendationCard from '@/components/molecules/RecommendationCard';
import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms/Typography';

interface ProductRecommendationsProps {
  currentProduct: Product;
  className?: string;
}

export default function ProductRecommendations({ currentProduct, className = '' }: ProductRecommendationsProps) {
  const [recommendedProducts, setRecommendedProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchRecommendations = async () => {
      try {
        setLoading(true);
        
        // First try to get products from the same category
        let products = await apiService.getProducts({
          category: currentProduct.category,
          sortBy: 'featured'
        });

        // Filter out the current product
        products = products.filter(product => product.id !== currentProduct.id);

        // If we don't have enough products from the same category, get more from other categories
        if (products.length < 4) {
          const additionalProducts = await apiService.getProducts({
            sortBy: 'featured'
          });
          
          const filteredAdditional = additionalProducts
            .filter(product => 
              product.id !== currentProduct.id && 
              !products.some(p => p.id === product.id)
            );
          
          products = [...products, ...filteredAdditional];
        }

        // Limit to 4 products for the recommendation grid
        setRecommendedProducts(products.slice(0, 4));
      } catch (error) {
        console.error('Error fetching recommendations:', error);
        setRecommendedProducts([]);
      } finally {
        setLoading(false);
      }
    };

    fetchRecommendations();
  }, [currentProduct.id, currentProduct.category]);

  if (loading) {
    return (
      <div className={`${className}`}>
        <h2 className="text-2xl font-bold text-gray-900 text-center mb-8">You might also like</h2>
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-6">
          {[...Array(4)].map((_, index) => (
            <div key={index} className="animate-pulse">
              <div className="bg-gray-200 aspect-square rounded-xl mb-4"></div>
              <div className="space-y-2">
                <div className="h-4 bg-gray-200 rounded w-3/4 mx-auto"></div>
                <div className="h-6 bg-gray-200 rounded w-1/2 mx-auto"></div>
                <div className="h-8 bg-gray-200 rounded"></div>
              </div>
            </div>
          ))}
        </div>
      </div>
    );
  }

  if (recommendedProducts.length === 0) {
    return null;
  }

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

  return (
    <div className={`bg-gray-50 rounded-xl p-8 ${className}`}>
      <Typography variant="h3" className="text-2xl font-bold text-primary mb-8 !text-center">Você também pode gostar</Typography>
      
      <motion.div
        variants={containerVariants}
        initial="hidden"
        animate="visible"
        className="grid grid-cols-2 lg:grid-cols-4 gap-6"
      >
        {recommendedProducts.map((product) => (
          <motion.div key={product.id} variants={itemVariants}>
            <RecommendationCard product={product} />
          </motion.div>
        ))}
      </motion.div>
    </div>
  );
} 