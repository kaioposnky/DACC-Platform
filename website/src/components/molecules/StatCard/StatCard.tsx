"use client";

import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { useEffect, useState } from 'react';
import React from 'react';

interface StatCardProps {
  // Common
  label: string;
  className?: string;

  // Animated Variant (Landing)
  number?: string;
  prefix?: string;
  suffix?: string;
  animateNumber?: boolean;
  delay?: number;

  // Admin Variant (Dashboard)
  value?: string | number;
  icon?: any;
  trend?: string;
  colorClass?: string;
  iconColorClass?: string;
}

export const StatCard = (props: StatCardProps) => {
  const {
    label,
    className = "",
    number,
    prefix = "",
    suffix = "",
    animateNumber = true,
    delay = 0,
    value,
    icon: Icon,
    trend,
    colorClass = "bg-blue-600",
    iconColorClass = "text-blue-600",
  } = props;

  // Admin / Dashboard Variant
  if (Icon || value !== undefined) {
    const displayValue = value ?? number;
    return (
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        className={`bg-white rounded-xl p-6 shadow-sm border border-gray-100 hover:shadow-md transition-shadow ${className}`}
      >
        <div className="flex items-center justify-between">
          <div className="flex-1">
            <p className="text-sm font-medium text-gray-500 mb-1">{label}</p>
            <p className="text-3xl font-bold text-gray-900">{displayValue}</p>
            {trend && <p className="text-xs text-gray-500 mt-2">{trend}</p>}
          </div>
          {Icon && (
            <div className={`p-3 rounded-lg ${colorClass} bg-opacity-10 flex items-center justify-center`}>
              <Icon className={`w-8 h-8 ${iconColorClass}`} />
            </div>
          )}
        </div>
      </motion.div>
    );
  }

  const [displayNumber, setDisplayNumber] = useState(animateNumber ? '0' : (number || '0'));

  useEffect(() => {
    if (!animateNumber || !number) return;

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
