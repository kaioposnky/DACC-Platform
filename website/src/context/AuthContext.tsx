'use client';

import { createContext, ReactNode, useContext, useEffect, useState } from 'react';
import { User } from '@/types';
import { apiService } from '@/services/api';
import { storageService } from '@/services/storage';
import { useRouter } from 'next/navigation';

interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (email: string, password: string) => Promise<void>;
  register: (formData: RegisterData) => Promise<void>;
  logout: () => void;
}

export interface RegisterData {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  ra: string;
  phone: string;
  course: string;
  isSubscribedToNews: boolean;
  role?: string;
}

const AuthContext = createContext<AuthContextType>({} as AuthContextType);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const router = useRouter();

  useEffect(() => {
    // Hidrata o estado ao carregar a página
    const storedUser = storageService.getUser();
    const token = storageService.getAccessToken();

    if (storedUser && token) {
      setUser(storedUser);
    }
    setIsLoading(false);
  }, []);

  const login = async (email: string, password: string) => {
    try {
      const response = await apiService.login({ email, password });

      storageService.setTokens(response.accessToken, response.refreshToken, response.expiresIn);
      storageService.setUser(response.user);

      setUser(response.user);
      router.push('/');
    } catch (error: any) {
      throw error;
    }
  };

  const logout = () => {
    storageService.clear();
    setUser(null);
    router.push('/');
  };

  const registerUser = async (formData: RegisterData) => {
    try {
      await apiService.register(formData);

      setTimeout(() => {
        router.push('/login');
      }, 2000);
    } catch (error: any) {
      throw error;
    }
  }

  return (
    <AuthContext.Provider value={{ user, isAuthenticated: !!user, isLoading, login, register: registerUser, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);