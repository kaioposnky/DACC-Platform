'use client';

import { useState, useEffect } from 'react';
import { Footer, Navigation } from "@/components";
import { ReviewsBanner } from "@/components/organisms/ReviewsBanner";
import { ReviewsStats, ReviewsStatsData } from "@/components/organisms/ReviewsStats";
import { ReviewsFilter, ReviewsFilterOptions } from "@/components/organisms/ReviewsFilter";
import { ReviewsPagination } from "@/components/organisms/ReviewsPagination";
import { useAuth } from '@/context/AuthContext';
import { apiService } from '@/services/api';
import { ProductReview } from '@/types';
import { toast } from 'sonner';
import { motion } from 'framer-motion';
import { StarIcon } from '@heroicons/react/20/solid';
import Link from 'next/link';

export default function ReviewsPage() {
  const { user } = useAuth();
  const [reviews, setReviews] = useState<ProductReview[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  // Pagination state
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;

  useEffect(() => {
    const fetchReviews = async () => {
      if (!user?.id) return;
      try {
        setIsLoading(true);
        const data = await apiService.getReviewsByUserId(user.id);
        setReviews(data || []);
      } catch (error) {
        console.error("Erro ao buscar histórico de avaliações:", error);
        toast.error("Não foi possível carregar as avaliações.");
      } finally {
        setIsLoading(false);
      }
    };
    fetchReviews();
  }, [user]);

  const statsData: ReviewsStatsData = {
    totalReviews: reviews.length,
    averageRating: reviews.length > 0 ? (reviews.reduce((acc, curr) => acc + curr.rating, 0) / reviews.length) : 0,
    helpfulVotes: 0
  };

  const totalPages = Math.ceil(reviews.length / itemsPerPage) || 1;
  const paginatedReviews = reviews.slice((currentPage - 1) * itemsPerPage, currentPage * itemsPerPage);

  const handleFilterChange = (filters: ReviewsFilterOptions) => {
    console.log('Filter changed:', filters);
    setCurrentPage(1);
  };

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
    window.scrollTo({ top: 300, behavior: 'smooth' });
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />

      <ReviewsBanner />

      <ReviewsStats stats={statsData} />

      <ReviewsFilter onFilterChange={handleFilterChange} />

      <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {isLoading ? (
          <div className="flex justify-center items-center py-20">
            <div className="animate-spin rounded-full h-8 w-8 border-t-2 border-b-2 border-primary"></div>
          </div>
        ) : reviews.length === 0 ? (
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.2 }}
            className="bg-white rounded-lg shadow-sm border border-gray-200 p-8 text-center"
          >
            <div className="text-gray-400 mb-4">
              <StarIcon className="h-16 w-16 mx-auto text-gray-300" />
            </div>
            <h3 className="text-lg font-medium text-gray-900 mb-2">Ainda não há avaliações</h3>
            <p className="text-gray-600">
              Você ainda não avaliou nenhum produto. Suas avaliações aparecerão aqui.
            </p>
          </motion.div>
        ) : (
          <div className="space-y-4">
            {paginatedReviews.map((review, i) => (
              <motion.div
                key={review.id}
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.3, delay: i * 0.1 }}
                className="bg-white rounded-lg shadow-sm border border-gray-200 p-6 flex flex-col md:flex-row gap-6"
              >
                {/* Product Image / Info placeholder */}
                <div className="md:w-1/4 flex flex-col items-center text-center gap-2">
                  <div className="w-24 h-24 bg-gray-100 rounded-lg overflow-hidden flex-shrink-0">
                    {review.productImage ? (
                      <img src={review.productImage} alt={review.productName} className="w-full h-full object-cover" />
                    ) : (
                      <div className="w-full h-full flex justify-center items-center text-gray-400 text-xs">Sem foto</div>
                    )}
                  </div>
                  <Link href={`/loja/${review.productId}`} className="text-sm font-medium text-gray-900 hover:text-primary transition-colors">
                    {review.productName}
                  </Link>
                </div>

                {/* Review Content */}
                <div className="md:w-3/4 flex flex-col">
                  <div className="flex justify-between items-start mb-2">
                    <div className="flex items-center gap-1">
                      {[...Array(5)].map((_, i) => (
                        <StarIcon
                          key={i}
                          className={`h-5 w-5 ${i < review.rating ? 'text-yellow-400' : 'text-gray-200'
                            }`}
                        />
                      ))}
                    </div>
                    <span className="text-xs text-gray-500">
                      {review.createdAt ? new Date(review.createdAt).toLocaleDateString() : 'N/A'}
                    </span>
                  </div>
                  <h4 className="font-semibold text-gray-900 mb-2">{review.title}</h4>
                  <p className="text-gray-600 text-sm whitespace-pre-wrap">{review.comment}</p>
                </div>
              </motion.div>
            ))}
          </div>
        )}
      </div>

      {totalPages > 1 && (
        <ReviewsPagination
          currentPage={currentPage}
          totalPages={totalPages}
          onPageChange={handlePageChange}
        />
      )}

      <Footer />
    </div>
  );
}