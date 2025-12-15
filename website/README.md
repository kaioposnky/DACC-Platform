# Coruja Overflow

A Next.js project demonstrating **Atomic Design** methodology with **Tailwind CSS** and **JSON Server** for API mocking.

## 🚀 Features

- **Next.js 15** with App Router
- **TypeScript** for type safety
- **Tailwind CSS** for styling
- **Atomic Design** component architecture
- **JSON Server** for API mocking
- **Responsive Design** with modern UI components

## 🏗️ Project Structure

```
src/
├── app/                    # Next.js app directory
├── components/             # Atomic Design components
│   ├── atoms/             # Basic building blocks
│   │   ├── Button/        # Reusable button component
│   │   ├── Input/         # Form input component
│   │   └── Card/          # Card container component
│   ├── molecules/         # Simple combinations of atoms
│   │   ├── SearchBar/     # Search input with button
│   │   └── PostCard/      # Post display card
│   ├── organisms/         # Complex combinations
│   │   └── PostList/      # List of post cards
│   └── templates/         # Page layouts
│       └── MainLayout/    # Main page template
├── services/              # API service layer
│   └── api.ts            # JSON Server API client
├── types/                 # TypeScript type definitions
└── utils/                 # Utility functions
```

## 📚 Atomic Design Levels

### 🔹 Atoms
Basic building blocks that can't be broken down further:
- `Button` - Various button styles and states
- `Input` - Form inputs with validation
- `Card` - Container with different variants

### 🔹 Molecules
Simple combinations of atoms:
- `SearchBar` - Input + Button for searching
- `PostCard` - Card + Button + Text for post display

### 🔹 Organisms
Complex combinations of molecules and atoms:
- `PostList` - Grid of PostCard components with loading states

### 🔹 Templates
Page-level layouts:
- `MainLayout` - Header, main content, and footer structure

## 🛠️ Getting Started

### Prerequisites
- Node.js 18+ 
- npm or yarn

### Installation

1. **Clone or use this project structure**

2. **Install dependencies**:
   ```bash
   npm install
   ```

3. **Start the development servers**:
   
   **Option 1: Run both servers simultaneously**
   ```bash
   npm run dev:full
   ```
   
   **Option 2: Run servers separately**
   ```bash
   # Terminal 1: Next.js development server
   npm run dev
   
   # Terminal 2: JSON Server API mock
   npm run api
   ```

4. **Access the application**:
   - Frontend: http://localhost:3000
   - API: http://localhost:3001

## 📝 Available Scripts

- `npm run dev` - Start Next.js development server
- `npm run build` - Build for production
- `npm run start` - Start production server
- `npm run lint` - Run ESLint
- `npm run api` - Start JSON Server on port 3001
- `npm run dev:full` - Run both Next.js and JSON Server

## 🔌 API Endpoints

The JSON Server provides these endpoints:

- `GET /users` - Get all users
- `GET /users/:id` - Get user by ID
- `POST /users` - Create new user
- `PATCH /users/:id` - Update user
- `DELETE /users/:id` - Delete user

- `GET /posts` - Get all posts
- `GET /posts/:id` - Get post by ID
- `POST /posts` - Create new post
- `PATCH /posts/:id` - Update post
- `DELETE /posts/:id` - Delete post

- `GET /comments` - Get all comments
- `GET /comments?postId=:id` - Get comments for a post
- `POST /comments` - Create new comment
- `PATCH /comments/:id` - Update comment
- `DELETE /comments/:id` - Delete comment

## 🎨 Component Usage Examples

### Button Component
```tsx
import { Button } from '@/components/atoms';

<Button variant="primary" size="lg" onClick={handleClick}>
  Click me
</Button>
```

### SearchBar Component
```tsx
import { SearchBar } from '@/components/molecules';

<SearchBar
  onSearch={(query) => console.log(query)}
  placeholder="Search posts..."
/>
```

### PostList Component
```tsx
import { PostList } from '@/components/organisms';

<PostList
  posts={posts}
  onDeletePost={handleDelete}
  onPostClick={handleClick}
  loading={loading}
/>
```

## 🎯 Key Features Demonstrated

1. **Atomic Design**: Hierarchical component structure
2. **TypeScript**: Full type safety across the application
3. **Tailwind CSS**: Utility-first styling approach
4. **API Integration**: RESTful API consumption with error handling
5. **Responsive Design**: Mobile-first responsive layouts
6. **Loading States**: Skeleton loaders and loading indicators
7. **Error Handling**: User-friendly error messages

## 🔧 Customization

### Adding New Components

1. **Create atom components** in `src/components/atoms/`
2. **Combine atoms** into molecules in `src/components/molecules/`
3. **Build organisms** from molecules in `src/components/organisms/`
4. **Design templates** using organisms in `src/components/templates/`

### Modifying API Data

Edit `db.json` to change the mock data structure. The JSON Server will automatically reload.

### Styling

This project uses Tailwind CSS. Customize the theme in `tailwind.config.ts` or modify component styles directly.

## 📚 Learning Resources

- [Atomic Design Methodology](https://atomicdesign.bradfrost.com/)
- [Next.js Documentation](https://nextjs.org/docs)
- [Tailwind CSS Documentation](https://tailwindcss.com/docs)
- [JSON Server Documentation](https://github.com/typicode/json-server)

## 🚀 Deployment

1. **Build the project**:
   ```bash
   npm run build
   ```

2. **Start production server**:
   ```bash
   npm start
   ```

For production deployment, consider using platforms like Vercel, Netlify, or your preferred hosting provider.

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License.

---

Built with ❤️ using Next.js, Tailwind CSS, and Atomic Design principles.
