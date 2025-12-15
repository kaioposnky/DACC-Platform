"use client";

import { motion } from 'framer-motion';

interface NavItemProps {
  href: string;
  children: React.ReactNode;
}

export const NavItem = ({ href, children }: NavItemProps) => {
  return (
    <motion.a 
      href={href} 
      className="text-primary hover:text-gray-900 px-3 py-1 rounded-md font-medium relative inline-block"
      whileHover="hover"
      initial="initial"
    >
      {children}
      <motion.div
        className="absolute bottom-0 left-3 h-0.5 bg-primary"
        variants={{
          initial: { width: 0 },
          hover: { width: "calc(100% - 1.5rem)" }
        }}
        transition={{ duration: 0.3, ease: "easeInOut" }}
      />
    </motion.a>
  );
};
