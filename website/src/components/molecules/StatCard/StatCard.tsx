"use client";

import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { useEffect, useState } from 'react';

interface StatCardProps {
  number: string;
  label: string;
  prefix?: string;
  suffix?: string;
  animateNumber?: boolean;
  delay?: number;
  className?: string;
}

export const StatCard = ({ 
  number, 
  label, 
  prefix = '', 
  suffix = '', 
  animateNumber = true,
  delay = 0,
  className = '' 
}: StatCardProps) => {
  const [displayNumber, setDisplayNumber] = useState(animateNumber ? '0' : number);

  useEffect(() => {
    if (!animateNumber) return;

    const isNumeric = /^\d+$/.test(number);
    if (!isNumeric) {
      setDisplayNumber(number);
      return;
    }

    const targetNumber = parseInt(number);
    const duration = 2000; // 2 seconds
    const startTime = Date.now() + (delay * 1000);
    
    const animate = () => {
      const currentTime = Date.now();
      if (currentTime < startTime) {
        requestAnimationFrame(animate);
        return;
      }

      const elapsed = currentTime - startTime;
      const progress = Math.min(elapsed / duration, 1);
      
      // Easing function for smooth animation
      const easeOutQuart = 1 - Math.pow(1 - progress, 4);
      const current = Math.floor(targetNumber * easeOutQuart);
      
      setDisplayNumber(current.toString());
      
      if (progress < 1) {
        requestAnimationFrame(animate);
      }
    };

    requestAnimationFrame(animate);
  }, [number, animateNumber, delay]);

  return (
    <motion.div
      className={`text-center ${className}`}
      initial={{ opacity: 0, y: 30 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.6, delay }}
    >
      <motion.div
        className="mb-4"
        initial={{ scale: 0.8 }}
        animate={{ scale: 1 }}
        transition={{ duration: 0.8, delay: delay + 0.2 }}
      >
        <Typography 
          variant="h2" 
          className="!text-4xl md:text-5xl lg:text-6xl font-bold text-yellow-400"
          align="center"
        >
          {prefix}{displayNumber}{suffix}
        </Typography>
      </motion.div>
      
      <motion.div
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ duration: 0.6, delay: delay + 0.4 }}
      >
        <Typography 
          variant="body" 
          className="text-secondary text-sm! md:text-base lg:text-lg font-medium tracking-wider uppercase"
          align="center"
        >
          {label}
        </Typography>
      </motion.div>
    </motion.div>
  );
}; 