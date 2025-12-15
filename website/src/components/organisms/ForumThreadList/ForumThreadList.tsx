'use client'

import { useState, useEffect } from 'react'
import { motion } from 'framer-motion'
import { 
  PlusIcon,
  MagnifyingGlassIcon,
  ChevronDownIcon,
  ChevronLeftIcon,
  ChevronRightIcon
} from '@heroicons/react/24/outline'
import { apiService, ForumThread } from '@/services/api'

export interface ForumThreadListProps {
  threads?: ForumThread[]
  currentPage?: number
  totalPages?: number
  sortBy?: string
  selectedCategory?: string
  onNewThread?: () => void
  onSortChange?: (sortBy: string) => void
  onSearch?: (query: string) => void
  onPageChange?: (page: number) => void
  className?: string
}

export const ForumThreadList: React.FC<ForumThreadListProps> = ({
  threads,
  currentPage = 1,
  totalPages = 15,
  sortBy = 'latest',
  selectedCategory = 'all',
  onNewThread,
  onSortChange,
  onSearch,
  onPageChange,
  className = ''
}) => {
  const [searchQuery, setSearchQuery] = useState('')
  const [selectedSort, setSelectedSort] = useState(sortBy)
  const [apiThreads, setApiThreads] = useState<ForumThread[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const sortOptions = [
    { value: 'latest', label: 'Latest Activity' },
    { value: 'oldest', label: 'Oldest' },
    { value: 'most_replies', label: 'Most Replies' },
    { value: 'most_views', label: 'Most Views' },
    { value: 'title', label: 'Title A-Z' }
  ]

  useEffect(() => {
    const fetchThreads = async () => {
      try {
        setLoading(true)
        const data = await apiService.getForumThreads({
          categoryId: selectedCategory,
          sortBy: selectedSort,
          search: searchQuery,
          page: currentPage,
          limit: 10
        })
        setApiThreads(data)
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to fetch threads')
      } finally {
        setLoading(false)
      }
    }

    if (!threads) {
      fetchThreads()
    }
  }, [threads, selectedCategory, selectedSort, searchQuery, currentPage])

  // Use provided threads or fetched threads
  const displayThreads = threads || apiThreads

  const handleSearch = () => {
    onSearch?.(searchQuery)
  }

  const handleSortChange = (value: string) => {
    setSelectedSort(value)
    onSortChange?.(value)
  }

  const handlePageChange = (page: number) => {
    if (page >= 1 && page <= totalPages) {
      onPageChange?.(page)
    }
  }

  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
      className={`bg-white rounded-lg shadow-sm border border-gray-200 ${className}`}
    >
      {/* Header Controls */}
      <div className="p-6 border-b border-gray-200">
        <div className="flex flex-col md:flex-row gap-4 items-start md:items-center justify-between">
          {/* Left side - New Thread Button */}
          <motion.button
            whileHover={{ scale: 1.05 }}
            whileTap={{ scale: 0.95 }}
            onClick={onNewThread}
            className="bg-primary text-white px-4 py-2 rounded-md hover:bg-primary-dark transition-colors flex items-center space-x-2"
          >
            <PlusIcon className="h-5 w-5" />
            <span>New Thread</span>
          </motion.button>

          {/* Right side - Sort and Search */}
          <div className="flex flex-col sm:flex-row gap-4 items-start sm:items-center">
            {/* Sort Dropdown */}
            <div className="relative">
              <select
                value={selectedSort}
                onChange={(e) => handleSortChange(e.target.value)}
                className="appearance-none bg-white border border-gray-300 rounded-md px-4 py-2 pr-10 text-sm focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent cursor-pointer"
              >
                {sortOptions.map((option) => (
                  <option key={option.value} value={option.value}>
                    {option.label}
                  </option>
                ))}
              </select>
              <ChevronDownIcon className="absolute right-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400 pointer-events-none" />
            </div>

            {/* Search */}
            <div className="flex items-center space-x-2">
              <div className="relative">
                <input
                  type="text"
                  placeholder="Search threads..."
                  value={searchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                  className="w-64 px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent text-sm"
                  onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
                />
              </div>
              <motion.button
                whileHover={{ scale: 1.05 }}
                whileTap={{ scale: 0.95 }}
                onClick={handleSearch}
                className="bg-primary text-white px-4 py-2 rounded-md hover:bg-primary-dark transition-colors flex items-center"
              >
                <MagnifyingGlassIcon className="h-5 w-5" />
              </motion.button>
            </div>
          </div>
        </div>
      </div>

      {/* Thread List Content */}
      <div className="p-6">
        {loading ? (
          <div className="space-y-4">
            {[...Array(5)].map((_, index) => (
              <div key={index} className="animate-pulse">
                <div className="h-20 bg-gray-200 rounded-lg"></div>
              </div>
            ))}
          </div>
        ) : error ? (
          <div className="text-center py-12">
            <div className="text-red-600 mb-4">
              <svg className="h-16 w-16 mx-auto" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z" />
              </svg>
            </div>
            <h3 className="text-lg font-medium text-gray-900 mb-2">Error loading threads</h3>
            <p className="text-gray-600 mb-4">{error}</p>
            <motion.button
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.95 }}
              onClick={() => window.location.reload()}
              className="bg-primary text-white px-6 py-2 rounded-md hover:bg-primary-dark transition-colors"
            >
              Retry
            </motion.button>
          </div>
        ) : displayThreads.length === 0 ? (
          <div className="text-center py-12">
            <div className="text-gray-400 mb-4">
              <svg className="h-16 w-16 mx-auto" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M8 12h.01M12 12h.01M16 12h.01M21 12c0 4.418-4.03 8-9 8a9.863 9.863 0 01-4.255-.949L3 20l1.395-3.72C3.512 15.042 3 13.574 3 12c0-4.418 4.03-8 9-8s9 3.582 9 8z" />
              </svg>
            </div>
            <h3 className="text-lg font-medium text-gray-900 mb-2">No threads yet</h3>
            <p className="text-gray-600 mb-4">
              Be the first to start a discussion in this category.
            </p>
            <motion.button
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.95 }}
              onClick={onNewThread}
              className="bg-primary text-white px-6 py-2 rounded-md hover:bg-primary-dark transition-colors"
            >
              Create New Thread
            </motion.button>
          </div>
        ) : (
          <div className="space-y-4">
            {displayThreads.map((thread) => (
              <motion.div
                key={thread.id}
                initial={{ opacity: 0, y: 10 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.3 }}
                className="bg-gray-50 rounded-lg p-4 hover:bg-gray-100 transition-colors"
              >
                <div className="flex items-start justify-between">
                  <div className="flex-1">
                    <div className="flex items-center space-x-2 mb-2">
                      {thread.isPinned && (
                        <span className="inline-block w-2 h-2 bg-yellow-400 rounded-full"></span>
                      )}
                      <h3 className="text-lg font-medium text-gray-900 hover:text-primary cursor-pointer">
                        {thread.title}
                      </h3>
                      {thread.isLocked && (
                        <span className="text-gray-400 text-sm">🔒</span>
                      )}
                    </div>
                    <p className="text-gray-600 text-sm mb-2">
                      by {thread.authorName} • {new Date(thread.lastActivity).toLocaleDateString()}
                    </p>
                    <div className="flex items-center space-x-4 text-sm text-gray-500">
                      <span>{thread.replies} replies</span>
                      <span>{thread.views} views</span>
                    </div>
                  </div>
                  <img
                    src={thread.authorAvatar}
                    alt={thread.authorName}
                    className="w-10 h-10 rounded-full"
                  />
                </div>
              </motion.div>
            ))}
          </div>
        )}
      </div>

      {/* Pagination */}
      <div className="px-6 py-4 border-t border-gray-200">
        <div className="flex items-center justify-between">
          {/* Previous Button */}
          <motion.button
            whileHover={currentPage > 1 ? { scale: 1.05 } : {}}
            whileTap={currentPage > 1 ? { scale: 0.95 } : {}}
            onClick={() => handlePageChange(currentPage - 1)}
            disabled={currentPage === 1}
            className={`flex items-center space-x-2 px-4 py-2 rounded-md transition-colors ${
              currentPage === 1
                ? 'text-gray-400 cursor-not-allowed'
                : 'text-gray-700 hover:bg-gray-50'
            }`}
          >
            <ChevronLeftIcon className="h-5 w-5" />
            <span>Previous</span>
          </motion.button>

          {/* Page Info */}
          <div className="flex items-center space-x-2">
            <span className="text-sm text-gray-600">
              Page {currentPage} of {totalPages}
            </span>
          </div>

          {/* Next Button */}
          <motion.button
            whileHover={currentPage < totalPages ? { scale: 1.05 } : {}}
            whileTap={currentPage < totalPages ? { scale: 0.95 } : {}}
            onClick={() => handlePageChange(currentPage + 1)}
            disabled={currentPage === totalPages}
            className={`flex items-center space-x-2 px-4 py-2 rounded-md transition-colors ${
              currentPage === totalPages
                ? 'text-gray-400 cursor-not-allowed'
                : 'text-gray-700 hover:bg-gray-50'
            }`}
          >
            <span>Next</span>
            <ChevronRightIcon className="h-5 w-5" />
          </motion.button>
        </div>
      </div>
    </motion.div>
  )
} 