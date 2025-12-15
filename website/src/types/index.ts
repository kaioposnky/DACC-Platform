import { Slide, SlideDetail } from './annoucementsSlide';

export type { Slide, SlideDetail};

export interface User {
  id: number;
  name: string;
  email: string;
  avatar: string;
  createdAt: string;
}

export interface Post {
  id: number;
  title: string;
  content: string;
  authorId: number;
  createdAt: string;
  updatedAt: string;
  tags: string[];
}

export interface Comment {
  id: number;
  postId: number;
  authorId: number;
  content: string;
  createdAt: string;
}

export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
} 

export interface Announcement {
  id: string;
  type: string;
  title: string;
  content: string;
  icon: string;
  details: { icon: string; text: string }[];
  primaryButtonText: string;
  secondaryButtonText: string;
  primaryButtonLink: string;
  secondaryButtonLink: string;
  imageSrc: string;
  imageAlt: string;
  createdAt: string;
}

export interface Event {
  id: string;
  title: string;
  description: string;
  date: string;
  time: string;
  actionText: string;
  actionLink: string;
  type: string;
}

export interface Project {
  id: string;
  title: string;
  description: string;
  icon: string;
  technologies: string[];
  status: 'in_progress' | 'completed' | 'planned';
  progress: number;
  completionText: string;
}

export interface News {
  id: string;
  title: string;
  description: string;
  content?: string;
  author?: string;
  readTime?: number;
  image?: string;
  tags?: string[];
  date: string;
  category: string;
  icon: string;
  gradient: string;
  readMoreLink: string;
}

export interface StatData {
  id: string;
  number: string;
  label: string;
  prefix?: string;
  suffix?: string;
  animateNumber?: boolean;
}

export interface Faculty {
  id: string;
  name: string;
  title: string;
  position: string;
  specialization: string;
  image: string;
  social: {
    linkedin?: string;
    github?: string;
    email?: string;
  };
}

export interface ProductReview {
  id: string;
  userId: string;
  userName: string;
  userAvatar: string;
  rating: number;
  title: string;
  comment: string;
  date: string;
  verified: boolean;
  helpful: number;
}

export interface ProductSpecification {
  name: string;
  value: string;
}

export interface Product {
  id: string;
  name: string;
  description: string;
  detailedDescription?: string;
  perfectFor?: string[];
  price: number;
  originalPrice?: number | null;
  category: 'tshirts' | 'hoodies' | 'cups' | 'stickers' | 'accessories';
  inStock: boolean;
  stockCount: number;
  image: string;
  images: string[];
  sizes: string[];
  colors: string[];
  featured: boolean;
  rating: number;
  reviews: number;
  reviewsList?: ProductReview[];
  specifications?: ProductSpecification[];
  shippingInfo?: {
    freeShipping: boolean;
    estimatedDays: string;
    shippingCost?: number;
    returnPolicy: string;
    warranty?: string;
  };
  createdAt: string;
}

export interface CartItem {
  id: string;
  productId: string;
  name: string;
  price: number;
  image: string;
  quantity: number;
  selectedSize?: string;
  selectedColor?: string;
}

export interface CartState {
  items: CartItem[];
  totalItems: number;
  totalAmount: number;
  shippingCost: number;
  subtotal: number;
  isOpen: boolean;
}

export interface UserProfile {
  id: string;
  name: string;
  email: string;
  avatar: string;
  role?: 'student' | 'faculty' | 'admin';
  isLoggedIn: boolean;
}