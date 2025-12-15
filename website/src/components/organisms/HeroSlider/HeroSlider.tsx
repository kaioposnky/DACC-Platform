"use client";

import { useState } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { Icon } from '@/components/atoms';
import { HeroContent, CodeSnippet, NavigationDots, EventImage } from '@/components/molecules';

interface EventDetail {
  icon: 'calendar' | 'users' | 'clock' | 'location';
  text: string;
}

interface Slide {
  content: string;
  id: number;
  title: string;
  type: 'code' | 'event' | 'default';
  // Event-specific props
  icon?: string;
  details?: EventDetail[];
  primaryButtonText?: string;
  secondaryButtonText?: string;
  imageSrc?: string;
  imageAlt?: string;
  primaryButtonLink?: string;
  secondaryButtonLink?: string;
}

interface HeroSliderProps {
  slides: Slide[];
  autoPlay?: boolean;
  interval?: number;
  className?: string;
}

export const HeroSlider = ({ 
  slides, 
  className = '' 
}: HeroSliderProps) => {
  const [currentSlide, setCurrentSlide] = useState(0);

  const nextSlide = () => {
    setCurrentSlide((prev) => (prev + 1) % slides.length);
  };

  const prevSlide = () => {
    setCurrentSlide((prev) => (prev - 1 + slides.length) % slides.length);
  };

  const goToSlide = (index: number) => {
    setCurrentSlide(index);
  };

  const currentSlideData = slides[currentSlide];

  const renderRightContent = () => {
    switch (currentSlideData.type) {
      case 'code':
        return (
          <div className="flex items-center justify-center lg:justify-end">
            <CodeSnippet className="max-w-md" />
          </div>
        );
      case 'event':
        return currentSlideData.imageSrc ? (
          <div className="flex items-center justify-center lg:justify-end">
            <EventImage 
              src={currentSlideData.imageSrc} 
              alt={currentSlideData.imageAlt || 'Event image'}
              className="max-w-md"
            />
          </div>
        ) : null;
      default:
        return null;
    }
  };

  return (
    <div className={`relative min-h-screen bg-primary overflow-hidden ${className}`}>
      
      <div className="relative z-10 container mx-auto px-4 sm:px-6 lg:px-8 h-screen flex items-center">
        <div className="max-w-6xl mx-auto grid grid-cols-1 lg:grid-cols-2 gap-8 lg:gap-12 w-full">
          {/* Left side - Hero content */}
          <div className="flex items-center justify-center lg:justify-start">
            <AnimatePresence mode="wait">
              <motion.div
                key={currentSlide}
                initial={{ opacity: 0, x: -50 }}
                animate={{ opacity: 1, x: 0 }}
                exit={{ opacity: 0, x: 50 }}
                transition={{ duration: 0.5 }}
                className="max-w-2xl"
              >
                <HeroContent
                  title={currentSlideData.title}
                  subtitle={currentSlideData.content}
                  icon={currentSlideData.icon}
                  isEvent={currentSlideData.type === 'event'}
                  eventDetails={currentSlideData.details}
                  primaryButtonText={currentSlideData.primaryButtonText}
                  secondaryButtonText={currentSlideData.secondaryButtonText}
                  primaryButtonLink={currentSlideData.primaryButtonLink}
                  secondaryButtonLink={currentSlideData.secondaryButtonLink}
                  onLearnMoreClick={() => console.log('Learn more clicked')}
                  onViewProjectsClick={() => console.log('View projects clicked')}
                />
              </motion.div>
            </AnimatePresence>
          </div>

          {/* Right side - Dynamic content */}
          <AnimatePresence mode="wait">
            <motion.div
              key={`${currentSlide}-content`}
              initial={{ opacity: 0, x: 50 }}
              animate={{ opacity: 1, x: 0 }}
              exit={{ opacity: 0, x: -50 }}
              transition={{ duration: 0.5 }}
            >
              {renderRightContent()}
            </motion.div>
          </AnimatePresence>
        </div>
      </div>

      {/* Navigation arrows */}
      <motion.button
        className="absolute left-4 top-1/2 transform -translate-y-1/2 z-20 bg-white/10 hover:bg-white/20 rounded-full p-3 transition-colors duration-200"
        onClick={prevSlide}
        whileHover={{ scale: 1.1 }}
        whileTap={{ scale: 0.9 }}
      >
        <Icon name="chevron-left" color="white" size="lg" />
      </motion.button>

      <motion.button
        className="absolute right-4 top-1/2 transform -translate-y-1/2 z-20 bg-white/10 hover:bg-white/20 rounded-full p-3 transition-colors duration-200"
        onClick={nextSlide}
        whileHover={{ scale: 1.1 }}
        whileTap={{ scale: 0.9 }}
      >
        <Icon name="chevron-right" color="white" size="lg" />
      </motion.button>

      {/* Navigation dots */}
      <div className="absolute bottom-8 left-1/2 transform -translate-x-1/2 z-20">
        <NavigationDots
          total={slides.length}
          active={currentSlide}
          onDotClick={goToSlide}
        />
      </div>
    </div>
  );
}; 