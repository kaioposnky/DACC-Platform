'use client'

import { useState, useEffect } from 'react'
import { motion } from 'framer-motion'
import { 
  ChatBubbleLeftRightIcon,
  CodeBracketIcon,
  PresentationChartLineIcon,
  BriefcaseIcon,
  BookOpenIcon
} from '@heroicons/react/24/outline'
import { apiService, ForumCategory } from '@/services/api'

export interface ForumCategoriesProps {
  categories?: (ForumCategory & { icon?: React.ReactNode })[]
  selectedCategory?: string
  onCategorySelect?: (categoryId: string) => void
  className?: string
}

export const ForumCategories: React.FC<ForumCategoriesProps> = ({
  categories,
  selectedCategory = 'all',
  onCategorySelect,
  className = ''
}) => {
  const [activeCategory, setActiveCategory] = useState(selectedCategory)
  const [apiCategories, setApiCategories] = useState<ForumCategory[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  // Icon mapping for categories
  const getIconForCategory = (categoryId: string): React.ReactNode => {
    switch (categoryId) {
      case 'all':
        return <ChatBubbleLeftRightIcon className="h-5 w-5" />
      case 'programming':
        return <CodeBracketIcon className="h-5 w-5" />
      case 'projects':
        return <PresentationChartLineIcon className="h-5 w-5" />
      case 'career':
        return <BriefcaseIcon className="h-5 w-5" />
      case 'study':
        return <BookOpenIcon className="h-5 w-5" />
      default:
        return null
    }
  }

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        setLoading(true)
        const data = await apiService.getForumCategories()
        setApiCategories(data)
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to fetch categories')
      } finally {
        setLoading(false)
      }
    }

    if (!categories) {
      fetchCategories()
    }
  }, [categories])

  // Use provided categories or fetched categories
  const displayCategories = categories || apiCategories

  const handleCategoryClick = (categoryId: string) => {
    setActiveCategory(categoryId)
    onCategorySelect?.(categoryId)
  }

  if (loading) {
    return (
      <div className={`bg-white rounded-lg shadow-sm border border-gray-200 p-6 ${className}`}>
        <h2 className="text-xl font-bold text-gray-900 mb-6">Categories</h2>
        <div className="space-y-2">
          {[...Array(6)].map((_, index) => (
            <div key={index} className="animate-pulse">
              <div className="h-12 bg-gray-200 rounded-lg"></div>
            </div>
          ))}
        </div>
      </div>
    )
  }

  if (error) {
    return (
      <div className={`bg-white rounded-lg shadow-sm border border-gray-200 p-6 ${className}`}>
        <h2 className="text-xl font-bold text-gray-900 mb-6">Categories</h2>
        <div className="text-center py-8">
          <p className="text-red-600 mb-4">Error loading categories</p>
          <p className="text-gray-600 text-sm">{error}</p>
        </div>
      </div>
    )
  }

  return (
    <motion.div
      initial={{ opacity: 0, x: -20 }}
      animate={{ opacity: 1, x: 0 }}
      transition={{ duration: 0.5 }}
      className={`bg-white rounded-lg shadow-sm border border-gray-200 p-6 ${className}`}
    >
      {/* Categories Header */}
      <h2 className="text-xl font-bold text-gray-900 mb-6">Categories</h2>

      {/* Categories List */}
      <div className="space-y-2">
        {displayCategories.map((category, index) => {
          const categoryIcon = getIconForCategory(category.id)
          return (
            <motion.button
              key={category.id}
              initial={{ opacity: 0, y: 10 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.3, delay: index * 0.1 }}
              onClick={() => handleCategoryClick(category.id)}
              className={`w-full flex items-center justify-between p-3 rounded-lg transition-all duration-200 ${
                activeCategory === category.id
                  ? 'bg-primary text-white shadow-md'
                  : 'text-gray-700 hover:bg-gray-50'
              }`}
            >
              {/* Category Name and Icon */}
              <div className="flex items-center space-x-3">
                {categoryIcon && (
                  <div className={`${
                    activeCategory === category.id ? 'text-white' : 'text-gray-500'
                  }`}>
                    {categoryIcon}
                  </div>
                )}
                <span className="text-left font-medium">{category.name}</span>
              </div>

              {/* Thread Count */}
              <div className={`px-2 py-1 rounded-full text-xs font-medium ${
                activeCategory === category.id
                  ? 'bg-white/20 text-white'
                  : 'bg-gray-100 text-gray-600'
              }`}>
                {category.threadCount.toLocaleString()}
              </div>
            </motion.button>
          )
        })}
      </div>
    </motion.div>
  )
} 