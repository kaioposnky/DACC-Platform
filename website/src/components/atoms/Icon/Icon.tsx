"use client";

import { ReactNode } from 'react';

interface IconProps {
  name: 'chevron-left' | 'chevron-right' | 'play' | 'pause' | 'arrow-left' | 'arrow-right';
  size?: 'sm' | 'md' | 'lg';
  color?: 'primary' | 'secondary' | 'white' | 'gray' | 'inherit';
  className?: string;
}

export const Icon = ({ 
  name, 
  size = 'md', 
  color = 'inherit',
  className = ''
}: IconProps) => {
  const sizeClasses = {
    sm: 'w-4 h-4',
    md: 'w-6 h-6',
    lg: 'w-8 h-8',
  };

  const colorClasses = {
    primary: 'text-primary',
    secondary: 'text-secondary',
    white: 'text-white',
    gray: 'text-gray-600',
    inherit: 'text-inherit',
  };

  const iconPaths: Record<string, ReactNode> = {
    'chevron-left': (
      <path strokeLinecap="round" strokeLinejoin="round" d="M15.75 19.5L8.25 12l7.5-7.5" />
    ),
    'chevron-right': (
      <path strokeLinecap="round" strokeLinejoin="round" d="M8.25 4.5l7.5 7.5-7.5 7.5" />
    ),
    'arrow-left': (
      <path strokeLinecap="round" strokeLinejoin="round" d="M19.5 12h-15m0 0l6.75 6.75M4.5 12l6.75-6.75" />
    ),
    'arrow-right': (
      <path strokeLinecap="round" strokeLinejoin="round" d="M4.5 12h15m0 0l-6.75-6.75M19.5 12l-6.75 6.75" />
    ),
    play: (
      <path strokeLinecap="round" strokeLinejoin="round" d="M5.25 5.653c0-.856.917-1.398 1.667-.986l11.54 6.347a1.125 1.125 0 0 1 0 1.972l-11.54 6.347a1.125 1.125 0 0 1-1.667-.986V5.653Z" />
    ),
    pause: (
      <path strokeLinecap="round" strokeLinejoin="round" d="M15.75 5.25v13.5m-7.5-13.5v13.5" />
    ),
  };

  return (
    <svg 
      className={`${sizeClasses[size]} ${colorClasses[color]} ${className}`}
      fill="none" 
      viewBox="0 0 24 24" 
      strokeWidth={2} 
      stroke="currentColor"
    >
      {iconPaths[name]}
    </svg>
  );
}; 