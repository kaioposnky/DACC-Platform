"use client";

import { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { FacultyCard } from '@/components/molecules';
import { Faculty } from '@/types';
import { apiService } from '@/services/api';

interface FacultySectionProps {
  className?: string;
  title?: string;
  subtitle?: string;
  backgroundColor?: 'white' | 'gray' | 'primary';
}

export const FacultySection = ({ 
  className = '',
  title = 'Diretores e Diretoras do DACC',
  subtitle = 'Conheça as pessoas que ajudam o DACC a ser o que é hoje.',
  backgroundColor = 'white'
}: FacultySectionProps) => {
  const [faculty, setFaculty] = useState<Faculty[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchFaculty();
  }, []);

  const fetchFaculty = async () => {
    try {
      setLoading(true);
      const fetchedFaculty = await apiService.getFaculty();
      setFaculty(fetchedFaculty);
    } catch (error) {
      console.error('Error fetching faculty:', error);
    } finally {
      setLoading(false);
    }
  };

  const getBackgroundClasses = () => {
    switch (backgroundColor) {
      case 'white':
        return 'bg-white';
      case 'gray':
        return 'bg-gray-50';
      case 'primary':
        return 'bg-primary';
      default:
        return 'bg-white';
    }
  };

  const getTitleColor = () => {
    return backgroundColor === 'primary' ? 'white' : 'primary';
  };

  const getSubtitleColor = () => {
    return backgroundColor === 'primary' ? 'white' : 'gray';
  };

  if (loading) {
    return (
      <section className={`py-16 ${getBackgroundClasses()} ${className}`}>
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-12">
            <div className="animate-pulse">
              <div className="h-8 bg-gray-200 rounded w-64 mx-auto mb-4"></div>
              <div className="h-4 bg-gray-200 rounded w-96 mx-auto mb-8"></div>
            </div>
          </div>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
            {[...Array(3)].map((_, index) => (
              <div key={index} className="animate-pulse">
                <div className="bg-white rounded-lg p-8 h-96">
                  <div className="w-32 h-32 bg-gray-200 rounded-full mx-auto mb-6"></div>
                  <div className="h-6 bg-gray-200 rounded w-3/4 mx-auto mb-2"></div>
                  <div className="h-4 bg-gray-200 rounded w-1/2 mx-auto mb-4"></div>
                  <div className="h-4 bg-gray-200 rounded w-full mx-auto mb-6"></div>
                  <div className="flex justify-center space-x-4">
                    <div className="w-10 h-10 bg-gray-200 rounded-full"></div>
                    <div className="w-10 h-10 bg-gray-200 rounded-full"></div>
                    <div className="w-10 h-10 bg-gray-200 rounded-full"></div>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>
    );
  }

  return (
    <section className={`py-16 ${getBackgroundClasses()} ${className}`}>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Section Header */}
        <motion.div
          className="text-center mb-16"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6 }}
        >
          <Typography 
            variant="h2" 
            color={getTitleColor()}
            className="mb-4 font-bold"
            align="center"
          >
            {title}
          </Typography>
          <Typography 
            variant="subtitle" 
            color={getSubtitleColor()}
            className="max-w-2xl mx-auto"
            align="center"
          >
            {subtitle}
          </Typography>
        </motion.div>

        {/* Faculty Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 lg:gap-12">
          {faculty.length === 0 ? (
            <motion.div
              className="col-span-full text-center py-12"
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              transition={{ duration: 0.6, delay: 0.2 }}
            >
              <Typography variant="body" color="gray">
                No faculty members available at the moment.
              </Typography>
            </motion.div>
          ) : (
            faculty.map((member, index) => (
              <motion.div
                key={member.id}
                initial={{ opacity: 0, y: 30 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.6, delay: 0.2 + index * 0.1 }}
              >
                <FacultyCard faculty={member} />
              </motion.div>
            ))
          )}
        </div>
      </div>
    </section>
  );
}; 