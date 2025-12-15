'use client';

import { motion } from 'framer-motion';

interface Step {
  id: number;
  label: string;
  status: 'completed' | 'current' | 'upcoming';
}

interface StepIndicatorProps {
  steps: Step[];
  className?: string;
}

export const StepIndicator = ({ steps, className = '' }: StepIndicatorProps) => {
  return (
    <div className={`flex items-center justify-center ${className}`}>
      {steps.map((step, index) => (
        <div key={step.id} className="flex items-center">
          {/* Step Circle */}
          <motion.div
            initial={{ scale: 0.8, opacity: 0 }}
            animate={{ scale: 1, opacity: 1 }}
            transition={{ duration: 0.3, delay: index * 0.1 }}
            className="flex flex-col items-center"
          >
            {/* Circle with number */}
            <div
              className={`
                w-10 h-10 rounded-full flex items-center justify-center font-bold text-sm
                ${step.status === 'completed' 
                  ? 'bg-yellow-400 text-black' 
                  : step.status === 'current' 
                    ? 'bg-white text-blue-900' 
                    : 'bg-gray-500 text-white'
                }
              `}
            >
              {step.id}
            </div>
            
            {/* Step Label */}
            <span
              className={`
                mt-2 text-sm font-medium
                ${step.status === 'current' 
                  ? 'text-white' 
                  : 'text-gray-300'
                }
              `}
            >
              {step.label}
            </span>
          </motion.div>

          {/* Connecting Line */}
          {index < steps.length - 1 && (
            <motion.div
              initial={{ scaleX: 0 }}
              animate={{ scaleX: 1 }}
              transition={{ duration: 0.5, delay: index * 0.1 + 0.2 }}
              className={`
                w-16 h-0.5 mx-4 mb-6
                ${step.status === 'completed' 
                  ? 'bg-yellow-400' 
                  : 'bg-gray-500'
                }
              `}
            />
          )}
        </div>
      ))}
    </div>
  );
}; 