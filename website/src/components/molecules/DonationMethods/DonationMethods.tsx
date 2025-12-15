"use client";

import { motion } from 'framer-motion';
import { Card, Typography, Button } from '@/components/atoms';

interface DonationMethodsProps {
  className?: string;
}

export const DonationMethods = ({ className = '' }: DonationMethodsProps) => {
  const handleDonationClick = (method: string) => {
    // In a real app, this would redirect to payment processing
    console.log(`Donation via ${method} clicked`);
    // For now, just show an alert
    alert(`Redirecionando para doação via ${method}`);
  };

  return (
    <motion.div
      className={className}
      initial={{ opacity: 0, x: 50 }}
      animate={{ opacity: 1, x: 0 }}
      transition={{ duration: 0.8, delay: 0.2 }}
    >
      <Card className="p-8 bg-gray-50 text-center shadow-xl w-fit p-12">
        {/* QR Code */}
        <div className="mb-6">
          <div className="w-32 h-32 mx-auto bg-white border-2 border-gray-300 rounded-lg flex items-center justify-center mb-4">
            <svg className="w-20 h-20 text-primary" fill="currentColor" viewBox="0 0 24 24">
              <path d="M3 11h8V3H3v8zm2-6h4v4H5V5zM3 21h8v-8H3v8zm2-6h4v4H5v-4zM13 3v8h8V3h-8zm6 6h-4V5h4v4zM19 13h2v2h-2zM13 13h2v2h-2zM15 15h2v2h-2zM13 17h2v2h-2zM15 19h2v2h-2zM17 17h2v2h-2zM17 19h2v2h-2zM19 17h2v2h-2zM19 19h2v2h-2z"/>
            </svg>
          </div>
          <Typography variant="caption" color="gray" align="center">
            Escaneie o QR Code para doar
          </Typography>
        </div>

        {/* Payment Methods */}
        <div className="flex gap-4 justify-center">
        
          <Button
            variant="primary"
            className="text-sm"
            onClick={() => handleDonationClick('PayPal')}
          >
            Pix
          </Button>
          
          <Button
            variant="secondary"
            className="text-sm"
            onClick={() => handleDonationClick('Credit Card')}
          >
            Transferência Bancária
          </Button>
          
        </div>
      </Card>
    </motion.div>
  );
}; 