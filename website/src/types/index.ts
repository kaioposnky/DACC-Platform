import { Slide, SlideDetail } from './annoucementsSlide';

export type { Slide, SlideDetail };

export interface User {
  id: string;
  name: string;
  lastName?: string;
  ra?: string;
  email: string;
  course?: string;
  phone?: string;
  avatar: string;
  isActive?: boolean;
  isSubscribedToNews?: boolean;
  role: 'aluno' | 'diretor' | 'administrador';
  isLoggedIn?: boolean;
  createdAt?: string;
  updatedAt?: string;
}

export interface Post {
  id: string;
  title: string;
  content: string;
  authorId: string;
  createdAt: string;
  updatedAt: string;
  tags: string[];
}

export interface Comment {
  id: string;
  postId: string;
  authorId: string;
  content: string;
  createdAt: string;
}

export interface ApiResponse<T> {
  statusCode: number;
  success: boolean;
  code: string;
  message: string;
  data: T;
  details?: any;
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
  author?: User;
}

export interface Directorate {
  id: string;
  name: string;
  description?: string;
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
  department?: Directorate;
}

export interface News {
  id: string;
  title: string;
  description: string;
  content?: string;
  author?: User;
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
  imageUrl: string;
  userId: string | null;
  social: {
    linkedin?: string;
    github?: string;
    email?: string;
  };
  createdAt?: string;
  updatedAt?: string;
}

export interface ProductReview {
  id: string;
  userId: string;
  userName: string;
  userAvatar: string;
  productId: string;
  productVariationId: string;
  productName: string;
  productImage?: string;
  rating: number;
  title: string;
  comment: string;
  createdAt?: string;
  updatedAt?: string;
}

export interface ProductCategory {
  id: string;
  name: string;
}

export interface ProductSubcategory {
  id: string;
  name: string;
  categoryId: string;
}

export interface ProductSize {
  id: string;
  name: string;
}

export interface ProductColor {
  id: string;
  name: string;
}

export interface ProductSpecification {
  name: string;
  value: string;
}

export interface ProductVariationImage {
  id?: string;
  url: string;
  order: number;
  imageAlt?: string;
}

export interface ProductVariation {
  id: string;
  color: string;
  size: string;
  stock: number;
  sku: string;
  images: ProductVariationImage[];
}

export interface Product {
  id: string;
  name: string;
  description: string;
  detailedDescription?: string;
  perfectFor?: string[];
  price: number;
  originalPrice?: number | null;
  category: string;
  active: boolean;
  subcategory?: string;
  inStock: boolean;
  featured?: boolean;
  rating?: number;
  reviews?: number;
  reviewsList?: ProductReview[];
  specifications?: ProductSpecification[];

  // A fonte de verdade para Estoque, Imagens e Atributos são as variações.
  variations: ProductVariation[];

  // Campos de visualização (geralmente derivados das variações no backend ou frontend)
  image?: string; // Thumbnail principal
  stockCount?: number; // Soma de variations.stock

  // Campos legados ou de leitura
  images?: string[]; // Para compatibilidade
  sizes?: string[]; // Derivado: variations.map(v => v.size)
  colors?: string[]; // Derivado: variations.map(v => v.color)

  shippingInfo?: {
    freeShipping: boolean;
    estimatedDays: number;
    shippingCost?: number;
    returnPolicy: string;
    warranty?: string;
  };
  createdAt?: string;
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
  role?: 'aluno' | 'diretor' | 'administrador';
  isLoggedIn: boolean;
}

export interface UserStats {
  orders: number;
  reviews: number;
  registryDate: string;
}

export interface ProductBatchRequest {
  id: string;
  name: string;
  description?: string;
  detailedDescription?: string;
  category?: string;
  subcategory?: string;
  price: number;
  originalPrice?: number | null;
  featured?: boolean;
  active?: boolean;
  perfectFor?: string[];
  specifications?: SpecificationItemRequest[];
  shippingInfo?: ShippingInfoRequest;
  variations?: VariationUpdateRequest[];
}

export interface VariationUpdateRequest {
  id?: string;
  color: string;
  size: string;
  stock: number;
  sku?: string;
  images?: VariationImageRequest[];
}

export interface VariationImageRequest {
  id?: string;
  url: string;
  order: number;
  imageAlt?: string;
}

export interface SpecificationItemRequest {
  name: string;
  value: string;
}

export interface ShippingInfoRequest {
  freeShipping: boolean;
  estimatedDays: string;
  returnPolicy: string;
  warranty?: string;
}

export const ValidStatus = ['created', 'pending', 'approved', 'rejected', 'delivered', 'cancelled'];
export type OrderStatus = typeof ValidStatus[number];
export type PaymentMethod = 'venda física' | 'pix';

export interface OrderItem {
  id: string;
  orderId: string;
  productId: string;
  productVariationId: string;
  quantity: number;
  unitPrice: number;
  // Campos virtuais para facilitar o frontend (devem vir no join)
  productName: string;
  productImage: string;
  variationColor: string;
  variationSize: string;
}

export interface Coupon {
  id: string;
  code: string;
  discountType: 'porcentagem' | 'valor_fixo';
  value: number;
  expirationDate?: string;
  usageLimit?: number;
  currentUsage: number;
  active: boolean;
}

export interface Order {
  id: string;
  userId: string;
  orderDate: string;
  status: OrderStatus;
  mercadopagoPaymentId?: number;
  preferenceId?: string;
  paymentMethod?: PaymentMethod;
  totalAmount: number;
  cupomId?: string;

  // Campos virtuais
  items: OrderItem[];
  user: User; // Dados do usuário populados
  coupon?: Coupon;
}
