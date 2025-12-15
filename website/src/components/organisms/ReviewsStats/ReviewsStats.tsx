'use client';

import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { StarIcon, ChartBarIcon, HandThumbUpIcon } from '@heroicons/react/24/solid';

export interface ReviewsStatsData {
  totalReviews: number;
  averageRating: number;
  helpfulVotes: number;
}

interface ReviewsStatsProps {
  stats: ReviewsStatsData;
  className?: string;
}

export const ReviewsStats = ({ stats, className = '' }: ReviewsStatsProps) => {
  
  const statsItems = [
    {
      id: 'total-reviews',
      value: stats.totalReviews,
      label: 'Total de Avaliações',
      icon: <StarIcon className="w-8 h-8 text-white" />,
      bgColor: 'bg-blue-900'
    },
    {
      id: 'average-rating',
      value: stats.averageRating.toFixed(1),
      label: 'Classificação Média',
      icon: <ChartBarIcon className="w-8 h-8 text-white" />,
      bgColor: 'bg-blue-900'
    },
    {
      id: 'helpful-votes',
      value: stats.helpfulVotes,
      label: 'Votos Úteis',
      icon: <HandThumbUpIcon className="w-8 h-8 text-white" />,
      bgColor: 'bg-blue-900'
    }
  ];

  return (
    <div className={`max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-8 ${className}`}>
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {statsItems.map((stat, index) => (
          <motion.div
            key={stat.id}
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: index * 0.1 }}
            className="bg-white rounded-lg shadow-sm border border-gray-200 p-6"
          >
            <div className="flex items-center gap-4">
              {/* Icon */}
              <div className={`w-16 h-16 ${stat.bgColor} rounded-full flex items-center justify-center flex-shrink-0`}>
                {stat.icon}
              </div>
              
              {/* Content */}
              <div className="flex-1">
                <Typography 
                  variant="h2" 
                  className="text-gray-900 font-bold text-3xl mb-1"
                >
                  {stat.value}
                </Typography>
                <Typography 
                  variant="body" 
                  className="text-gray-600"
                >
                  {stat.label}
                </Typography>
              </div>
            </div>
          </motion.div>
        ))}
      </div>
    </div>
  );
}; 