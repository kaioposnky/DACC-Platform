"use client";

import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { DonationMethods } from '@/components/molecules';

interface SupportSectionProps {
  className?: string;
}

export const SupportSection = ({ className = '' }: SupportSectionProps) => {
  const supportFeatures = [
    "Novos projetos e iniciativas",
    "Equipamentos e software atualizados",
    "Eventos e workshops com palestrantes convidados"
  ];

  return (
    <section id='apoie' className={`py-16 bg-blue-50 ${className}`}>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between">
          {/* Left Side - Content */}
          <motion.div
            initial={{ opacity: 0, x: -50 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.8 }}
            className="w-1/2"
          >
            <Typography variant="h1" className="mb-6 text-primary font-bold">
              Apoie o DACC
            </Typography>
            
            <Typography variant="body" color="gray" className="mb-8 leading-relaxed">
              Ajude-nos a continuar fornecendo qualidade dos nossos projetos e recursos para os alunos de computação.
              Seu apoio faz toda a diferença!
            </Typography>

            {/* Features List */}
            <div className="space-y-4">
              {supportFeatures.map((feature, index) => (
                <motion.div
                  key={index}
                  className="flex items-start gap-3"
                  initial={{ opacity: 0, x: -20 }}
                  animate={{ opacity: 1, x: 0 }}
                  transition={{ duration: 0.5, delay: 0.2 + index * 0.1 }}
                >
                  <div className="flex-shrink-0 mt-1">
                    <svg className="w-5 h-5 text-green-500" fill="currentColor" viewBox="0 0 20 20">
                      <path 
                        fillRule="evenodd" 
                        d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" 
                        clipRule="evenodd" 
                      />
                    </svg>
                  </div>
                  <Typography variant="body" color="gray" className="flex-1">
                    {feature}
                  </Typography>
                </motion.div>
              ))}
            </div>
          </motion.div>

          {/* Right Side - Donation Methods */}
          <DonationMethods />
        </div>
      </div>
    </section>
  );
}; 