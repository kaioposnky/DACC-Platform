'use client';

import { useState } from 'react';
import { motion } from 'framer-motion';
import { Product } from '@/types';
import Image from 'next/image';
import { StarIcon, CheckBadgeIcon, HandThumbUpIcon } from '@heroicons/react/24/solid';
import { StarIcon as StarOutlineIcon } from '@heroicons/react/24/outline';

interface ProductTabsProps {
  product: Product;
  className?: string;
}

type TabType = 'description' | 'specifications' | 'reviews' | 'shipping';

export default function ProductTabs({ product, className = '' }: ProductTabsProps) {
  const [activeTab, setActiveTab] = useState<TabType>('description');

  const tabs = [
    { id: 'description' as TabType, label: 'Descrição' },
    { id: 'specifications' as TabType, label: 'Especificações' },
    { id: 'reviews' as TabType, label: `Avaliações (${product.reviews})` },
    { id: 'shipping' as TabType, label: 'Envio & Devoluções' },
  ];

  const renderStars = (rating: number) => {
    const stars = [];
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 !== 0;
    
    for (let i = 0; i < fullStars; i++) {
      stars.push(
        <StarIcon key={i} className="w-4 h-4 text-yellow-400" />
      );
    }
    
    if (hasHalfStar) {
      stars.push(
        <div key="half" className="relative">
          <StarOutlineIcon className="w-4 h-4 text-yellow-400" />
          <StarIcon className="w-4 h-4 text-yellow-400 absolute top-0 left-0" style={{ width: '50%', overflow: 'hidden' }} />
        </div>
      );
    }
    
    const remainingStars = 5 - Math.ceil(rating);
    for (let i = 0; i < remainingStars; i++) {
      stars.push(
        <StarOutlineIcon key={`outline-${i}`} className="w-4 h-4 text-gray-300" />
      );
    }
    
    return stars;
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('pt-BR', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  const renderTabContent = () => {
    switch (activeTab) {
      case 'description':
        return (
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            <div className="lg:col-span-2 space-y-6">
              <div>
                <h3 className="text-2xl font-bold text-gray-900 mb-4">Descrição do Produto</h3>
                <div className="prose prose-gray max-w-none">
                  {product.detailedDescription?.split('\n\n').map((paragraph, index) => (
                    <p key={index} className="text-gray-600 leading-relaxed mb-4">
                      {paragraph}
                    </p>
                  ))}
                </div>
              </div>
              
              {product.perfectFor && product.perfectFor.length > 0 && (
                <div>
                  <h4 className="text-lg font-semibold text-gray-900 mb-3">Perfeito para:</h4>
                  <ul className="space-y-2">
                    {product.perfectFor.map((item, index) => (
                      <li key={index} className="flex items-center gap-2 text-gray-600">
                        <div className="w-2 h-2 bg-blue-500 rounded-full"></div>
                        {item}
                      </li>
                    ))}
                  </ul>
                </div>
              )}
            </div>
            
            <div className="lg:col-span-1">
              <div className="bg-gray-50 rounded-xl p-6">
                <Image
                  src={product.images[2] || product.images[0]}
                  alt="Product lifestyle"
                  width={400}
                  height={400}
                  className="w-full rounded-lg object-cover"
                />
              </div>
            </div>
          </div>
        );

      case 'specifications':
        return (
          <div className="max-w-4xl">
            <h3 className="text-2xl font-bold text-gray-900 mb-6">Especificações</h3>
            {product.specifications && product.specifications.length > 0 ? (
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                {product.specifications.map((spec, index) => (
                  <div key={index} className="bg-gray-50 rounded-lg p-4">
                    <div className="flex justify-between items-start">
                      <dt className="font-medium text-gray-900">{spec.name}</dt>
                      <dd className="text-gray-600 text-right ml-4">{spec.value}</dd>
                    </div>
                  </div>
                ))}
              </div>
            ) : (
              <p className="text-gray-500">Especificações não disponíveis para este produto.</p>
            )}
          </div>
        );

      case 'reviews':
        return (
          <div className="max-w-4xl space-y-8">
            <div className="flex items-center justify-between">
              <h3 className="text-2xl font-bold text-gray-900">Avaliações</h3>
              <div className="flex items-center gap-2">
                <div className="flex items-center">
                  {renderStars(product.rating)}
                </div>
                <span className="text-lg font-semibold text-gray-900">{product.rating}</span>
                <span className="text-gray-500">({product.reviews} avaliações)</span>
              </div>
            </div>

            {product.reviewsList && product.reviewsList.length > 0 ? (
              <div className="space-y-6">
                {product.reviewsList.map((review) => (
                  <div key={review.id} className="bg-gray-50 rounded-xl p-6">
                    <div className="flex items-start gap-4">
                      <div className="w-12 h-12 rounded-full overflow-hidden flex-shrink-0">
                        <Image
                          src={review.userAvatar}
                          alt={review.userName}
                          width={48}
                          height={48}
                          className="w-full h-full object-cover"
                        />
                      </div>
                      
                      <div className="flex-1 space-y-3">
                        <div className="flex items-center justify-between">
                          <div>
                            <div className="flex items-center gap-2">
                              <h4 className="font-semibold text-gray-900">{review.userName}</h4>
                              {review.verified && (
                                <CheckBadgeIcon className="w-5 h-5 text-blue-500" />
                              )}
                            </div>
                            <p className="text-sm text-gray-500">{formatDate(review.date)}</p>
                          </div>
                          <div className="flex items-center">
                            {renderStars(review.rating)}
                          </div>
                        </div>
                        
                        <div>
                          <h5 className="font-medium text-gray-900 mb-2">{review.title}</h5>
                          <p className="text-gray-600 leading-relaxed">{review.comment}</p>
                        </div>
                        
                        <div className="flex items-center gap-2 pt-2">
                          <button className="flex items-center gap-1 text-sm text-gray-500 hover:text-gray-700 transition-colors">
                            <HandThumbUpIcon className="w-4 h-4" />
                            Útil ({review.helpful})
                          </button>
                        </div>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            ) : (
              <div className="text-center py-12">
                <p className="text-gray-500">Nenhuma avaliação disponível para este produto.</p>
              </div>
            )}
          </div>
        );

      case 'shipping':
        return (
          <div className="max-w-4xl space-y-8">
            <div>
              <h3 className="text-2xl font-bold text-gray-900 mb-6">Envio & Devoluções</h3>
              
              {product.shippingInfo ? (
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <div className="bg-blue-50 rounded-xl p-6">
                    <h4 className="text-lg font-semibold text-blue-900 mb-3">Informações de Envio</h4>
                    <ul className="space-y-2 text-blue-800">
                      <li className="flex items-center gap-2">
                        <div className="w-2 h-2 bg-blue-500 rounded-full"></div>
                        {product.shippingInfo.freeShipping ? 'Frete grátis' : `Frete: R$ ${product.shippingInfo.shippingCost}`}
                      </li>
                      <li className="flex items-center gap-2">
                        <div className="w-2 h-2 bg-blue-500 rounded-full"></div>
                        Entrega estimada: {product.shippingInfo.estimatedDays}
                      </li>
                    </ul>
                  </div>
                  
                  <div className="bg-green-50 rounded-xl p-6">
                    <h4 className="text-lg font-semibold text-green-900 mb-3">Devoluções & Garantia</h4>
                    <ul className="space-y-2 text-green-800">
                      <li className="flex items-center gap-2">
                        <div className="w-2 h-2 bg-green-500 rounded-full"></div>
                        {product.shippingInfo.returnPolicy}
                      </li>
                      {product.shippingInfo.warranty && (
                        <li className="flex items-center gap-2">
                          <div className="w-2 h-2 bg-green-500 rounded-full"></div>
                          {product.shippingInfo.warranty}
                        </li>
                      )}
                    </ul>
                  </div>
                </div>
              ) : (
                <p className="text-gray-500">Informações de envio não disponíveis para este produto.</p>
              )}
            </div>
          </div>
        );

      default:
        return null;
    }
  };

  return (
    <div className={`bg-white rounded-xl shadow-lg overflow-hidden ${className}`}>
      {/* Tab Navigation */}
      <div className="border-b border-gray-200">
        <nav className="flex">
          {tabs.map((tab) => (
            <button
              key={tab.id}
              onClick={() => setActiveTab(tab.id)}
              className={`flex-1 py-4 px-6 text-center font-medium transition-colors duration-200 relative ${
                activeTab === tab.id
                  ? 'text-blue-600 border-b-2 border-blue-600 bg-blue-50'
                  : 'text-gray-500 hover:text-gray-700 hover:bg-gray-50'
              }`}
            >
              {tab.label}
              {activeTab === tab.id && (
                <motion.div
                  layoutId="activeTab"
                  className="absolute bottom-0 left-0 right-0 h-0.5 bg-blue-600"
                  initial={false}
                  transition={{ type: "spring", bounce: 0.2, duration: 0.6 }}
                />
              )}
            </button>
          ))}
        </nav>
      </div>

      {/* Tab Content */}
      <div className="p-8 flex justify-center">
        <motion.div
          key={activeTab}
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.3 }}
        >
          {renderTabContent()}
        </motion.div>
      </div>
    </div>
  );
} 