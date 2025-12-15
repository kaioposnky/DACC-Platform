"use client";

import { motion } from 'framer-motion';

interface DotProps {
  active?: boolean;
  onClick?: () => void;
  className?: string;
}

export const Dot = ({ active = false, onClick, className = '' }: DotProps) => {
  return (
    <motion.button
      className={`w-3 h-3 rounded-full transition-all duration-300 ${
        active 
          ? 'bg-secondary' 
          : 'bg-white/50 hover:bg-white/70'
      } ${className}`}
      onClick={onClick}
      whileHover={{ scale: 1.2 }}
      whileTap={{ scale: 0.9 }}
    />
  );
}; 