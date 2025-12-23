'use client';

import { useState, useRef, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import Image from 'next/image';
import { UserProfile as UserProfileType } from '@/types';
import { 
  UserIcon, 
  ClipboardDocumentListIcon, 
  StarIcon, 
  ChevronDownIcon
} from '@heroicons/react/24/outline';
import {ArrowRightEndOnRectangleIcon} from "@heroicons/react/16/solid";

interface UserProfileProps {
  user: UserProfileType;
  onProfileClick?: () => void;
  onOrderHistoryClick?: () => void;
  onReviewsClick?: () => void;
  onLogoutClick?: () => void;
  className?: string;
}

export default function UserProfile({ 
  user, 
  onProfileClick, 
  onOrderHistoryClick, 
  onReviewsClick, 
  onLogoutClick,
  className = '' 
}: UserProfileProps) {
  const [isOpen, setIsOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  // Close dropdown when clicking outside
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  const handleMenuItemClick = (action?: () => void) => {
    setIsOpen(false);
    action?.();
  };

  const handleLogin = () => {
    window.location.href = '/login';
  };

  const menuItems = [
    {
      icon: UserIcon,
      label: 'Perfil',
      onClick: onProfileClick,
    },
    {
      icon: ClipboardDocumentListIcon,
      label: 'Histórico de Pedidos',
      onClick: onOrderHistoryClick,
    },
    {
      icon: StarIcon,
      label: 'Minhas Avaliações',
      onClick: onReviewsClick,
    },
    {
      icon: ArrowRightEndOnRectangleIcon,
      label: 'Deslogar',
      onClick: onLogoutClick,
      className: 'text-red-600 hover:text-red-700',
    },
  ];

  if (!user.isLoggedIn) {
    return (
      <button
        onClick={handleLogin}
        className="bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 px-4 rounded-lg transition-colors duration-200"
      >
        Login
      </button>
    );
  }

  return (
    <div ref={dropdownRef} className={`relative ${className}`}>
      {/* Profile Button */}
      <button
        onClick={() => setIsOpen(!isOpen)}
        className="flex items-center gap-2 p-2 rounded-lg hover:bg-gray-100 transition-colors duration-200"
      >
        <div className="w-8 h-8 rounded-full overflow-hidden">
          <Image
            src={user.avatar}
            alt={user.name}
            width={32}
            height={32}
            className="w-full h-full object-cover"
          />
        </div>
        <span className="text-sm font-medium text-gray-700 hidden sm:block">
          {user.name}
        </span>
        <ChevronDownIcon 
          className={`w-4 h-4 text-gray-500 transition-transform duration-200 ${
            isOpen ? 'rotate-180' : ''
          }`} 
        />
      </button>

      {/* Dropdown Menu */}
      <AnimatePresence>
        {isOpen && (
          <motion.div
            initial={{ opacity: 0, y: -10 }}
            animate={{ opacity: 1, y: 0 }}
            exit={{ opacity: 0, y: -10 }}
            transition={{ duration: 0.2 }}
            className="absolute right-0 mt-2 w-48 bg-white rounded-lg shadow-lg border border-gray-200 py-1 z-50"
          >
            {menuItems.map((item, index) => {
              const Icon = item.icon;
              return (
                <button
                  key={index}
                  onClick={() => handleMenuItemClick(item.onClick)}
                  className={`w-full flex items-center gap-3 px-4 py-3 text-left hover:bg-gray-50 transition-colors duration-200 ${
                    item.className || 'text-gray-700 hover:text-gray-900'
                  }`}
                >
                  <Icon className="w-5 h-5" />
                  <span className="text-sm font-medium">{item.label}</span>
                </button>
              );
            })}
          </motion.div>
        )}
      </AnimatePresence>
    </div>
  );
} 