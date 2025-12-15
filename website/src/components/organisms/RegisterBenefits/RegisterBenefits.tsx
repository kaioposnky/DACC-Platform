'use client';

import { motion } from 'framer-motion';
import { 
  BookOpenIcon, 
  //HandshakeIcon, 
  TrophyIcon, 
  RocketLaunchIcon,
  UserGroupIcon 
} from '@heroicons/react/24/outline';

interface Benefit {
  icon: React.ReactNode;
  title: string;
  description: string;
}

interface RegisterBenefitsProps {
  className?: string;
}

export default function RegisterBenefits({ className = '' }: RegisterBenefitsProps) {
  const benefits: Benefit[] = [
    {
      icon: <BookOpenIcon className="w-6 h-6 text-yellow-400" />,
      title: 'Learning Resources',
      description: 'Access exclusive study materials, tutorials, and coding challenges'
    },
    {
      icon: '',//<HandshakeIcon className="w-6 h-6 text-yellow-400" />,
      title: 'Project Collaboration',
      description: 'Work on real projects with fellow students and build your portfolio'
    },
    {
      icon: <TrophyIcon className="w-6 h-6 text-yellow-400" />,
      title: 'Achievements & Badges',
      description: 'Earn recognition for your progress and showcase your skills'
    },
    {
      icon: <UserGroupIcon className="w-6 h-6 text-yellow-400" />,
      title: 'Mentorship Program',
      description: 'Connect with experienced developers and industry professionals'
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
    hidden: { opacity: 0, y: 20 },
    visible: {
      opacity: 1,
      y: 0,
      transition: {
        duration: 0.4,
      },
    },
  };

  const testimonialVariants = {
    hidden: { opacity: 0, y: 20 },
    visible: {
      opacity: 1,
      y: 0,
      transition: {
        duration: 0.5,
        delay: 0.6,
      },
    },
  };

  return (
    <div className={`bg-primary text-white p-8 rounded-2xl ${className}`}>
      {/* Header */}
      <motion.div
        initial={{ opacity: 0, y: -20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5 }}
        className="text-center mb-8"
      >
        <div className="inline-flex items-center justify-center w-16 h-16 bg-yellow-400 rounded-full mb-4">
          <RocketLaunchIcon className="w-8 h-8 text-primary" />
        </div>
        <h2 className="text-2xl font-bold">Start Your Journey</h2>
      </motion.div>

      {/* Benefits List */}
      <motion.div
        variants={containerVariants}
        initial="hidden"
        animate="visible"
        className="space-y-6 mb-8"
      >
        {benefits.map((benefit, index) => (
          <motion.div
            key={index}
            variants={itemVariants}
            className="flex items-start gap-4"
          >
            <div className="flex-shrink-0 mt-1">
              {benefit.icon}
            </div>
            <div>
              <h3 className="text-lg font-semibold mb-1">{benefit.title}</h3>
              <p className="text-blue-100 text-sm leading-relaxed">{benefit.description}</p>
            </div>
          </motion.div>
        ))}
      </motion.div>

      {/* Testimonial */}
      <motion.div
        variants={testimonialVariants}
        initial="hidden"
        animate="visible"
        className="bg-blue-800 bg-opacity-50 rounded-xl p-6 border border-blue-600 border-opacity-30"
      >
        <div className="mb-4">
          <p className="text-blue-100 italic leading-relaxed">
            &quot;Joining Coruja Overflow was the best decision I made as a CS student. The community is amazing!&quot;
          </p>
        </div>
        
        <div className="flex items-center gap-3">
          <div className="w-10 h-10 rounded-full bg-gradient-to-br from-yellow-400 to-orange-500 flex items-center justify-center">
            <span className="text-primary font-bold text-sm">AJ</span>
          </div>
          <div>
            <p className="text-white font-medium text-sm">Alex Johnson</p>
            <p className="text-blue-200 text-xs">Computer Science, 3rd Year</p>
          </div>
        </div>
      </motion.div>
    </div>
  );
} 