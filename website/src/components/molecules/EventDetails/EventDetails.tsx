"use client";

import { motion } from 'framer-motion';

interface EventDetail {
  icon: 'calendar' | 'users' | 'clock' | 'location';
  text: string;
}

interface EventDetailsProps {
  eventDetails: EventDetail[];
  className?: string;
}

export const EventDetails = ({ eventDetails, className = '' }: EventDetailsProps) => {
  const iconMap = {
    calendar: (
      <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
      </svg>
    ),
    users: (
      <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-2.25" />
      </svg>
    ),
    clock: (
      <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
      </svg>
    ),
    location: (
      <svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 21l-12-18h24z" />
      </svg>
    ),
  };

  return (
    <motion.div 
      className={`space-y-4 ${className}`}
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5, delay: 0.3 }}
    >
        {eventDetails.map((detail, index) => (
        <motion.div 
          key={index}
          className="flex items-center space-x-3 text-white/90"
          initial={{ opacity: 0, x: -20 }}
          animate={{ opacity: 1, x: 0 }}
          transition={{ duration: 0.4, delay: 0.4 + index * 0.1 }}
        >
          <div className="text-secondary">
            {iconMap[detail.icon]}
          </div>
          <span className="text-sm font-medium">{detail.text}</span>
        </motion.div>
      ))}
    </motion.div>
  );
}; 