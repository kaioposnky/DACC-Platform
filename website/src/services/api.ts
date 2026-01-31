import {
  Announcement,
  ApiResponse,
  Comment,
  Event,
  Faculty,
  News,
  Post,
  Product,
  Project,
  ProductBatchUpdateRequest,
  User,
  UserStats
} from '@/types';
import { storageService } from "@/services/storage";
import { RegisterData } from "@/context/AuthContext";
import { toast } from "sonner";

// Forum types
export interface ForumCategory {
  id: string;
  name: string;
  threadCount: number;
  description: string;
}

export interface ForumThread {
  id: string;
  title: string;
  content: string;
  authorId: string;
  authorName: string;
  authorAvatar: string;
  categoryId: string;
  replies: number;
  views: number;
  lastActivity: string;
  createdAt: string;
  isPinned: boolean;
  isLocked: boolean;
  tags: string[];
}

export interface ResetPasswordData {
  token: string;
  newPassword: string;
}

export interface ChangePasswordData {
  currentPassword: string;
  newPassword: string;
}

const API_BASE_URL = 'http://localhost:3001/v1/api';

class ApiService {
  private async request<T>(endpoint: string, options?: RequestInit): Promise<T> {

    // If the token expired we use refresh token to get a new token
    const timeNowUnix = Math.floor(Date.now() / 1000);
    const tokenExpirationUnix = storageService.getTokenExpiration();

    // 30 seconds margin to the expiring token
    if (tokenExpirationUnix && timeNowUnix > (tokenExpirationUnix - 30)) {
      try {
        const refreshToken = storageService.getRefreshToken();
        if (refreshToken) {
          const tokens = await this.refreshToken(refreshToken);
          storageService.setTokens(tokens.accessToken, tokens.refreshToken, tokens.expiresIn);
        }
      } catch (e) {
        storageService.clear();
        console.info("Could not refresh user token! Redirecting user to login.");
        toast.message("Seu acesso expirou! Faça login novamente para continuar navegando.")
        window.location.href = '/login'
      }
    }

    const accessToken = storageService.getAccessToken();

    const headers: Record<string, string> = {
      ...options?.headers as Record<string, string>,
    }

    // Se o corpo não for FormData, adicionamos Content-Type application/json
    if (!(options?.body instanceof FormData)) {
      headers['Content-Type'] = 'application/json';
    }

    if (accessToken) headers["Authorization"] = `Bearer ${accessToken}`;

    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      ...options,
      headers: headers,
    });

    if (response.status === 204) {
      return null as T;
    }

    const data = await response.json();

    // Verifica se a resposta segue o padrão ApiResponse do backend
    if (data && typeof data === 'object' && 'success' in data) {
      const apiResponse = data as ApiResponse<T>;

      if (!apiResponse.success) {
        throw new Error(apiResponse.message || 'Erro desconhecido na API');
      }

      return apiResponse.data;
    }
    return data as T;
  }

  async refreshToken(refreshToken: string): Promise<{ accessToken: string; refreshToken: string; expiresIn: number; }> {
    const response = await fetch(`${API_BASE_URL}/auth/refresh`, {
      method: 'POST',
      body: JSON.stringify({ refreshToken }),
      headers: {
        'Content-Type': 'application/json'
      }
    });

    const data = await response.json();

    // Verifica se a resposta segue o padrão ApiResponse do backend
    if (data && typeof data === 'object' && 'success' in data) {
      const apiResponse = data as ApiResponse<{ accessToken: string; refreshToken: string; expiresIn: number; }>;

      if (!apiResponse.success) {
        throw new Error(apiResponse.message || 'Erro desconhecido na API');
      }

      return apiResponse.data;
    }

    return data as { accessToken: string; refreshToken: string; expiresIn: number; };
  }

  async login(credentials: { email: string; password: string }): Promise<{ accessToken: string; refreshToken: string; expiresIn: number; user: User }> {
    return this.request<{ accessToken: string; refreshToken: string; expiresIn: number; user: User }>('/auth/login', {
      method: 'POST',
      body: JSON.stringify(credentials),
    });
  }

  async register(userData: RegisterData): Promise<User> {
    return this.request<User>('/auth/register', {
      method: 'POST',
      body: JSON.stringify(userData),
    });
  }

  // Auth - Password Management
  async forgotPassword(email: string): Promise<void> {
    return this.request<void>('/auth/forgot-password', {
      method: 'POST',
      body: JSON.stringify({ email }),
    });
  }

  async validateResetToken(token: string): Promise<void> {
    return this.request<void>(`/auth/validate-reset-token?token=${token}`);
  }

  async resetPassword(data: ResetPasswordData): Promise<void> {
    return this.request<void>('/auth/reset-password', {
      method: 'POST',
      body: JSON.stringify(data),
    });
  }

  async changePassword(data: ChangePasswordData): Promise<void> {
    return this.request<void>('/auth/change-password', {
      method: 'POST',
      body: JSON.stringify(data),
    });
  }

  // Users
  async getUsers(): Promise<User[]> {
    const data = await this.request<{ users: User[] }>('/users');
    return data?.users || [];
  }

  async getUser(id: string): Promise<User> {
    const data = await this.request<{ user: User }>(`/users/${id}`);
    return data.user;
  }

  async getUserStats(id: string): Promise<UserStats> {
    return this.request<UserStats>(`/users/${id}/stats`);
  }

  async updateUser(id: string, user: Partial<User> | FormData): Promise<User> {
    const isFormData = user instanceof FormData;

    const data = await this.request<{ user: User }>(`/users/${id}`, {
      method: 'PATCH',
      body: isFormData ? user : JSON.stringify(user),
    });
    return data.user;
  }

  async deleteUser(id: string): Promise<void> {
    return this.request<void>(`/users/${id}`, {
      method: 'DELETE',
    });
  }

  // Posts
  async getPosts(): Promise<Post[]> {
    const data = await this.request<{ posts: Post[] }>('/posts');
    return data?.posts || [];
  }

  async getPost(id: string): Promise<Post> {
    const data = await this.request<{ post: Post }>(`/posts/${id}`);
    return data.post;
  }

  async createPost(post: Omit<Post, 'id'>): Promise<Post> {
    const data = await this.request<{ post: Post }>('/posts', {
      method: 'POST',
      body: JSON.stringify(post),
    });
    return data.post;
  }

  async updatePost(id: string, post: Partial<Post>): Promise<Post> {
    const data = await this.request<{ post: Post }>(`/posts/${id}`, {
      method: 'PATCH',
      body: JSON.stringify(post),
    });
    return data.post;
  }

  async deletePost(id: string): Promise<void> {
    return this.request<void>(`/posts/${id}`, {
      method: 'DELETE',
    });
  }

  // Comments
  async getComments(): Promise<Comment[]> {
    const data = await this.request<{ comments: Comment[] }>('/comments');
    return data?.comments || [];
  }

  async getComment(id: string): Promise<Comment> {
    const data = await this.request<{ comment: Comment }>(`/comments/${id}`);
    return data.comment;
  }

  async getCommentsByPost(postId: string): Promise<Comment[]> {
    const data = await this.request<{ comments: Comment[] }>(`/comments?postId=${postId}`);
    return data?.comments || [];
  }

  async createComment(comment: Omit<Comment, 'id'>): Promise<Comment> {
    const data = await this.request<{ comment: Comment }>('/comments', {
      method: 'POST',
      body: JSON.stringify(comment),
    });
    return data.comment;
  }

  async updateComment(id: string, comment: Partial<Comment>): Promise<Comment> {
    const data = await this.request<{ comment: Comment }>(`/comments/${id}`, {
      method: 'PATCH',
      body: JSON.stringify(comment),
    });
    return data.comment;
  }

  async deleteComment(id: string): Promise<void> {
    return this.request<void>(`/comments/${id}`, {
      method: 'DELETE',
    });
  }

  // Announcements
  async getAnnouncements(): Promise<Announcement[]> {
    const data = await this.request<{ announcements: Announcement[] }>('/announcements');
    return data?.announcements || [];
  }

  async getAnnouncement(id: string): Promise<Announcement> {
    const data = await this.request<{ announcement: Announcement }>(`/announcements/${id}`);
    return data.announcement;
  }

  async createAnnouncement(announcement: Omit<Announcement, 'id'>): Promise<Announcement> {
    const data = await this.request<{ announcement: Announcement }>('/announcements', {
      method: 'POST',
      body: JSON.stringify(announcement),
    });
    return data.announcement;
  }

  async updateAnnouncement(id: string, announcement: Partial<Announcement>): Promise<Announcement> {
    const data = await this.request<{ announcement: Announcement }>(`/announcements/${id}`, {
      method: 'PATCH',
      body: JSON.stringify(announcement),
    });
    return data.announcement;
  }

  async deleteAnnouncement(id: string): Promise<void> {
    return this.request<void>(`/announcements/${id}`, {
      method: 'DELETE',
    });
  }

  // Events
  async getEvents(): Promise<Event[]> {
    const data = await this.request<{ events: Event[] }>('/events');
    return data?.events || [];
  }

  async getEvent(id: string): Promise<Event> {
    const data = await this.request<{ event: Event }>(`/events/${id}`);
    return data.event;
  }

  async createEvent(event: Omit<Event, 'id'>): Promise<Event> {
    const data = await this.request<{ event: Event }>('/events', {
      method: 'POST',
      body: JSON.stringify(event),
    });
    return data.event;
  }

  async updateEvent(id: string, event: Partial<Event>): Promise<Event> {
    const data = await this.request<{ event: Event }>(`/events/${id}`, {
      method: 'PATCH',
      body: JSON.stringify(event),
    });
    return data.event;
  }

  async deleteEvent(id: string): Promise<void> {
    return this.request<void>(`/events/${id}`, {
      method: 'DELETE',
    });
  }

  // Projects
  async getProjects(): Promise<Project[]> {
    const data = await this.request<{ projects: Project[] }>('/projects');
    return data?.projects || [];
  }

  async getProject(id: string): Promise<Project> {
    const data = await this.request<{ project: Project }>(`/projects/${id}`);
    return data.project;
  }

  async createProject(project: Omit<Project, 'id'>): Promise<Project> {
    const data = await this.request<{ project: Project }>('/projects', {
      method: 'POST',
      body: JSON.stringify(project),
    });
    return data.project;
  }

  async updateProject(id: string, project: Partial<Project>): Promise<Project> {
    const data = await this.request<{ project: Project }>(`/projects/${id}`, {
      method: 'PATCH',
      body: JSON.stringify(project),
    });
    return data.project;
  }

  async deleteProject(id: string): Promise<void> {
    return this.request<void>(`/projects/${id}`, {
      method: 'DELETE',
    });
  }

  // News
  async getNews(): Promise<News[]> {
    const data = await this.request<{ news: News[] }>('/news');
    return data?.news || [];
  }

  async getNewsItem(id: string): Promise<News> {
    const data = await this.request<{ news: News }>(`/news/${id}`);
    return data.news;
  }

  async createNews(news: Omit<News, 'id'>): Promise<News> {
    const data = await this.request<{ news: News }>('/news', {
      method: 'POST',
      body: JSON.stringify(news),
    });
    return data.news;
  }

  async updateNews(id: string, news: Partial<News>): Promise<News> {
    const data = await this.request<{ news: News }>(`/news/${id}`, {
      method: 'PATCH',
      body: JSON.stringify(news),
    });
    return data.news;
  }

  async deleteNews(id: string): Promise<void> {
    return this.request<void>(`/news/${id}`, {
      method: 'DELETE',
    });
  }

  // Faculty
  async getFaculty(): Promise<Faculty[]> {
    const data = await this.request<{ faculty: Faculty[] }>('/faculty');
    return data?.faculty || [];
  }

  async getFacultyMember(id: string): Promise<Faculty> {
    const data = await this.request<{ faculty: Faculty }>(`/faculty/${id}`);
    return data.faculty;
  }

  async createFacultyMember(faculty: Omit<Faculty, 'id'>): Promise<Faculty> {
    const data = await this.request<{ faculty: Faculty }>('/faculty', {
      method: 'POST',
      body: JSON.stringify(faculty),
    });
    return data.faculty;
  }

  async updateFacultyMember(id: string, faculty: Partial<Faculty>): Promise<Faculty> {
    const data = await this.request<{ faculty: Faculty }>(`/faculty/${id}`, {
      method: 'PATCH',
      body: JSON.stringify(faculty),
    });
    return data.faculty;
  }

  async deleteFacultyMember(id: string): Promise<void> {
    return this.request<void>(`/faculty/${id}`, {
      method: 'DELETE',
    });
  }

  // Products
  async getProducts(params?: {
    category?: string;
    sortBy?: string;
    search?: string;
    page?: number;
    limit?: number;
  }): Promise<Product[]> {
    const searchParams = new URLSearchParams();

    if (params?.category && params.category !== 'all') {
      searchParams.append('category', params.category); // Case insensitive on backend
    }

    if (params?.search) {
      searchParams.append('SearchQuery', params.search);
    }

    if (params?.page && params?.limit) {
      searchParams.append('Page', params.page.toString());
      searchParams.append('Limit', params.limit.toString());
    }

    // Handle sorting
    if (params?.sortBy) {
      switch (params.sortBy) {
        case 'price-low':
          searchParams.append('OrderBy', 'price-low');
          break;
        case 'price-high':
          searchParams.append('OrderBy', 'price-high');
          break;
        case 'newest':
          searchParams.append('OrderBy', 'newest');
          break;
        case 'name-az':
          searchParams.append('OrderBy', 'name');
          break;
        case 'popular':
          // Backend might not support popular yet, mapping to newest or keeping logic if custom
          searchParams.append('OrderBy', 'newest');
          break;
        case 'featured':
        default:
          searchParams.append('OrderBy', 'newest');
          break;
      }
    }

    const query = searchParams.toString();
    const endpoint = query ? `/products?${query}` : '/products';

    const data = await this.request<{ products: Product[] }>(endpoint);
    return data?.products || [];
  }

  async getSubcategories(): Promise<{ id: string; name: string; categoryId: string }[]> {
    const response = await this.request<{ subcategories: { id: string; name: string; categoryId: string }[] }>('/products/subcategorias');
    return response.subcategories;
  }

  async getProduct(id: string): Promise<Product> {
    const data = await this.request<{ product: Product }>(`/products/${id}`);
    return data.product;
  }

  async createProduct(product: Omit<Product, 'id'> | FormData): Promise<Product> {
    const isFormData = product instanceof FormData;

    const data = await this.request<{ product: Product }>('/products', {
      method: 'POST',
      body: isFormData ? product : JSON.stringify(product),
    });
    return data.product;
  }

  async updateProduct(id: string, product: Partial<Product> | FormData): Promise<Product> {
    const isFormData = product instanceof FormData;

    const data = await this.request<{ product: Product }>(`/products/${id}`, {
      method: 'PATCH',
      body: isFormData ? product : JSON.stringify(product),
    });
    return data.product;
  }

  async updateProductFull(id: string, product: ProductBatchUpdateRequest): Promise<Product> {
    const data = await this.request<{ product: Product }>(`/products/${id}/batch-update`, {
      method: 'PATCH',
      body: JSON.stringify(product),
    });
    return data.product;
  }

  async deleteProduct(id: string): Promise<void> {
    return this.request<void>(`/products/${id}`, {
      method: 'DELETE',
    });
  }

  async uploadImage(file: File): Promise<{ url: string }> {
    const formData = new FormData();
    formData.append('file', file);

    return this.request<{ url: string }>('/filestorage/uploadImage', {
      method: 'POST',
      body: formData,
    });
  }

  // Forum Categories
  async getForumCategories(): Promise<ForumCategory[]> {
    return (await this.request<ForumCategory[]>('/forumCategories')) || [];
  }

  async getForumCategory(id: string): Promise<ForumCategory> {
    return this.request<ForumCategory>(`/forumCategories/${id}`);
  }

  // Forum Threads
  async getForumThreads(params?: {
    categoryId?: string;
    sortBy?: string;
    search?: string;
    page?: number;
    limit?: number;
  }): Promise<ForumThread[]> {
    const searchParams = new URLSearchParams();

    if (params?.categoryId && params.categoryId !== 'all') {
      searchParams.append('categoryId', params.categoryId);
    }

    if (params?.search) {
      searchParams.append('title_like', params.search);
    }

    if (params?.page && params?.limit) {
      searchParams.append('_page', params.page.toString());
      searchParams.append('_limit', params.limit.toString());
    }

    // Handle sorting
    if (params?.sortBy) {
      switch (params.sortBy) {
        case 'latest':
          searchParams.append('_sort', 'lastActivity');
          searchParams.append('_order', 'desc');
          break;
        case 'oldest':
          searchParams.append('_sort', 'createdAt');
          searchParams.append('_order', 'asc');
          break;
        case 'most_replies':
          searchParams.append('_sort', 'replies');
          searchParams.append('_order', 'desc');
          break;
        case 'most_views':
          searchParams.append('_sort', 'views');
          searchParams.append('_order', 'desc');
          break;
        case 'title':
          searchParams.append('_sort', 'title');
          searchParams.append('_order', 'asc');
          break;
        default:
          searchParams.append('_sort', 'isPinned,lastActivity');
          searchParams.append('_order', 'desc,desc');
          break;
      }
    } else {
      // Default sorting: pinned first, then by latest activity
      searchParams.append('_sort', 'isPinned,lastActivity');
      searchParams.append('_order', 'desc,desc');
    }

    const query = searchParams.toString();
    const endpoint = query ? `/forumThreads?${query}` : '/forumThreads';

    return (await this.request<ForumThread[]>(endpoint)) || [];
  }

  async getForumThread(id: string): Promise<ForumThread> {
    return this.request<ForumThread>(`/forumThreads/${id}`);
  }

  async createForumThread(thread: Omit<ForumThread, 'id' | 'replies' | 'views' | 'lastActivity' | 'createdAt'>): Promise<ForumThread> {
    const newThread = {
      ...thread,
      replies: 0,
      views: 0,
      lastActivity: new Date().toISOString(),
      createdAt: new Date().toISOString(),
    };

    return this.request<ForumThread>('/forumThreads', {
      method: 'POST',
      body: JSON.stringify(newThread),
    });
  }

  async updateForumThread(id: string, thread: Partial<ForumThread>): Promise<ForumThread> {
    return this.request<ForumThread>(`/forumThreads/${id}`, {
      method: 'PATCH',
      body: JSON.stringify(thread),
    });
  }

  async deleteForumThread(id: string): Promise<void> {
    return this.request<void>(`/forumThreads/${id}`, {
      method: 'DELETE',
    });
  }
}

export const apiService = new ApiService();
