'use client';

import { useState } from 'react';
import { motion } from 'framer-motion';
import { DeliveryInformation, DeliveryFormData } from '../DeliveryInformation';
import { DeliveryOptions, DeliveryOption } from '../DeliveryOptions';
import { PaymentMethod, PaymentData } from '../PaymentMethod';
import { OrderSummary } from '../OrderSummary';
import { useCart } from '@/context/CartContext';
import { useRouter } from 'next/navigation';
import { XMarkIcon } from '@heroicons/react/24/outline';
import { apiService } from '@/services/api';
import { CreateOrderRequest, DeliveryMethodType } from '@/types';

interface CheckoutFormProps {
  className?: string;
}

export const CheckoutForm = ({ className = '' }: CheckoutFormProps) => {
  const { cart, clearCart, updateShippingCost } = useCart();
  const router = useRouter();
  const [deliveryData, setDeliveryData] = useState<DeliveryFormData | null>(null);
  const [paymentData, setPaymentData] = useState<PaymentData | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleDeliveryInfoChange = (data: DeliveryFormData) => {
    setDeliveryData(data);
    setError(null); // Clear error when form is updated
  };

  const handleDeliveryOptionChange = (option: DeliveryOption) => {
    updateShippingCost(option.cost);
  };

  const handlePaymentDataChange = (data: PaymentData) => {
    setPaymentData(data);
    setError(null); // Clear error when form is updated
  };

  const handlePlaceOrder = async () => {
    setError(null);

    if (!deliveryData) {
      setError('Por favor, preencha todas as informações de entrega');
      return;
    }

    if (!paymentData) {
      setError('Por favor, selecione um método de pagamento');
      return;
    }

    // Validate required delivery fields
    const requiredFields = ['firstName', 'lastName', 'email', 'phone', 'address', 'city', 'state', 'zipCode'];
    const missingFields = requiredFields.filter(field => !deliveryData[field as keyof DeliveryFormData]);

    if (missingFields.length > 0) {
      setError('Por favor, preencha todos os campos obrigatórios de entrega');
      return;
    }

    // Validate payment fields for card payments
    if ((paymentData.method === 'credit' || paymentData.method === 'debit')) {
      const requiredPaymentFields = ['cardNumber', 'expiryDate', 'cvv', 'nameOnCard'];
      const missingPaymentFields = requiredPaymentFields.filter(field => !paymentData[field as keyof PaymentData]);

      if (missingPaymentFields.length > 0) {
        setError('Por favor, preencha todos os dados do cartão');
        return;
      }
    }

    if (cart.items.length === 0) {
      setError('Seu carrinho está vazio');
      return;
    }

    setIsSubmitting(true);
    setError(null);

    try {
      // 1. Prepare request
      const orderItems = cart.items.map(item => ({
        id: item.variationId,
        productId: item.productId,
        quantity: item.quantity
      }));

      const orderRequest: CreateOrderRequest = {
        items: orderItems,
        deliveryMethod: DeliveryMethodType.CampusDelivery
      };

      // 2. Call API to create order
      const response = await apiService.createOrder(orderRequest);

      if (response && response.paymentUrl) {
        // Clear cart before redirecting
        clearCart();

        // 3. Redirect to Mercado Pago
        window.location.href = response.paymentUrl;
      } else {
        throw new Error('URL de pagamento não recebida do servidor.');
      }
    } catch (err: any) {
      console.error('Error placing order:', err);
      setError(err.message || 'Erro ao processar pedido. Tente novamente.');
      setIsSubmitting(false);
    }
  };

  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
      className={`max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-8 ${className}`}
    >
      {/* Error Message */}
      {error && (
        <motion.div
          initial={{ opacity: 0, y: -20 }}
          animate={{ opacity: 1, y: 0 }}
          className="mb-6 bg-red-50 border border-red-200 rounded-lg p-4 flex items-center justify-between"
        >
          <div className="flex items-center">
            <div className="flex-shrink-0">
              <XMarkIcon className="h-5 w-5 text-red-400" />
            </div>
            <div className="ml-3">
              <p className="text-sm font-medium text-red-800">{error}</p>
            </div>
          </div>
          <button
            onClick={() => setError(null)}
            className="flex-shrink-0 bg-red-50 rounded-md p-1.5 text-red-500 hover:bg-red-100 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-red-50 focus:ring-red-600"
          >
            <span className="sr-only">Fechar</span>
            <XMarkIcon className="h-3 w-3" />
          </button>
        </motion.div>
      )}

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-4">
        {/* Left Column - Delivery Information */}
        <div className="lg:pr-4 col-span-2">
          <DeliveryInformation
            onFormChange={handleDeliveryInfoChange}
            className="mb-6"
          />
          <DeliveryOptions
            onDeliveryOptionChange={handleDeliveryOptionChange}
            className="mb-6"
          />
          <PaymentMethod
            onPaymentDataChange={handlePaymentDataChange}
            className="mb-6"
          />
        </div>

        {/* Right Column - Order Summary */}
        <div className="lg:pl-4">
          <OrderSummary
            onPlaceOrder={handlePlaceOrder}
            className="sticky top-8"
          />

          {/* Loading overlay */}
          {isSubmitting && (
            <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
              <div className="bg-white p-8 rounded-lg shadow-lg text-center">
                <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto mb-4"></div>
                <p className="text-gray-600">Processando seu pedido...</p>
              </div>
            </div>
          )}
        </div>
      </div>
    </motion.div>
  );
}; 