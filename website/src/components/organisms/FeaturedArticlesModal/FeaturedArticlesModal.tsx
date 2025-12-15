"use client";

import { Typography } from '@/components/atoms';
import { Modal, NewsletterSignup } from '@/components/molecules';

interface FeaturedArticlesModalProps {
  isOpen: boolean;
  onClose: () => void;
  onNewsletterSubmit?: (email: string) => void;
}

export const FeaturedArticlesModal = ({
  isOpen,
  onClose,
  onNewsletterSubmit
}: FeaturedArticlesModalProps) => {

  const handleNewsletterSubmit = async (email: string) => {
    // Here you would typically send the email to your newsletter service
    console.log('Newsletter subscription:', email);
    
    if (onNewsletterSubmit) {
      await onNewsletterSubmit(email);
    }
    
    // You might want to close the modal after successful subscription
    // setTimeout(() => onClose(), 2000);
  };

  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      className="max-w-lg"
      showCloseButton={true}
      closeOnBackdropClick={true}
    >
      <div className="pt-2">
        {/* Modal Title */}
        <div className="mb-8">
          <Typography variant="h2" className="text-gray-900 font-bold text-center">
            Artigos em Destaque
          </Typography>
        </div>

        {/* Newsletter Signup */}
        <NewsletterSignup 
          onSubmit={handleNewsletterSubmit}
          className="w-full"
        />
      </div>
    </Modal>
  );
}; 