"use client";

import { NavItem } from "@/components/atoms/NavItem";
import { UserProfile, CartButton, ShoppingCart } from "@/components/molecules";
import { UserProfile as UserProfileType } from "@/types";
import Link from "next/link";
import { useState } from "react";

export const Navigation = () => {
  const [isUserLoggedIn] = useState(false);
  // Mock user data - in a real app, this would come from authentication context
  const mockUser: UserProfileType = {
    id: "1",
    name: "John Doe",
    email: "john@example.com",
    avatar: "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=500&q=80",
    role: "student",
    isLoggedIn: true,
  };

  const handleProfileClick = () => {
    console.log("Profile clicked");
  };

  const handleOrderHistoryClick = () => {
    console.log("Order history clicked");
  };

  const handleReviewsClick = () => {
    console.log("Reviews clicked");
  };

  const handleLogoutClick = () => {
    console.log("Logout clicked");
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
              <NavItem href="/forum">Fórum</NavItem>
              <NavItem href="/loja">Loja</NavItem>
              <NavItem href="/#apoie">Apoie o DACC</NavItem>
            </nav>

            {/* User Actions */}
            <div className="flex items-center gap-4">
              {isUserLoggedIn ? (
              <UserProfile
                user={mockUser}
                onProfileClick={handleProfileClick}
                onOrderHistoryClick={handleOrderHistoryClick}
                onReviewsClick={handleReviewsClick}
                onLogoutClick={handleLogoutClick}
              />
              ) : (
                <Link href="/login" className="text-primary text-sm font-semibold hover:bg-gray-100 rounded-md px-4 py-2 transition-colors duration-300">Login</Link>
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