"use client";

import { motion, AnimatePresence } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { NewsletterSignup } from '@/components/molecules';

interface NewsletterWidgetProps {
  isVisible: boolean;
  onClose: () => void;
  onNewsletterSubmit?: (email: string) => void;
}

export const NewsletterWidget = ({
  isVisible,
  onClose,
  onNewsletterSubmit
}: NewsletterWidgetProps) => {

  const handleNewsletterSubmit = async (email: string) => {
    console.log('Newsletter subscription:', email);

    if (onNewsletterSubmit) {
      await onNewsletterSubmit(email);
    }
  };

  return (
    <AnimatePresence>
      {isVisible && (
        <motion.div
          className="fixed top-1/2 right-6 z-40 transform -translate-y-1/2"
          initial={{ opacity: 0, x: 100, scale: 0.9 }}
          animate={{ opacity: 1, x: 0, scale: 1 }}
          exit={{ opacity: 0, x: 100, scale: 0.9 }}
          transition={{ duration: 0.4, ease: "easeOut" }}
        >
          <div className="bg-white rounded-2xl shadow-2xl border border-gray-200 w-80 max-w-[calc(100vw-3rem)]">
            {/* Header */}
            <div className="relative p-6 pb-4">
              {/* Close Button */}
              <button
                onClick={onClose}
                className="absolute top-4 right-4 p-2 rounded-full bg-gray-100 hover:bg-gray-200 text-gray-600 hover:text-gray-800 transition-colors duration-200"
                aria-label="Close widget"
              >
                <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                </svg>
              </button>

              {/* Widget Title */}
              <Typography variant="h4" className="text-primary font-bold pr-8">
                Novidades em primeira mão
              </Typography>
              <hr className="my-4 h-0.5 border-t-0 bg-gray-200" />

            </div>

            {/* Newsletter Signup */}
            <div className="px-6 pb-6">
              <NewsletterSignup
                onSubmit={handleNewsletterSubmit}
                className="w-full"
              />
            </div>
          </div>

          {/* Optional decorative element */}
          <div className="absolute -top-2 -right-2 w-4 h-4 bg-primary rounded-full animate-pulse"></div>
        </motion.div>
      )}
    </AnimatePresence>
  );
}; 