'use client'

import { motion } from 'framer-motion'

export interface ForumStats {
  members: number
  threads: number
  posts: number
  onlineNow: number
}

export interface ForumBannerProps {
  stats?: ForumStats
  className?: string
}

export const ForumBanner: React.FC<ForumBannerProps> = ({ 
  stats = {
    members: 2847,
    threads: 1234,
    posts: 15678,
    onlineNow: 156
  },
  className = '' 
}) => {
  const formatNumber = (num: number) => {
    return num.toLocaleString()
  }

  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
      className={`bg-primary text-white py-16 px-6 ${className}`}
    >
      <div className="max-w-6xl mx-auto text-center">
        {/* Title and Subtitle */}
        <div className="space-y-6 mb-12">
          <motion.h1
            initial={{ opacity: 0, y: -20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.2 }}
            className="text-4xl md:text-5xl font-bold"
          >
            Fórum
          </motion.h1>
          
          <motion.p
            initial={{ opacity: 0, y: -20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.4 }}
            className="text-lg text-blue-200 max-w-2xl mx-auto"
          >
            Conecte-se, discuta e compartilhe conhecimento com outros estudantes de ciência da computação
          </motion.p>
        </div>

        {/* Statistics */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5, delay: 0.6 }}
          className="grid grid-cols-2 md:grid-cols-4 gap-8"
        >
          {/* Members */}
          <div className="text-center">
            <div className="text-3xl md:text-4xl font-bold text-yellow-400 mb-2">
              {formatNumber(stats.members)}
            </div>
            <div className="text-sm text-blue-200 uppercase tracking-wide">
              Membros
            </div>
          </div>

          {/* Threads */}
          <div className="text-center">
            <div className="text-3xl md:text-4xl font-bold text-yellow-400 mb-2">
              {formatNumber(stats.threads)}
            </div>
            <div className="text-sm text-blue-200 uppercase tracking-wide">
              Tópicos
            </div>
          </div>

          {/* Posts */}
          <div className="text-center">
            <div className="text-3xl md:text-4xl font-bold text-yellow-400 mb-2">
              {formatNumber(stats.posts)}
            </div>
            <div className="text-sm text-blue-200 uppercase tracking-wide">
              Mensagens
            </div>
          </div>

          {/* Online Now */}
          <div className="text-center">
            <div className="text-3xl md:text-4xl font-bold text-yellow-400 mb-2">
              {formatNumber(stats.onlineNow)}
            </div>
            <div className="text-sm text-blue-200 uppercase tracking-wide">
              Online agora
            </div>
          </div>
        </motion.div>
      </div>
    </motion.div>
  )
} 