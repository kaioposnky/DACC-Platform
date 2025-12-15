"use client";

import { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { Typography, Icon } from '@/components/atoms';
import { EventCard } from '@/components/molecules';
import { Event } from '@/types';
import { apiService } from '@/services/api';

interface EventCalendarProps {
  className?: string;
}

export const EventCalendar = ({ className = '' }: EventCalendarProps) => {
  const [events, setEvents] = useState<Event[]>([]);
  const [loading, setLoading] = useState(true);
  const [currentMonth, setCurrentMonth] = useState(new Date());

  useEffect(() => {
    fetchEvents();
  }, []);

  const fetchEvents = async () => {
    try {
      setLoading(true);
      const fetchedEvents = await apiService.getEvents();
      setEvents(fetchedEvents);
    } catch (error) {
      console.error('Error fetching events:', error);
    } finally {
      setLoading(false);
    }
  };

  const formatMonth = (date: Date) => {
    return date.toLocaleDateString('en-US', { month: 'long', year: 'numeric' });
  };

  const navigateMonth = (direction: 'prev' | 'next') => {
    setCurrentMonth(prev => {
      const newDate = new Date(prev);
      if (direction === 'prev') {
        newDate.setMonth(newDate.getMonth() - 1);
      } else {
        newDate.setMonth(newDate.getMonth() + 1);
      }
      return newDate;
    });
  };

  if (loading) {
    return (
      <section className={`py-16 bg-white ${className}`}>
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center">
            <div className="animate-pulse">
              <div className="h-8 bg-gray-200 rounded w-64 mx-auto mb-4"></div>
              <div className="h-4 bg-gray-200 rounded w-96 mx-auto mb-8"></div>
            </div>
          </div>
        </div>
      </section>
    );
  }

  return (
    <section className={`py-16 bg-white ${className}`}>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Section Header */}
        <motion.div
          className="text-center mb-12"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6 }}
        >
          <Typography variant="h2" className="mb-4 text-primary font-bold" align="center">
            Próximos Eventos
          </Typography>
          <Typography variant="subtitle" color="gray" className="max-w-2xl mx-auto" align="center">
            Acompanhe os próximos eventos do DACC
          </Typography>
        </motion.div>
        <div className='shadow-lg rounded-lg'>
          {/* Calendar Header */}
          <motion.div
            className="bg-primary text-white rounded-t-lg p-6 mb-8"
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.6, delay: 0.2 }}
          >
            <div className="flex items-center justify-between">
              <Typography variant="h3" color="white" className="font-semibold">
                {formatMonth(currentMonth)}
              </Typography>
              <div className="flex gap-2">
                <button
                  onClick={() => navigateMonth('prev')}
                  className="p-2 rounded-full hover:bg-white/10 transition-colors"
                >
                  <Icon name="chevron-left" color="white" size="md" />
                </button>
                <button
                  onClick={() => navigateMonth('next')}
                  className="p-2 rounded-full hover:bg-white/10 transition-colors"
                >
                  <Icon name="chevron-right" color="white" size="md" />
                </button>
              </div>
            </div>
          </motion.div>

          {/* Events List */}
          <div className="space-y-6 px-12 pb-8">
            {events.length === 0 ? (
              <motion.div
                className="text-center py-12"
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                transition={{ duration: 0.6, delay: 0.4 }}
              >
                <Typography variant="body" color="gray">
                  No events scheduled for this month.
                </Typography>
              </motion.div>
            ) : (
              events.map((event, index) => (
                <motion.div
                  key={event.id}
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ duration: 0.5, delay: 0.4 + index * 0.1 }}
                >
                  <EventCard event={event} />
                </motion.div>
              ))
            )}
          </div>
        </div>
      </div>
    </section>
  );
}; 