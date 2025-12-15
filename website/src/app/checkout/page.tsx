'use client';

import { Footer, Navigation } from "@/components";
import { CheckoutBanner } from "@/components/organisms/CheckoutBanner";
import { CheckoutForm } from "@/components/organisms/CheckoutForm";

export default function Checkout() {
  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />
      <CheckoutBanner />
      <CheckoutForm />
      <Footer />
    </div>
  );
}