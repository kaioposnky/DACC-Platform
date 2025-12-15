"use client";

import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import Link from 'next/link';

interface FooterLink {
  label: string;
  href: string;
  target?: string;
}

interface FooterSectionProps {
  title: string;
  links?: FooterLink[];
  children?: React.ReactNode;
  className?: string;
}

export const FooterSection = ({ title, links, children, className = '' }: FooterSectionProps) => {
  return (
    <motion.div
      className={className}
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.6, delay: 0.2 }}
    >
      <Typography variant="h5" color="white" className="font-semibold mb-6">
        {title}
      </Typography>
      
      {links && (
        <nav className="space-y-3">
          {links.map((link, index) => (
            <motion.div
              key={link.label}
              initial={{ opacity: 0, x: -20 }}
              animate={{ opacity: 1, x: 0 }}
              transition={{ duration: 0.4, delay: 0.3 + index * 0.1 }}
            >
              <Link
                href={link.href}
                target={link.target || '_self'}
                className="block text-gray-300 hover:text-white transition-colors duration-200"
              >
                {link.label}
              </Link>
            </motion.div>
          ))}
        </nav>
      )}
      
      {children && (
        <div className="text-gray-300">
          {children}
        </div>
      )}
    </motion.div>
  );
}; 