'use client'

import { motion } from 'framer-motion'
import { CalendarIcon, ClockIcon, UserIcon, TagIcon, ChevronRightIcon } from '@heroicons/react/24/outline'
import Link from 'next/link'

export interface NewsArticleBannerProps {
  title: string
  description: string
  image: string
  author: string
  date: string
  readTime: number
  category: string
  tags: string[]
  className?: string
}

export const NewsArticleBanner: React.FC<NewsArticleBannerProps> = ({
  title,
  description,
  image,
  author,
  date,
  readTime,
  category,
  tags,
  className = ''
}) => {
  return (
    <div className={`relative bg-gray-900 ${className}`}>
      {/* Background Image */}
      <div className="absolute inset-0">
        <img
          src={image}
          alt={title}
          className="w-full h-full object-cover"
        />
        <div className="absolute inset-0 bg-gray-900/70"></div>
      </div>

      {/* Content */}
      <div className="relative max-w-6xl mx-auto px-4 sm:px-6 lg:px-8 py-16 md:py-24">
        {/* Breadcrumb */}
        <motion.nav
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5 }}
          className="flex items-center space-x-2 text-sm mb-8"
        >
          <Link 
            href="/" 
            className="text-gray-300 hover:text-white transition-colors"
          >
            Início
          </Link>
          <ChevronRightIcon className="h-4 w-4 text-gray-400" />
          <Link 
            href="/noticias" 
            className="text-gray-300 hover:text-white transition-colors"
          >
            Notícias
          </Link>
          <ChevronRightIcon className="h-4 w-4 text-gray-400" />
          <span className="text-white">{category}</span>
        </motion.nav>

        {/* Article Header */}
        <div className="space-y-6">
          {/* Category Badge */}
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.1 }}
          >
            <span className="inline-block bg-primary text-white px-3 py-1 rounded-full text-sm font-medium">
              {category}
            </span>
          </motion.div>

          {/* Title */}
          <motion.h1
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.2 }}
            className="text-4xl md:text-5xl lg:text-6xl font-bold text-white leading-tight"
          >
            {title}
          </motion.h1>

          {/* Description */}
          <motion.p
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.3 }}
            className="text-xl text-gray-300 max-w-3xl leading-relaxed"
          >
            {description}
          </motion.p>

          {/* Metadata */}
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.4 }}
            className="flex flex-wrap items-center gap-6 text-gray-300"
          >
            {/* Author */}
            <div className="flex items-center space-x-2">
              <UserIcon className="h-5 w-5" />
              <span className="text-sm">{author}</span>
            </div>

            {/* Date */}
            <div className="flex items-center space-x-2">
              <CalendarIcon className="h-5 w-5" />
              <span className="text-sm">{date}</span>
            </div>

            {/* Read Time */}
            <div className="flex items-center space-x-2">
              <ClockIcon className="h-5 w-5" />
              <span className="text-sm">{readTime} min de leitura</span>
            </div>
          </motion.div>

          {/* Tags */}
          {tags && tags.length > 0 && (
            <motion.div
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.5, delay: 0.5 }}
              className="flex items-center space-x-2"
            >
              <TagIcon className="h-5 w-5 text-gray-400" />
              <div className="flex flex-wrap gap-2">
                {tags.map((tag, index) => (
                  <span
                    key={index}
                    className="bg-white/10 backdrop-blur-sm text-white px-3 py-1 rounded-full text-xs font-medium"
                  >
                    #{tag}
                  </span>
                ))}
              </div>
            </motion.div>
          )}
        </div>
      </div>
    </div>
  )
} 