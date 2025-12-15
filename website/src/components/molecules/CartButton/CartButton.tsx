'use client';

import { motion } from 'framer-motion';
import { useCart } from '@/context/CartContext';
import { ShoppingCartIcon } from '@heroicons/react/24/outline';

interface CartButtonProps {
  className?: string;
}

export default function CartButton({ className = '' }: CartButtonProps) {
  const { cart, toggleCart } = useCart();

  return (
    <motion.button
      whileHover={{ scale: 1.05 }}
      whileTap={{ scale: 0.95 }}
      onClick={toggleCart}
      className={`relative p-2 rounded-lg hover:bg-gray-100 transition-colors duration-200 ${className}`}
    >
      <ShoppingCartIcon className="w-6 h-6 text-gray-700" />
      
      {/* Cart Count Badge */}
      {cart.totalItems > 0 && (
        <motion.div
          initial={{ scale: 0 }}
          animate={{ scale: 1 }}
          className="absolute -top-1 -right-1 bg-yellow-500 text-white text-xs font-bold rounded-full w-5 h-5 flex items-center justify-center"
        >
          {cart.totalItems > 99 ? '99+' : cart.totalItems}
        </motion.div>
      )}
    </motion.button>
  );
} 