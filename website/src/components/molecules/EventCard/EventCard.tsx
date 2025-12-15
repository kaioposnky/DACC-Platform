"use client";

import { motion } from 'framer-motion';
import { Card, Typography, Button } from '@/components/atoms';
import { Event } from '@/types';

interface EventCardProps {
  event: Event;
  className?: string;
}

export const EventCard = ({ event, className = '' }: EventCardProps) => {
  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    const day = date.getDate();
    const month = date.toLocaleDateString('en-US', { month: 'short' }).toUpperCase();
    return { day, month };
  };

  const { day, month } = formatDate(event.date);

  return (
    <motion.div
      className={className}
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
    >
      <Card className="p-6 h-full flex items-center gap-6">
        {/* Date Display */}
        <div className="flex-shrink-0">
          <div className="text-center">
            <div className="text-3xl font-bold text-primary">
              {day}
            </div>
            <div className="text-sm font-medium text-gray-600">
              {month}
            </div>
          </div>
        </div>

        {/* Event Content */}
        <div className="flex-1">
          <Typography variant="h4" className="mb-2 text-primary font-bold !text-base">
            {event.title}
          </Typography>
          <Typography variant="body" color="gray" className="mb-3 line-clamp-2">
            {event.description}
          </Typography>
          <div className="flex items-center text-gray-600 text-xs">
            <svg className="w-4 h-4 mr-2" fill="currentColor" stroke="white" viewBox="0 0 20 20">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
            {event.time}
          </div>
        </div>

        {/* Action Button */}
        <div className="flex-shrink-0">
          <Button
            variant="ghost"
            className="text-secondary"
            size="sm"
            onClick={() => window.open(event.actionLink, '_blank')}
          >
            {event.actionText}
          </Button>
        </div>
      </Card>
    </motion.div>
  );
}; 