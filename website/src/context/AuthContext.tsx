'use client';

import { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import { User } from '@/types';
import { apiService } from '@/services/api';
import { storageService } from '@/services/storage';
import { useRouter } from 'next/navigation';
import { toast } from 'sonner';

interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (email: string, senha: string) => Promise<void>;
  register: (formData: RegisterData) => Promise<void>;
  logout: () => void;
}

export interface RegisterData {
  nome: string;
  sobrenome: string;
  email: string;
  senha: string;
  ra: string;
  telefone: string;
  curso: string;
  inscritoNoticia: boolean;
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

  const login = async (email: string, senha: string) => {
    try {
      const response = await apiService.login({ email, senha });
      
      storageService.setTokens(response.accessToken, response.refreshToken);
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
    try{
      await apiService.register(formData);

      setTimeout(() => {
        router.push('/login');
      }, 2000);
    } catch (error: any){
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