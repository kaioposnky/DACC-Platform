"use client";

import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';

interface Feature {
  icon: React.ReactNode;
  title: string;
  emoji: string;
}

interface ShopFeaturesProps {
  className?: string;
}

export const ShopFeatures = ({ className = '' }: ShopFeaturesProps) => {
  const features: Feature[] = [
    {
      emoji: "🚚",
      title: "Free Campus Delivery",
      icon: (
        <svg className="w-6 h-6" fill="currentColor" viewBox="0 0 24 24">
          <path d="M19 7c0-1.1-.9-2-2-2h-3V4c0-1.1-.9-2-2-2h-4c-1.1 0-2 .9-2 2v1H3c-1.1 0-2 .9-2 2v11h2c0 1.66 1.34 3 3 3s3-1.34 3-3h6c0 1.66 1.34 3 3 3s3-1.34 3-3h2V7zM8 4h4v1H8V4zM6 19c-.55 0-1-.45-1-1s.45-1 1-1 1 .45 1 1-.45 1-1 1zm12 0c-.55 0-1-.45-1-1s.45-1 1-1 1 .45 1 1-.45 1-1 1z"/>
        </svg>
      )
    },
    {
      emoji: "🏆",
      title: "Quality Guaranteed",
      icon: (
        <svg className="w-6 h-6" fill="currentColor" viewBox="0 0 24 24">
          <path d="M12 2l3.09 6.26L22 9l-5 4.74L18.18 21L12 17.27L5.82 21L7 13.74L2 9l6.91-1.26L12 2z"/>
        </svg>
      )
    },
    {
      emoji: "💛",
      title: "Made with Love",
      icon: (
        <svg className="w-6 h-6" fill="currentColor" viewBox="0 0 24 24">
          <path d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z"/>
        </svg>
      )
    }
  ];

  return (
    <div className={`bg-primary py-8 ${className}`}>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-8 lg:gap-12">
          {features.map((feature, index) => (
            <motion.div
              key={feature.title}
              className="flex items-center justify-center text-center"
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.6, delay: index * 0.2 }}
            >
              <div className="flex items-center gap-3">
                {/* Emoji */}
                <span className="text-2xl" role="img" aria-label={feature.title}>
                  {feature.emoji}
                </span>
                
                {/* Feature Title */}
                <Typography 
                  variant="body" 
                  color="white"
                  className="font-semibold text-lg"
                >
                  {feature.title}
                </Typography>
              </div>
            </motion.div>
          ))}
        </div>
      </div>
    </div>
  );
}; 