import { User, Post, Comment, Announcement, Event, Project, News, Faculty, Product } from '@/types';

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

const API_BASE_URL = 'http://localhost:3001';

class ApiService {
  private async request<T>(endpoint: string, options?: RequestInit): Promise<T> {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      headers: {
        'Content-Type': 'application/json',
        ...options?.headers,
      },
      ...options,
    });

    if (!response.ok) {
      throw new Error(`API request failed: ${response.statusText}`);
    }

    return response.json();
  }

  // Users
  async getUsers(): Promise<User[]> {
    return this.request<User[]>('/users');
  }

  async getUser(id: number): Promise<User> {
    return this.request<User>(`/users/${id}`);
  }

  async createUser(user: Omit<User, 'id'>): Promise<User> {
    return this.request<User>('/users', {
      method: 'POST',
      body: JSON.stringify(user),
    });
  }

  async updateUser(id: number, user: Partial<User>): Promise<User> {
    return this.request<User>(`/users/${id}`, {
      method: 'PATCH',
      body: JSON.stringify(user),
    });
  }

  async deleteUser(id: number): Promise<void> {
    return this.request<void>(`/users/${id}`, {
      method: 'DELETE',
    });
  }

  // Posts
  async getPosts(): Promise<Post[]> {
    return this.request<Post[]>('/posts');
  }

  async getPost(id: number): Promise<Post> {
    return this.request<Post>(`/posts/${id}`);
  }

  async createPost(post: Omit<Post, 'id'>): Promise<Post> {
    return this.request<Post>('/posts', {
      method: 'POST',
      body: JSON.stringify(post),
    });
  }

  async updatePost(id: number, post: Partial<Post>): Promise<Post> {
    return this.request<Post>(`/posts/${id}`, {
      method: 'PATCH',
      body: JSON.stringify(post),
    });
  }

  async deletePost(id: number): Promise<void> {
    return this.request<void>(`/posts/${id}`, {
      method: 'DELETE',
    });
  }

  // Comments
  async getComments(): Promise<Comment[]> {
    return this.request<Comment[]>('/comments');
  }

  async getComment(id: number): Promise<Comment> {
    return this.request<Comment>(`/comments/${id}`);
  }

  async getCommentsByPost(postId: number): Promise<Comment[]> {
    return this.request<Comment[]>(`/comments?postId=${postId}`);
  }

  async createComment(comment: Omit<Comment, 'id'>): Promise<Comment> {
    return this.request<Comment>('/comments', {
      method: 'POST',
      body: JSON.stringify(comment),
    });
  }

  async updateComment(id: number, comment: Partial<Comment>): Promise<Comment> {
    return this.request<Comment>(`/comments/${id}`, {
      method: 'PATCH',
      body: JSON.stringify(comment),
    });
  }

  async deleteComment(id: number): Promise<void> {
    return this.request<void>(`/comments/${id}`, {
      method: 'DELETE',
    });
  }

  // Announcements
  async getAnnouncements(): Promise<Announcement[]> {
    return this.request<Announcement[]>('/announcements');
  }

  // Events
  async getEvents(): Promise<Event[]> {
    return this.request<Event[]>('/events');
  }

  async getEvent(id: string): Promise<Event> {
    return this.request<Event>(`/events/${id}`);
  }

  // Projects
  async getProjects(): Promise<Project[]> {
    return this.request<Project[]>('/projects');
  }

  async getProject(id: string): Promise<Project> {
    return this.request<Project>(`/projects/${id}`);
  }

  // News
  async getNews(): Promise<News[]> {
    return this.request<News[]>('/news');
  }

  async getNewsItem(id: string): Promise<News> {
    return this.request<News>(`/news/${id}`);
  }

  // Faculty
  async getFaculty(): Promise<Faculty[]> {
    return this.request<Faculty[]>('/faculty');
  }

  async getFacultyMember(id: string): Promise<Faculty> {
    return this.request<Faculty>(`/faculty/${id}`);
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
      searchParams.append('category', params.category);
    }
    
    if (params?.search) {
      searchParams.append('name_like', params.search);
    }
    
    if (params?.page && params?.limit) {
      searchParams.append('_page', params.page.toString());
      searchParams.append('_limit', params.limit.toString());
    }
    
    // Handle sorting
    if (params?.sortBy) {
      switch (params.sortBy) {
        case 'price-low':
          searchParams.append('_sort', 'price');
          searchParams.append('_order', 'asc');
          break;
        case 'price-high':
          searchParams.append('_sort', 'price');
          searchParams.append('_order', 'desc');
          break;
        case 'newest':
          searchParams.append('_sort', 'createdAt');
          searchParams.append('_order', 'desc');
          break;
        case 'name-az':
          searchParams.append('_sort', 'name');
          searchParams.append('_order', 'asc');
          break;
        case 'popular':
          searchParams.append('_sort', 'reviews');
          searchParams.append('_order', 'desc');
          break;
        case 'featured':
        default:
          searchParams.append('_sort', 'featured,createdAt');
          searchParams.append('_order', 'desc,desc');
          break;
      }
    }
    
    const query = searchParams.toString();
    const endpoint = query ? `/products?${query}` : '/products';
    
    return this.request<Product[]>(endpoint);
  }

  async getProduct(id: string): Promise<Product> {
    return this.request<Product>(`/products/${id}`);
  }

  // Forum Categories
  async getForumCategories(): Promise<ForumCategory[]> {
    return this.request<ForumCategory[]>('/forumCategories');
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
    
    return this.request<ForumThread[]>(endpoint);
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