'use client';

import { Product } from '@/types';
import { motion } from 'framer-motion';
import Image from 'next/image';
import { useRouter } from 'next/navigation';
import { ShoppingCartIcon, PencilIcon, TrashIcon, StarIcon, CubeIcon } from '@heroicons/react/24/solid';

interface ManageProductCardProps {
  product: Product;
  onDeleteProduct: (product: Product) => void;
  className?: string;
}

export default function ManageProductCard({
  product,
  onDeleteProduct,
  className = ''
}: ManageProductCardProps) {
  const router = useRouter();
  const hasDiscount = product.originalPrice && product.originalPrice > product.price;
  const discountPercentage = hasDiscount
    ? Math.round(((product.originalPrice! - product.price) / product.originalPrice!) * 100)
    : 0;

  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(price);
  };

  const handleEditProduct = () => {
    router.push(`/admin/produtos/${product.id}`);
  };

  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      whileHover={{ y: -5 }}
      onClick={handleEditProduct}
      className={`bg-white flex flex-row items-center gap-10 rounded-xl shadow-lg group hover:shadow-xl transition-all duration-300 cursor-pointer ${className}`}
    >
      {/* Product Image */}
      <div className="relative aspect-square overflow-hidden w-24 h-24 ml-4 rounded-lg flex-shrink-0">
        <Image
          src={product.image || "https://gerenciador.fei.edu.br/Content/Arquivos/logo_fei_color-01.svg"}
          alt={product.name}
          fill
          className="object-cover group-hover:scale-105 transition-transform duration-300"
        />

        {/* Badges */}
        <div className="absolute top-3 flex flex-col gap-2">
          {hasDiscount && (
            <span className="bg-red-500 text-white text-[8px] font-bold px-2 py-1 rounded">
              -{discountPercentage}%
            </span>
          )}
          {product.featured && (
            <span className="bg-blue-500 text-white text-[8px] font-bold px-2 py-1 rounded">
              Destaque
            </span>
          )}
          {!product.inStock && (
            <span className="bg-gray-500 text-white text-[8px] font-bold px-2 py-1 rounded">
              Esgotado
            </span>
          )}
        </div>
      </div>

      {/* Product Info */}
      <div className="grid grid-cols-[1fr_120px_100px_100px_120px] items-center flex-1 gap-4">
        {/* Product Name */}
        <h3 className="font-semibold text-lg text-gray-900 truncate group-hover:text-blue-600 transition-colors duration-200">
          {product.name}
        </h3>

        {/* Product Price */}
        <div className="flex flex-col">
          <span className="text-lg font-bold text-gray-900">
            {formatPrice(product.price)}
          </span>
          {hasDiscount && (
            <span className="text-xs text-gray-500 line-through">
              {formatPrice(product.originalPrice!)}
            </span>
          )}
        </div>

        {/* Product Stock */}
        <div className="flex flex-col items-center">
          <span className={`text-xs font-bold uppercase px-2 py-0.5 rounded-full ${product.inStock ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'}`}>
            {product.inStock ? '' : 'Esgotado'}
          </span>
          <span className="text-sm text-gray-500 flex items-center gap-1 mt-1">
            <CubeIcon className="w-4 h-4" />
            {product.stockCount} und
          </span>
        </div>

        {/* Product Rating */}
        <div className="text-sm text-gray-500 text-center">
          <span className="flex items-center justify-center gap-1 font-bold text-gray-700 text-lg">
            {product.rating}
            <StarIcon className="w-5 h-5 text-yellow-400" />
          </span>
        </div>

        {/* Product Manage Actions */}
        <div className="flex items-center justify-end gap-3 pr-5">
          <motion.button
            whileHover={{ scale: 1.1 }}
            whileTap={{ scale: 0.9 }}
            type="button"
            onClick={(e) => { e.stopPropagation(); handleEditProduct(); }}
            className="rounded-xl p-2 bg-neutral-100 hover:bg-blue-100 text-blue-600 transition-colors cursor-pointer"
          >
            <PencilIcon className="w-5 h-5" />
          </motion.button>
          <motion.button
            whileHover={{ scale: 1.1 }}
            whileTap={{ scale: 0.9 }}
            type="button"
            onClick={(e) => { e.stopPropagation(); onDeleteProduct(product); }}
            className="rounded-xl p-2 bg-neutral-100 hover:bg-red-100 text-red-600 transition-colors cursor-pointer"
          >
            <TrashIcon className="w-5 h-5" />
          </motion.button>
        </div>
      </div>

    </motion.div>
  );
}
