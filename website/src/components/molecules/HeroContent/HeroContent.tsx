"use client";

import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { ButtonGroup } from '../ButtonGroup';
import { EventDetails } from '../EventDetails';

interface EventDetail {
  icon: 'calendar' | 'users' | 'clock' | 'location';
  text: string;
}

interface HeroContentProps {
  title: string;
  subtitle: string;
  className?: string;
  onLearnMoreClick?: () => void;
  onViewProjectsClick?: () => void;
  // Event-specific props
  icon?: React.ReactNode;
  isEvent?: boolean;
  eventDetails?: EventDetail[];
  primaryButtonText?: string;
  secondaryButtonText?: string;
  primaryButtonLink?: string;
  secondaryButtonLink?: string;
}

export const HeroContent = ({ 
  title, 
  subtitle, 
  className = '',
  onLearnMoreClick,
  onViewProjectsClick,
  icon,
  isEvent = false,
  eventDetails = [],
  primaryButtonText,
  secondaryButtonText,
  primaryButtonLink,
  secondaryButtonLink
}: HeroContentProps) => {
  const containerVariants = {
    hidden: { opacity: 0 },
    visible: {
      opacity: 1,
      transition: {
        staggerChildren: 0.3,
        delayChildren: 0.1
      }
    }
  };

  const itemVariants = {
    hidden: { opacity: 0, y: 30 },
    visible: { opacity: 1, y: 0 }
  };
  return (
    <motion.div 
      className={`space-y-6 ${className}`}
      variants={containerVariants}
      initial="hidden"
      animate="visible"
    >
      {/* Icon for event slides */}
      {icon && (
        <motion.div 
          variants={itemVariants}
          className="w-16 h-16 bg-secondary rounded-2xl flex items-center justify-center text-primary"
        >
          <svg className="w-8 h-8" fill="currentColor" viewBox="0 0 24 24">
            <path d={icon as string} />
          </svg>
        </motion.div>
      )}

      <motion.div variants={itemVariants}>
        <Typography 
          variant="h1" 
          color="white" 
          className="font-bold leading-tight"
        >
          {title}
        </Typography>
      </motion.div>
      
      <motion.div variants={itemVariants}>
        <Typography 
          variant="subtitle" 
          color="white" 
          className="opacity-90 max-w-lg"
        >
          {subtitle}
        </Typography>
      </motion.div>

      {/* Event details */}
      {isEvent && eventDetails.length > 0 && (
        <motion.div variants={itemVariants}>
          <EventDetails eventDetails={eventDetails} />
        </motion.div>
      )}
      
      <motion.div variants={itemVariants}>
        <ButtonGroup 
          onLearnMoreClick={onLearnMoreClick}
          onViewProjectsClick={onViewProjectsClick}
          primaryButtonText={primaryButtonText}
          secondaryButtonText={secondaryButtonText}
          primaryButtonLink={primaryButtonLink}
          secondaryButtonLink={secondaryButtonLink}
        />
      </motion.div>
    </motion.div>
  );
}; 