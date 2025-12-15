"use client";

import { motion } from 'framer-motion';
import { Card, Typography } from '@/components/atoms';

interface FeatureCardProps {
  icon: React.ReactNode;
  title: string;
  description: string;
  className?: string;
}

export const FeatureCard = ({ icon, title, description, className = '' }: FeatureCardProps) => {
  return (
    <motion.div
      className={className}
      initial={{ opacity: 0, y: 30 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.6, delay: 0.2 }}
    >
      <Card className="text-center h-full p-8 hover:shadow-xl transition-all duration-500">
        {/* Icon Container */}
        <div className="flex justify-center mb-6">
          <div className="w-20 h-20 bg-primary rounded-full flex items-center justify-center text-white">
            {icon}
          </div>
        </div>
        
        {/* Title */}
        <Typography variant="h3" className="mb-4 text-primary font-bold" align="center">
          {title}
        </Typography>
        
        {/* Description */}
        <Typography variant="body" color="gray" className="leading-relaxed" align="center">
          {description}
        </Typography>
      </Card>
    </motion.div>
  );
}; 