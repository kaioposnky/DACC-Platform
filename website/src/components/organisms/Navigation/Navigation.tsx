"use client";

import { NavItem } from "@/components/atoms/NavItem";
import { UserProfile, CartButton, ShoppingCart } from "@/components/molecules";
import { UserProfile as UserProfileType } from "@/types";
import Link from "next/link";
import { useAuth } from "@/context/AuthContext";
import { useRouter } from "next/navigation";

export const Navigation = () => {
  const { user, logout, isAuthenticated, isLoading } = useAuth();
  const router = useRouter();

  const handleProfileClick = () => {
    router.push('/perfil');
  };

  const handleOrderHistoryClick = () => {
    router.push('/perfil/pedidos');
  };

  const handleReviewsClick = () => {
    router.push('/perfil/avaliacoes');
  };

  const handleLogoutClick = () => {
    logout();
    router.push('/');
  };

  return (
    <>
      <header className="bg-white shadow-sm border-gray-200">
        <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center py-4">
            <div className="flex items-center">
              <h1 className="text-2xl font-bold text-primary">
                Coruja Overflow
              </h1>
            </div>
            
            {/* Desktop Navigation */}
            <nav className="hidden md:flex space-x-4">
              <NavItem href="/">Inicio</NavItem>
              <NavItem href="/sobre">Sobre</NavItem>
              <NavItem href="/noticias">Noticias</NavItem>
              <NavItem href="/loja">Loja</NavItem>
              <NavItem href="/#apoie">Apoie o DACC</NavItem>
            </nav>

            {/* User Actions */}
            <div className="flex items-center gap-4">
              {(!isLoading && isAuthenticated && user !== null) ? (
                <UserProfile
                  user={{
                    id: user.id,
                    name: user.name,
                    email: user.email,
                    avatar: user.avatar || '',
                    role: (user.role as any) || 'student',
                    isLoggedIn: true
                  } as UserProfileType}
                  onProfileClick={handleProfileClick}
                  onOrderHistoryClick={handleOrderHistoryClick}
                  onReviewsClick={handleReviewsClick}
                  onLogoutClick={handleLogoutClick}
                />
              ) : !isLoading && (
                <Link 
                  href="/login" 
                  className="text-primary text-sm font-semibold hover:bg-gray-100 rounded-md px-4 py-2 transition-colors duration-300"
                >
                  Login
                </Link>
              )}
              <CartButton />
            </div>
          </div>
        </div>
      </header>
      
      {/* Shopping Cart Modal */}
      <ShoppingCart />
    </>
  );
};
