'use client';

import { useState } from 'react';
import { motion } from 'framer-motion';
import { Typography, Button } from '@/components/atoms';
import { Input } from '@/components/atoms/Input';
import { useCart } from '@/context/CartContext';
import { 
  ArrowLeftIcon, 
  LockClosedIcon, 
  ArrowPathIcon, 
  ChatBubbleLeftRightIcon 
} from '@heroicons/react/24/outline';
import Link from 'next/link';

interface OrderSummaryProps {
  onPlaceOrder?: () => void;
  className?: string;
}

export const OrderSummary = ({ onPlaceOrder, className = '' }: OrderSummaryProps) => {
  const { cart } = useCart();
  const [promoCode, setPromoCode] = useState('');
  const [promoDiscount, setPromoDiscount] = useState(0);
  const [isApplyingPromo, setIsApplyingPromo] = useState(false);

  const formatPrice = (price: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(price);
  };

  const handleApplyPromo = async () => {
    if (!promoCode.trim()) return;
    
    setIsApplyingPromo(true);
    
    // Simulate API call to validate promo code
    setTimeout(() => {
      // Mock promo code validation
      if (promoCode.toLowerCase() === 'save10') {
        setPromoDiscount(cart.subtotal * 0.1); // 10% discount
      } else if (promoCode.toLowerCase() === 'free5') {
        setPromoDiscount(5); // R$5 off
      } else {
        setPromoDiscount(0);
        alert('Código promocional inválido');
      }
      setIsApplyingPromo(false);
    }, 1000);
  };

  const finalTotal = cart.totalAmount - promoDiscount;

  const handlePlaceOrder = () => {
    if (onPlaceOrder) {
      onPlaceOrder();
    } else {
      // Default behavior - could redirect to payment
      console.log('Placing order with total:', finalTotal);
    }
  };

  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5, delay: 0.2 }}
      className={`bg-white rounded-lg shadow-sm border border-gray-200 p-6 h-fit ${className}`}
    >
      {/* Header */}
      <div className="flex items-center justify-between mb-6">
        <Typography variant="h4" className="text-primary font-semibold !text-xl">
          Resumo do Pedido
        </Typography>
        <Link
          href="/loja"
          className="flex  items-center justify-center gap-2 px-2 py-2 text-sm text-primary hover:opacity-80 transition-colors bg-gray-100  duration-200 rounded-xl border-2 border-blue-100"
        >
          <ArrowLeftIcon className="w-4 h-4" />
          <Typography variant="body" className="text-primary text-sm hover:opacity-80 !text-center !leading-4.5 transition-colors duration-200">Continuar Comprando</Typography>
        </Link>
      </div>

      {/* Order Totals */}
      <div className="space-y-4 mb-6">
        {/* Subtotal */}
        <div className="flex justify-between items-center">
          <Typography variant="body" className="text-primary">
            Subtotal:
          </Typography>
          <Typography variant="body" className="font-medium">
            {formatPrice(cart.subtotal)}
          </Typography>
        </div>

        {/* Delivery */}
        <div className="flex justify-between items-center">
          <Typography variant="body" className="text-gray-600">
            Entrega:
          </Typography>
          <Typography variant="body" className="font-medium text-green-600">
            {cart.shippingCost > 0 ? formatPrice(cart.shippingCost) : 'GRÁTIS'}
          </Typography>
        </div>

        {/* Tax */}
        <div className="flex justify-between items-center">
          <Typography variant="body" className="text-gray-600">
            Impostos:
          </Typography>
          <Typography variant="body" className="font-medium">
            {formatPrice(0)} {/* Assuming no tax for now */}
          </Typography>
        </div>

        {/* Promo Discount */}
        {promoDiscount > 0 && (
          <div className="flex justify-between items-center">
            <Typography variant="body" className="text-gray-600">
              Desconto:
            </Typography>
            <Typography variant="body" className="font-medium text-green-600">
              -{formatPrice(promoDiscount)}
            </Typography>
          </div>
        )}

        {/* Divider */}
        <div className="border-t border-gray-200 my-4"></div>

        {/* Total */}
        <div className="flex justify-between items-center">
          <Typography variant="h5" className="text-gray-900 font-bold">
            Total:
          </Typography>
          <Typography variant="h5" className="font-bold text-gray-900">
            {formatPrice(finalTotal)}
          </Typography>
        </div>
      </div>

      {/* Promo Code */}
      <div className="mb-6">
        <div className="flex gap-2">
          <Input
            type="text"
            placeholder="Digite o código promocional"
            value={promoCode}
            onChange={(e) => setPromoCode(e.target.value)}
            className="flex-1"
          />
          <Button
            onClick={handleApplyPromo}
            disabled={isApplyingPromo || !promoCode.trim()}
            variant="primary"
            className="px-4 py-2"
          >
            {isApplyingPromo ? 'Aplicando...' : 'Aplicar'}
          </Button>
        </div>
      </div>

      {/* Place Order Button */}
      <Button
        onClick={handlePlaceOrder}
        disabled={cart.items.length === 0}
        variant="primary"
        size="lg"
        className="w-full mb-6"
      >
        <LockClosedIcon className="w-5 h-5" />
        Finalizar Pedido
      </Button>

      {/* Security Features */}
      <div className="space-y-3 text-sm text-gray-600">
        <div className="flex items-center gap-2">
          <LockClosedIcon className="w-4 h-4" />
          <span>Criptografia SSL segura</span>
        </div>
        <div className="flex items-center gap-2">
          <ArrowPathIcon className="w-4 h-4" />
          <span>Política de devolução de 30 dias</span>
        </div>
        <div className="flex items-center gap-2">
          <ChatBubbleLeftRightIcon className="w-4 h-4" />
          <span>Suporte ao cliente 24/7</span>
        </div>
      </div>
    </motion.div>
  );
}; 