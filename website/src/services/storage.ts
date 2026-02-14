import {User} from '@/types';

const STORAGE_KEYS = {
  ACCESS_TOKEN: 'dacc:accessToken',
  REFRESH_TOKEN: 'dacc:refreshToken',
  EXPIRES_IN: 'dacc:expiresIn',
  USER: 'dacc:user',
};

class StorageService {
  setTokens(accessToken: string, refreshToken: string, expiresIn: number) {
    if (typeof window === 'undefined') return;
    localStorage.setItem(STORAGE_KEYS.ACCESS_TOKEN, accessToken);
    localStorage.setItem(STORAGE_KEYS.REFRESH_TOKEN, refreshToken);
    localStorage.setItem(STORAGE_KEYS.EXPIRES_IN, expiresIn.toString());
  }

  getAccessToken() {
    if (typeof window === 'undefined') return null;
    return localStorage.getItem(STORAGE_KEYS.ACCESS_TOKEN);
  }

  getRefreshToken() {
    if (typeof window === 'undefined') return null;
    return localStorage.getItem(STORAGE_KEYS.REFRESH_TOKEN);
  }

  getTokenExpiration() {
    if (typeof window === 'undefined') return null;
    return Number(localStorage.getItem(STORAGE_KEYS.EXPIRES_IN));
  }

  setUser(user: User) {
    if (typeof window === 'undefined') return;
    localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(user));
  }

  getUser(): User | null {
    if (typeof window === 'undefined') return null;
    const userStr = localStorage.getItem(STORAGE_KEYS.USER);
    try {
      return userStr ? JSON.parse(userStr) : null;
    } catch {
      return null;
    }
  }

  clear() {
    if (typeof window === 'undefined') return;
    localStorage.removeItem(STORAGE_KEYS.ACCESS_TOKEN);
    localStorage.removeItem(STORAGE_KEYS.REFRESH_TOKEN);
    localStorage.removeItem(STORAGE_KEYS.EXPIRES_IN);
    localStorage.removeItem(STORAGE_KEYS.USER);
  }
}

export const storageService = new StorageService();