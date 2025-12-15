"use client";

import { motion } from 'framer-motion';
import Image from 'next/image';
import { Card } from '@/components/atoms';

interface EventImageProps {
  src: string;
  alt: string;
  className?: string;
}

export const EventImage = ({ src, alt, className = '' }: EventImageProps) => {
  console.log('src', src);
  return (
    <motion.div
      className={className}
      initial={{ opacity: 0, x: 50 }}
      animate={{ opacity: 1, x: 0 }}
      transition={{ duration: 0.8, delay: 0.3 }}
    >
      <Card className="relative overflow-hidden border-gray-700 shadow-2xl h-96 w-lg">
        <Image 
            src={src} 
            alt={alt}
            fill
            className="object-cover"
            sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
          />
      </Card>
    </motion.div>
  );
}; 