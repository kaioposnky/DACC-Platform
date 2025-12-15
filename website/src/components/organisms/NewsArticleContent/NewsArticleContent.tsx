'use client'

import { motion } from 'framer-motion'
import { ShareIcon, HeartIcon, BookmarkIcon } from '@heroicons/react/24/outline'
import { HeartIcon as HeartSolidIcon, BookmarkIcon as BookmarkSolidIcon } from '@heroicons/react/24/solid'
import { useState } from 'react'

export interface NewsArticleContentProps {
  content: string
  className?: string
}

export const NewsArticleContent: React.FC<NewsArticleContentProps> = ({
  content,
  className = ''
}) => {
  const [isLiked, setIsLiked] = useState(false)
  const [isBookmarked, setIsBookmarked] = useState(false)

  const handleShare = async () => {
    if (navigator.share) {
      try {
        await navigator.share({
          title: document.title,
          url: window.location.href,
        })
      } catch (error) {
        console.log('Error sharing:', error)
      }
    } else {
      // Fallback: copy to clipboard
      navigator.clipboard.writeText(window.location.href)
      alert('Link copiado para a área de transferência!')
    }
  }

  // Split content into paragraphs
  const paragraphs = content.split('\n\n').filter(p => p.trim())

  return (
    <div className={`max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-12 ${className}`}>
      <div className="bg-white rounded-lg shadow-sm border border-gray-200 overflow-hidden">
        {/* Article Actions */}
        <div className="border-b border-gray-200 px-8 py-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center space-x-4">
              <motion.button
                whileHover={{ scale: 1.05 }}
                whileTap={{ scale: 0.95 }}
                onClick={() => setIsLiked(!isLiked)}
                className={`flex items-center space-x-2 px-4 py-2 rounded-lg transition-colors ${
                  isLiked
                    ? 'bg-red-50 text-red-600'
                    : 'text-gray-600 hover:bg-gray-50'
                }`}
              >
                {isLiked ? (
                  <HeartSolidIcon className="h-5 w-5" />
                ) : (
                  <HeartIcon className="h-5 w-5" />
                )}
                <span className="text-sm font-medium">
                  {isLiked ? 'Curtido' : 'Curtir'}
                </span>
              </motion.button>

              <motion.button
                whileHover={{ scale: 1.05 }}
                whileTap={{ scale: 0.95 }}
                onClick={() => setIsBookmarked(!isBookmarked)}
                className={`flex items-center space-x-2 px-4 py-2 rounded-lg transition-colors ${
                  isBookmarked
                    ? 'bg-blue-50 text-blue-600'
                    : 'text-gray-600 hover:bg-gray-50'
                }`}
              >
                {isBookmarked ? (
                  <BookmarkSolidIcon className="h-5 w-5" />
                ) : (
                  <BookmarkIcon className="h-5 w-5" />
                )}
                <span className="text-sm font-medium">
                  {isBookmarked ? 'Salvo' : 'Salvar'}
                </span>
              </motion.button>
            </div>

            <motion.button
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.95 }}
              onClick={handleShare}
              className="flex items-center space-x-2 px-4 py-2 rounded-lg text-gray-600 hover:bg-gray-50 transition-colors"
            >
              <ShareIcon className="h-5 w-5" />
              <span className="text-sm font-medium">Compartilhar</span>
            </motion.button>
          </div>
        </div>

        {/* Article Content */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5, delay: 0.2 }}
          className="px-8 py-8"
        >
          <div className="prose prose-lg prose-gray max-w-none">
            {paragraphs.map((paragraph, index) => {
              // Check if paragraph is a list item
              if (paragraph.startsWith('•')) {
                const listItems = paragraph.split('\n').filter(item => item.trim())
                return (
                  <ul key={index} className="list-disc list-inside space-y-2 my-6">
                    {listItems.map((item, itemIndex) => (
                      <li key={itemIndex} className="text-gray-700 leading-relaxed">
                        {item.replace('•', '').trim()}
                      </li>
                    ))}
                  </ul>
                )
              }

              // Check if paragraph is a quote
              if (paragraph.startsWith('"') && paragraph.endsWith('"')) {
                return (
                  <blockquote 
                    key={index} 
                    className="border-l-4 border-primary pl-6 py-4 my-8 bg-gray-50 rounded-r-lg"
                  >
                    <p className="text-gray-800 italic text-lg leading-relaxed">
                      {paragraph}
                    </p>
                  </blockquote>
                )
              }

              // Regular paragraph
              return (
                <p 
                  key={index} 
                  className="text-gray-700 leading-relaxed mb-6 text-lg"
                >
                  {paragraph}
                </p>
              )
            })}
          </div>
        </motion.div>

        {/* Article Footer */}
        <div className="border-t border-gray-200 px-8 py-6 bg-gray-50">
          <div className="flex items-center justify-between">
            <div className="flex items-center space-x-4">
              <span className="text-sm text-gray-600">Gostou desta notícia?</span>
              <div className="flex items-center space-x-2">
                <motion.button
                  whileHover={{ scale: 1.05 }}
                  whileTap={{ scale: 0.95 }}
                  onClick={() => setIsLiked(!isLiked)}
                  className={`px-3 py-1 rounded-full text-xs font-medium transition-colors ${
                    isLiked
                      ? 'bg-red-100 text-red-700'
                      : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
                  }`}
                >
                  👍 {isLiked ? 'Curtido' : 'Curtir'}
                </motion.button>
              </div>
            </div>

            <div className="text-xs text-gray-500">
              Última atualização: {new Date().toLocaleDateString('pt-BR')}
            </div>
          </div>
        </div>
      </div>
    </div>
  )
} 