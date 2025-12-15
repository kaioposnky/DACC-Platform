'use client'

import { motion } from 'framer-motion'

export interface ForumRule {
  id: string
  text: string
  icon?: React.ReactNode
}

export interface ForumRulesProps {
  rules?: ForumRule[]
  className?: string
}

export const ForumRules: React.FC<ForumRulesProps> = ({
  rules = [
    {
      id: '1',
      text: 'Be respectful and professional'
    },
    {
      id: '2',
      text: 'No spam or self-promotion'
    },
    {
      id: '3',
      text: 'Use appropriate categories'
    },
    {
      id: '4',
      text: 'Search before posting'
    },
    {
      id: '5',
      text: 'Help others when you can'
    }
  ],
  className = ''
}) => {
  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5, delay: 0.2 }}
      className={`bg-white rounded-lg shadow-sm border border-gray-200 p-6 ${className}`}
    >
      {/* Header */}
      <h2 className="text-xl font-bold text-gray-900 mb-6">Forum Rules</h2>

      {/* Rules List */}
      <div className="space-y-3">
        {rules.map((rule, index) => (
          <motion.div
            key={rule.id}
            initial={{ opacity: 0, x: -10 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.3, delay: index * 0.1 }}
            className="flex items-start space-x-3"
          >
            {/* Bullet Point */}
            <div className="flex-shrink-0 mt-1">
              <div className="w-2 h-2 bg-primary rounded-full"></div>
            </div>

            {/* Rule Text */}
            <p className="text-sm text-gray-700 leading-relaxed">
              {rule.text}
            </p>
          </motion.div>
        ))}
      </div>

      {/* Footer Note */}
      <motion.div
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ duration: 0.3, delay: 0.8 }}
        className="mt-6 pt-4 border-t border-gray-200"
      >
        <p className="text-xs text-gray-500 text-center">
          Violation of these rules may result in content removal or account suspension.
        </p>
      </motion.div>
    </motion.div>
  )
} 