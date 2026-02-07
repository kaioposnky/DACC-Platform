"use client";

import { motion } from 'framer-motion';
import { Card, Typography } from '@/components/atoms';
import { Faculty } from '@/types';
import Image from 'next/image';
import { getSocialIcon } from '../SocialIcons/SocialIcons';

interface FacultyCardProps {
  faculty: Faculty;
  className?: string;
}

export const FacultyCard = ({ faculty, className = '' }: FacultyCardProps) => {

  return (
    <motion.div
      className={className}
      initial={{ opacity: 0, y: 30 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.6, delay: 0.2 }}
    >


      <Card className="text-center h-full  hover:shadow-xl transition-shadow duration-300 !p-0">
        {/* Profile Image */}
        <motion.div
          initial={{ scale: 0.8, opacity: 0 }}
          animate={{ scale: 1, opacity: 1 }}
          transition={{ duration: 0.6, delay: 0.3 }}
        >
          <div className="relative w-full h-50 mx-auto overflow-hidden rounded-t-lg">
            <Image
              src={faculty.imageUrl}
              alt={faculty.name}
              fill
              className="object-cover"
              sizes="100%"
            />
          </div>
        </motion.div>
        {/* Name and Title */}
        <motion.div
          className="mt-4"
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ duration: 0.6, delay: 0.4 }}
        >
          <Typography variant="h4" className="mb-2 text-primary font-bold" align="center">
            {faculty.name}
          </Typography>
          <Typography variant="body" className="text-primary font-medium" align="center">
            {faculty.position}
          </Typography>
        </motion.div>

        {/* Specialization */}
        <motion.div
          className="mb-6"
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ duration: 0.6, delay: 0.5 }}
        >
          <Typography variant="body" color="gray" className="text-sm" align="center">
            {faculty.specialization}
          </Typography>
        </motion.div>

        {/* Social Links */}
        <motion.div
          className="flex justify-center space-x-4 pb-8"
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6, delay: 0.6 }}
        >
          {Object.entries(faculty.social).map(([platform, url], index) => {
            if (!url) return null;

            return (
              <motion.a
                key={platform}
                href={platform === 'email' ? `mailto:${url}` : url}
                target={platform === 'email' ? '_self' : '_blank'}
                rel={platform === 'email' ? '' : 'noopener noreferrer'}
                className="w-10 h-10 bg-gray-100 hover:bg-primary hover:text-white rounded-full flex items-center justify-center text-gray-600 transition-colors duration-200"
                whileHover={{ scale: 1.1 }}
                whileTap={{ scale: 0.95 }}
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.4, delay: 0.7 + index * 0.1 }}
              >
                {getSocialIcon(platform as 'github' | 'linkedin' | 'email', 'w-5 h-5 text-current')}
              </motion.a>
            );
          })}
        </motion.div>
      </Card>
    </motion.div>
  );
};
