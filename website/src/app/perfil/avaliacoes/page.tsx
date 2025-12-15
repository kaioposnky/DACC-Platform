'use client';

import { useState } from 'react';
import { Footer, Navigation } from "@/components";
import { ReviewsBanner } from "@/components/organisms/ReviewsBanner";
import { ReviewsStats, ReviewsStatsData } from "@/components/organisms/ReviewsStats";
import { ReviewsFilter, ReviewsFilterOptions } from "@/components/organisms/ReviewsFilter";
import { ReviewsPagination } from "@/components/organisms/ReviewsPagination";

export default function ReviewsPage() {
  // Mock stats data - in a real app, this would come from API
  const statsData: ReviewsStatsData = {
    totalReviews: 23,
    averageRating: 4.2,
    helpfulVotes: 156
  };

  // Pagination state
  const [currentPage, setCurrentPage] = useState(1);
  const totalPages = 3;

  const handleFilterChange = (filters: ReviewsFilterOptions) => {
    console.log('Filter changed:', filters);
    // Here you would typically fetch filtered reviews from API
  };

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
    console.log('Page changed to:', page);
    // Here you would typically fetch reviews for the new page
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />
      
      <ReviewsBanner />
      
      <ReviewsStats stats={statsData} />
      
      <ReviewsFilter onFilterChange={handleFilterChange} />
      
      {/* TODO: Add ReviewsList component here to display the actual reviews */}
      <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-8 text-center">
          <p className="text-gray-600">Lista de avaliações será implementada aqui...</p>
        </div>
      </div>
      
      <ReviewsPagination 
        currentPage={currentPage}
        totalPages={totalPages}
        onPageChange={handlePageChange}
      />
      
      <Footer />
    </div>
  );
}