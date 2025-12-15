"use client";

import { motion } from 'framer-motion';
import { Button } from '@/components/atoms';
import Link from 'next/link';

interface ButtonGroupProps {
  className?: string;
  onLearnMoreClick?: () => void;
  onViewProjectsClick?: () => void;
  primaryButtonText?: string;
  secondaryButtonText?: string;
  secondaryButtonLink?: string;
  primaryButtonLink?: string;
  }

export const ButtonGroup = ({
  className = '',
  onLearnMoreClick,
  onViewProjectsClick,
  primaryButtonText,
  secondaryButtonText,
  secondaryButtonLink = '',
  primaryButtonLink = ''
}: ButtonGroupProps) => {
  const containerVariants = {
    hidden: { opacity: 0 },
    visible: {
      opacity: 1,
      transition: {
        staggerChildren: 0.2,
        delayChildren: 0.4
      }
    }
  };

  const buttonVariants = {
    hidden: { opacity: 0, y: 20 },
    visible: { opacity: 1, y: 0 }
  };

  return (
    <motion.div
      className={`flex flex-col sm:flex-row gap-4 ${className}`}
      variants={containerVariants}
      initial="hidden"
      animate="visible"
    >
      <motion.div variants={buttonVariants}>
        <Link href={primaryButtonLink}>
          <Button
            variant="hero-primary"
            size="lg"
            onClick={onLearnMoreClick}
            className="w-full sm:w-auto"
          >
            {primaryButtonText}
          </Button>
        </Link>
      </motion.div>

      {secondaryButtonText && (
        <motion.div variants={buttonVariants}>
          <Link href={secondaryButtonLink}>
            <Button
              variant="hero-outline"
              size="lg"
              onClick={onViewProjectsClick}
              className="w-full sm:w-auto"
            >
              {secondaryButtonText}
            </Button>
          </Link>
        </motion.div>
      )}
    </motion.div>
  );
}; 