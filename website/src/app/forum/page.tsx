'use client'

import { useState } from 'react'
import { Footer, Navigation } from "@/components"
import { 
  ForumBanner,
  ForumCategories,
  ForumThreadList,
  ActiveUsers,
  ForumRules
} from "@/components/organisms"

export default function ForumPage() {
  const [selectedCategory, setSelectedCategory] = useState('all')
  const [currentPage, setCurrentPage] = useState(1)
  const [sortBy, setSortBy] = useState('latest')

  const handleCategorySelect = (categoryId: string) => {
    setSelectedCategory(categoryId)
    setCurrentPage(1) // Reset to first page when category changes
  }

  const handleSortChange = (newSortBy: string) => {
    setSortBy(newSortBy)
    setCurrentPage(1) // Reset to first page when sort changes
  }

  const handleSearch = () => {
    setCurrentPage(1) // Reset to first page when search changes
  }

  const handlePageChange = (page: number) => {
    setCurrentPage(page)
  }

  const handleNewThread = () => {
    // Navigate to new thread creation page
    window.location.href = '/forum/new-thread';
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />
      
      <ForumBanner />
      
      {/* Main Content */}
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="grid grid-cols-1 lg:grid-cols-4 gap-8">
          {/* Left Sidebar - Categories */}
          <div className="lg:col-span-1 space-y-6">
            <ForumCategories
              selectedCategory={selectedCategory}
              onCategorySelect={handleCategorySelect}
            />
            <ActiveUsers />
            <ForumRules />
          </div>

          {/* Main Content - Thread List */}
          <div className="lg:col-span-3">
            <ForumThreadList
              selectedCategory={selectedCategory}
              currentPage={currentPage}
              sortBy={sortBy}
              onNewThread={handleNewThread}
              onSortChange={handleSortChange}
              onSearch={handleSearch}
              onPageChange={handlePageChange}
            />
          </div>

          {/* Right Sidebar - Active Users & Rules */}
          <div className="lg:col-span-1 space-y-6">
            
          </div>
        </div>
      </div>
      
      <Footer />
    </div>
  )
}