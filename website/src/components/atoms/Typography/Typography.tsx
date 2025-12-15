import React, { ReactNode } from 'react';

interface TypographyProps {
  variant?: 'h1' | 'h2' | 'h3' | 'h4' | 'h5' | 'h6' | 'subtitle' | 'body' | 'caption';
  children: ReactNode;
  className?: string;
  color?: 'primary' | 'secondary' | 'white' | 'gray' | 'inherit';
  align?: 'left' | 'center' | 'right';
}

export const Typography = ({ 
  variant = 'body', 
  children, 
  className = '',
  color = 'primary',
  align = 'left'
}: TypographyProps) => {
  const baseClasses = 'transition-colors duration-200';
  
  const variantClasses = {
    h1: 'text-4xl md:text-5xl leading-tight',
    h2: 'text-3xl md:text-4xl leading-tight',
    h3: 'text-2xl md:text-3xl leading-tight',
    h4: 'text-xl md:text-2xl leading-tight',
    h5: 'text-lg md:text-xl leading-tight',
    h6: 'text-base md:text-lg leading-tight',
    subtitle: 'text-lg md:text-lg font-normal leading-relaxed',
    body: 'text-base leading-relaxed',
    caption: 'text-sm leading-normal',
  };

  const colorClasses = {
    primary: 'text-primary',
    secondary: 'text-secondary',
    white: 'text-white',
    gray: 'text-gray-600',
    inherit: 'text-inherit',
  };

  const alignClasses = {
    left: 'text-left',
    center: 'text-center',
    right: 'text-right',
  };

  const Component = variant.startsWith('h') ? variant as keyof React.JSX.IntrinsicElements : 'p';

  return (
    <Component 
      className={`${baseClasses} ${variantClasses[variant]} ${colorClasses[color]} ${alignClasses[align]} ${className}`}
    >
      {children}
    </Component>
  );
}; 