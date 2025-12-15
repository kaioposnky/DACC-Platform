"use client";

import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { StatCard } from '@/components/molecules';
import { StatData } from '@/types';

interface StatsSectionProps {
  title: string;
  subtitle?: string;
  stats: StatData[];
  backgroundColor?: 'primary' | 'secondary' | 'dark' | 'blue' | 'gradient';
  showDecorations?: boolean;
  className?: string;
  titleColor?: 'white' | 'gray' | 'primary';
  subtitleColor?: 'white' | 'gray' | 'primary';
}

export const StatsSection = ({ 
  title,
  subtitle,
  stats,
  backgroundColor = 'primary',
  showDecorations = true,
  className = '',
  titleColor = 'white',
  subtitleColor = 'white'
}: StatsSectionProps) => {
  
  const getBackgroundClasses = () => {
    switch (backgroundColor) {
      case 'primary':
        return 'bg-primary';
      case 'secondary':
        return 'bg-secondary';
      case 'dark':
        return 'bg-slate-800';
      case 'blue':
        return 'bg-blue-900';
      case 'gradient':
        return 'bg-gradient-to-br from-blue-900 via-blue-800 to-blue-700';
      default:
        return 'bg-primary';
    }
  };

  const getDecorationIcons = () => ({
    leftIcon: (
      <svg className="w-16 h-16 text-white/10" fill="currentColor" viewBox="0 0 24 24">
        <path d="M8.5 7l3-3 3 3M12 4v16M8.5 17l3 3 3-3"/>
      </svg>
    ),
    rightIcon: (
      <svg className="w-16 h-16 text-white/10" fill="currentColor" viewBox="0 0 24 24">
        <path d="M4 7h16M4 12h16M4 17h16"/>
        <circle cx="12" cy="12" r="3" fill="none" stroke="currentColor"/>
      </svg>
    )
  });

  const decorations = getDecorationIcons();

  return (
    <section className={`relative py-16 lg:py-20 overflow-hidden ${getBackgroundClasses()} ${className}`}>
      {/* Decorative Icons */}
      {showDecorations && (
        <>
          <motion.div
            className="absolute left-8 top-1/2 transform -translate-y-1/2 hidden lg:block"
            initial={{ opacity: 0, x: -50 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 1, delay: 0.5 }}
          >
            {decorations.leftIcon}
          </motion.div>
          
          <motion.div
            className="absolute right-8 top-1/2 transform -translate-y-1/2 hidden lg:block"
            initial={{ opacity: 0, x: 50 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 1, delay: 0.7 }}
          >
            {decorations.rightIcon}
          </motion.div>
        </>
      )}

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 relative z-10">
        {/* Header */}
        <motion.div
          className="text-center mb-16"
          initial={{ opacity: 0, y: 30 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8 }}
        >
          <Typography 
            variant="h2" 
            color={titleColor}
            className="mb-6 font-bold text-3xl md:text-4xl lg:text-5xl"
            align="center"
          >
            {title}
          </Typography>
          {subtitle && (
            <Typography 
              variant="subtitle" 
              color={subtitleColor} 
              className="max-w-4xl mx-auto text-lg md:text-xl opacity-90"
              align="center"
            >
              {subtitle}
            </Typography>
          )}
        </motion.div>

        {/* Stats Grid */}
        <div className="flex gap-24 justify-center">
          {stats.map((stat, index) => (
            <StatCard
              key={stat.id}
              number={stat.number}
              label={stat.label}
              prefix={stat.prefix}
              suffix={stat.suffix}
              animateNumber={stat.animateNumber !== false}
              delay={index * 0.2}
            />
          ))}
        </div>
      </div>
    </section>
  );
}; 