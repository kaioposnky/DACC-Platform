"use client";

import { motion } from 'framer-motion';
import { Dot } from '@/components/atoms';

interface NavigationDotsProps {
  total: number;
  active: number;
  onDotClick: (index: number) => void;
  className?: string;
}

export const NavigationDots = ({ 
  total, 
  active, 
  onDotClick, 
  className = '' 
}: NavigationDotsProps) => {
  return (
    <motion.div 
      className={`flex items-center justify-center space-x-2 ${className}`}
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5, delay: 0.5 }}
    >
      {Array.from({ length: total }).map((_, index) => (
        <Dot
          key={index}
          active={index === active}
          onClick={() => onDotClick(index)}
        />
      ))}
    </motion.div>
  );
}; 