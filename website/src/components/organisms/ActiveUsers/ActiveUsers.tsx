'use client'

import { motion } from 'framer-motion'

export interface ActiveUser {
  id: string
  name: string
  avatar: string
  status: 'online' | 'away' | 'offline'
}

export interface ActiveUsersProps {
  users?: ActiveUser[]
  className?: string
}

export const ActiveUsers: React.FC<ActiveUsersProps> = ({
  users = [
    {
      id: '1',
      name: 'Alex Johnson',
      avatar: 'https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-4.0.3&auto=format&fit=crop&w=100&q=80',
      status: 'online'
    },
    {
      id: '2',
      name: 'Sarah Chen',
      avatar: 'https://images.unsplash.com/photo-1494790108755-2616b612b98c?ixlib=rb-4.0.3&auto=format&fit=crop&w=100&q=80',
      status: 'online'
    },
    {
      id: '3',
      name: 'Mike Rodriguez',
      avatar: 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?ixlib=rb-4.0.3&auto=format&fit=crop&w=100&q=80',
      status: 'away'
    },
    {
      id: '4',
      name: 'Emma Wilson',
      avatar: 'https://images.unsplash.com/photo-1611432579402-7037e3e2c1e4?ixlib=rb-4.0.3&auto=format&fit=crop&w=100&q=80',
      status: 'online'
    }
  ],
  className = ''
}) => {
  const getStatusColor = (status: string) => {
    switch (status) {
      case 'online':
        return 'text-green-600'
      case 'away':
        return 'text-yellow-600'
      case 'offline':
        return 'text-gray-400'
      default:
        return 'text-gray-400'
    }
  }

  const getStatusText = (status: string) => {
    return status.toUpperCase()
  }

  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5 }}
      className={`bg-white rounded-lg shadow-sm border border-gray-200 p-6 ${className}`}
    >
      {/* Header */}
      <h2 className="text-xl font-bold text-gray-900 mb-6">Active Users</h2>

      {/* Users List */}
      <div className="space-y-4">
        {users.map((user, index) => (
          <motion.div
            key={user.id}
            initial={{ opacity: 0, x: -10 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.3, delay: index * 0.1 }}
            className="flex items-center space-x-3"
          >
            {/* User Avatar */}
            <div className="relative">
              <img
                src={user.avatar}
                alt={user.name}
                className="w-10 h-10 rounded-full object-cover"
              />
              {/* Status Indicator */}
              <div className={`absolute -bottom-1 -right-1 w-3 h-3 rounded-full border-2 border-white ${
                user.status === 'online' ? 'bg-green-500' :
                user.status === 'away' ? 'bg-yellow-500' :
                'bg-gray-400'
              }`}></div>
            </div>

            {/* User Info */}
            <div className="flex-1 min-w-0">
              <p className="text-sm font-medium text-gray-900 truncate">
                {user.name}
              </p>
              <p className={`text-xs font-medium ${getStatusColor(user.status)}`}>
                {getStatusText(user.status)}
              </p>
            </div>
          </motion.div>
        ))}
      </div>

      {/* Show More Button (optional) */}
      {users.length > 4 && (
        <motion.button
          whileHover={{ scale: 1.02 }}
          whileTap={{ scale: 0.98 }}
          className="w-full mt-4 text-sm text-primary hover:text-primary-dark transition-colors text-center py-2"
        >
          View all active users
        </motion.button>
      )}
    </motion.div>
  )
} 