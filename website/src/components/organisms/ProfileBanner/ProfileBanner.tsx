'use client';

import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import { CameraIcon } from '@heroicons/react/24/solid';
import Image from 'next/image';
import {UserStats} from "@/types";

export interface ProfileUser {
  id: string;
  name: string;
  email: string;
  role: string;
  avatar: string;
  stats: UserStats
}

interface ProfileBannerProps {
  user: ProfileUser;
  onChangeAvatar?: () => void;
  className?: string;
}

export const ProfileBanner = ({ 
  user, 
  onChangeAvatar,
  className = '' 
}: ProfileBannerProps) => {
  
  const statItems = [
    {
      value: user.stats.orders ?? 0,
      label: 'Pedidos',
      id: 'orders'
    },
    {
      value: user.stats.reviews ?? 0,
      label: 'Avaliações',
      id: 'reviews'
    },
    {
      value: user.stats.registryDate ?? "22/12/2025",
      label: 'Membro desde',
      id: 'registryDate'
    }
  ];

  return (
    <section className={`relative py-16 bg-primary ${className}`}>
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 relative z-10">
        <div className="flex flex-col lg:flex-row items-center lg:items-start gap-8">
          
          {/* Profile Image */}
          <motion.div
            initial={{ opacity: 0, scale: 0.8 }}
            animate={{ opacity: 1, scale: 1 }}
            transition={{ duration: 0.5 }}
            className="relative flex-shrink-0"
          >
            <div className="w-32 h-32 md:w-40 md:h-40 relative">
              <Image
                src={user.avatar}
                alt={`Foto de perfil de ${user.name}`}
                width={160}
                height={160}
                className="w-full h-full rounded-full object-cover border-4 border-white shadow-lg"
              />
              
              {/* Camera Badge */}
              {onChangeAvatar && (
                <motion.button
                  initial={{ opacity: 0, scale: 0.8 }}
                  animate={{ opacity: 1, scale: 1 }}
                  transition={{ duration: 0.3, delay: 0.2 }}
                  onClick={onChangeAvatar}
                  className="absolute bottom-2 right-2 w-10 h-10 bg-yellow-400 hover:bg-yellow-500 rounded-full flex items-center justify-center shadow-lg transition-colors duration-200"
                  aria-label="Alterar foto do perfil"
                >
                  <CameraIcon className="w-5 h-5 text-gray-900" />
                </motion.button>
              )}
            </div>
          </motion.div>

          {/* User Information */}
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.2 }}
            className="flex-1 text-center lg:text-left"
          >
            {/* Name */}
            <Typography 
              variant="h1" 
              color="white"
              className="font-bold text-3xl md:text-4xl lg:text-5xl mb-2"
            >
              {user.name}
            </Typography>

            {/* Role */}
            <Typography 
              variant="h5" 
              className="text-yellow-400 font-medium mb-3"
            >
              {user.role[0].toUpperCase()}{user.role.slice(1)}
            </Typography>

            {/* Email */}
            <Typography 
              variant="body" 
              color="white"
              className="opacity-90 mb-8"
            >
              {user.email}
            </Typography>

            {/* Statistics */}
            <motion.div
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.5, delay: 0.4 }}
              className="flex flex-wrap justify-center lg:justify-start gap-8"
            >
              {statItems.map((stat, index) => (
                <motion.div
                  key={stat.id}
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ duration: 0.3, delay: 0.5 + index * 0.1 }}
                  className="text-center"
                >
                  <Typography
                    variant="body" 
                    color="white"
                    className="opacity-90 font-medium"
                  >
                    {stat.label}
                  </Typography>
                  <Typography
                      variant="h2"
                      className="text-yellow-400 font-bold text-3xl md:text-4xl"
                  >
                    {stat.value}
                  </Typography>
                </motion.div>
              ))}
            </motion.div>
          </motion.div>
        </div>
      </div>
    </section>
  );
}; 