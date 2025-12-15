'use client';

import { motion } from 'framer-motion';
import { 
  AcademicCapIcon, 
  UsersIcon, 
  ShoppingBagIcon, 
  CalendarIcon,
  BookmarkIcon 
} from '@heroicons/react/24/outline';

interface Benefit {
  icon: React.ReactNode;
  title: string;
  description: string;
}

interface CommunityBenefitsProps {
  className?: string;
}

export default function CommunityBenefits({ className = '' }: CommunityBenefitsProps) {
  const benefits: Benefit[] = [
    {
      icon: <UsersIcon className="w-6 h-6 text-yellow-400" />,
      title: 'Connect with Peers',
      description: 'Join a community of passionate computer science students'
    },
    {
      icon: <ShoppingBagIcon className="w-6 h-6 text-yellow-400" />,
      title: 'Exclusive Shop Access',
      description: 'Get access to student discounts and exclusive merchandise'
    },
    {
      icon: <CalendarIcon className="w-6 h-6 text-yellow-400" />,
      title: 'Event Notifications',
      description: 'Stay updated on hackathons, workshops, and tech events'
    },
    {
      icon: <BookmarkIcon className="w-6 h-6 text-yellow-400" />,
      title: 'Save Your Progress',
      description: 'Track your learning journey and save favorite content'
    }
  ];

  const containerVariants = {
    hidden: { opacity: 0 },
    visible: {
      opacity: 1,
      transition: {
        staggerChildren: 0.1,
        delayChildren: 0.2,
      },
    },
  };

  const itemVariants = {
    hidden: { opacity: 0, x: 20 },
    visible: {
      opacity: 1,
      x: 0,
      transition: {
        duration: 0.4,
      },
    },
  };

  const statsVariants = {
    hidden: { opacity: 0, y: 20 },
    visible: {
      opacity: 1,
      y: 0,
      transition: {
        duration: 0.5,
        delay: 0.8,
      },
    },
  };

  return (
    <div className={`text-white ${className}`}>
      {/* Header */}
      <motion.div
        initial={{ opacity: 0, y: -20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
        className="text-center mb-12"
      >
        <div className="inline-flex items-center justify-center w-16 h-16 bg-yellow-400 rounded-full mb-6">
          <AcademicCapIcon className="w-8 h-8 text-blue-900" />
        </div>
        <h2 className="text-3xl font-bold mb-4">Join Our Community</h2>
      </motion.div>

      {/* Benefits List */}
      <motion.div
        variants={containerVariants}
        initial="hidden"
        animate="visible"
        className="space-y-8 mb-16"
      >
        {benefits.map((benefit, index) => (
          <motion.div
            key={index}
            variants={itemVariants}
            className="flex items-start gap-4"
          >
            <div className="flex-shrink-0">
              {benefit.icon}
            </div>
            <div>
              <h3 className="text-lg font-semibold mb-2">{benefit.title}</h3>
              <p className="text-blue-100 leading-relaxed">{benefit.description}</p>
            </div>
          </motion.div>
        ))}
      </motion.div>

      {/* Stats */}
      <motion.div
        variants={statsVariants}
        initial="hidden"
        animate="visible"
        className="grid grid-cols-3 gap-8 text-center"
      >
        <div>
          <div className="text-3xl font-bold text-yellow-400 mb-2">1,200+</div>
          <div className="text-sm text-blue-100">Students</div>
        </div>
        <div>
          <div className="text-3xl font-bold text-yellow-400 mb-2">50+</div>
          <div className="text-sm text-blue-100">Projects</div>
        </div>
        <div>
          <div className="text-3xl font-bold text-yellow-400 mb-2">25+</div>
          <div className="text-sm text-blue-100">Events</div>
        </div>
      </motion.div>
    </div>
  );
} 