'use client';

import { Product } from '@/types';
import { motion } from 'framer-motion';
import Image from 'next/image';
import { useRouter } from 'next/navigation';
import { useCart } from '@/context/CartContext';

interface RecommendationCardProps {
  product: Product;
  className?: string;
}

export default function RecommendationCard({ product, className = '' }: RecommendationCardProps) {
  const router = useRouter();
  const { addToCart } = useCart();

  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(price);
  };

  const handleCardClick = () => {
    router.push(`/loja/${product.id}`);
  };

  const handleQuickAdd = (e: React.MouseEvent) => {
    e.stopPropagation();
    addToCart(product, 1);
  };

  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      whileHover={{ y: -5 }}
      onClick={handleCardClick}
      className={`bg-white rounded-xl shadow-md overflow-hidden group hover:shadow-lg transition-all duration-300 cursor-pointer ${className}`}
    >
      {/* Product Image */}
      <div className="relative aspect-square overflow-hidden">
        <Image
          src={product.image}
          alt={product.name}
          fill
          className="object-cover group-hover:scale-105 transition-transform duration-300"
        />
      </div>

      {/* Product Info */}
      <div className="p-4 text-center space-y-3">
        {/* Product Name */}
        <h3 className="font-semibold text-gray-900 text-sm group-hover:text-blue-600 transition-colors duration-200">
          {product.name}
        </h3>

        {/* Price */}
        <div className="text-lg font-bold text-gray-900">
          {formatPrice(product.price)}
        </div>

        {/* Quick Add Button */}
        <motion.button
          whileHover={{ scale: 1.02 }}
          whileTap={{ scale: 0.98 }}
          onClick={handleQuickAdd}
          disabled={!product.inStock}
          className="w-full bg-blue-600 hover:bg-blue-700 disabled:bg-gray-400 text-white font-semibold py-2 px-4 rounded-lg transition-colors duration-200"
        >
          {product.inStock ? 'Adicionar ao Carrinho' : 'Indisponível'}
        </motion.button>
      </div>
    </motion.div>
  );
} 