'use client';

import { motion } from 'framer-motion';
import { StepIndicator } from '@/components/molecules/StepIndicator';
import { Typography } from '@/components/atoms';

interface CheckoutBannerProps {
  currentStep?: number;
  className?: string;
}

export const CheckoutBanner = ({ currentStep = 2, className = '' }: CheckoutBannerProps) => {
  // Define the checkout steps
  const steps = [
    { id: 1, label: 'Carrinho', status: 'completed' as const },
    { id: 2, label: 'Checkout', status: 'current' as const },
    { id: 3, label: 'Pagamento', status: 'upcoming' as const },
    { id: 4, label: 'Confirmação', status: 'upcoming' as const },
  ];

  // Update step statuses based on current step
  const updatedSteps = steps.map(step => {
    if (step.id < currentStep) {
      return { ...step, status: 'completed' as const };
    } else if (step.id === currentStep) {
      return { ...step, status: 'current' as const };
    } else {
      return { ...step, status: 'upcoming' as const };
    }
  });

  return (
    <section className={`relative py-16 bg-primary overflow-hidden ${className}`}>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 relative z-10">
        {/* Step Indicator */}
        <motion.div
          initial={{ opacity: 0, y: 30 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8 }}
          className="mb-12"
        >
          <StepIndicator steps={updatedSteps} />
        </motion.div>

        {/* Content */}
        <div className="text-center">
          {/* Title */}
          <motion.div
            initial={{ opacity: 0, y: 30 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.2 }}
            className="mb-4"
          >
            <Typography 
              variant="h1" 
              color="white"
              className="font-bold !text-4xl md:text-5xl lg:text-6xl"
              align="center"
            >
              Checkout
            </Typography>
          </motion.div>

          {/* Subtitle */}
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.4 }}
          >
            <Typography 
              variant="subtitle" 
              color="white"
              className="max-w-4xl mx-auto !text-lg opacity-40"
              align="center"
            >
              Revise seus dados e finalize sua compra
            </Typography>
          </motion.div>
        </div>
      </div>
    </section>
  );
}; 