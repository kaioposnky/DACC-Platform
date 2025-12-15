'use client';

import { motion } from 'framer-motion';
import { Typography } from '@/components/atoms';
import {
  UserIcon,
  ShieldCheckIcon,
  CogIcon,
  BellIcon,
  ShoppingBagIcon,
  StarIcon,
  ShoppingCartIcon
} from '@heroicons/react/24/outline';
import Link from 'next/link';

export type SettingsSection = 'account' | 'security' | 'preferences' | 'notifications';

export interface QuickAction {
  id: string;
  title: string;
  subtitle: string;
  icon: React.ReactNode;
  onClick: () => void;
  link: string;
}

interface ProfileSettingsSidebarProps {
  activeSection: SettingsSection;
  onSectionChange: (section: SettingsSection) => void;
  quickActions?: QuickAction[];
  className?: string;
}

export const ProfileSettingsSidebar = ({
  activeSection,
  onSectionChange,
  quickActions,
  className = ''
}: ProfileSettingsSidebarProps) => {
  
  const navigationItems = [
    {
      id: 'account' as const,
      title: 'Configurações da Conta',
      icon: <UserIcon className="w-5 h-5" />
    },
    {
      id: 'security' as const,
      title: 'Segurança',
      icon: <ShieldCheckIcon className="w-5 h-5" />
    },
    {
      id: 'preferences' as const,
      title: 'Preferências',
      icon: <CogIcon className="w-5 h-5" />
    },
    {
      id: 'notifications' as const,
      title: 'Notificações',
      icon: <BellIcon className="w-5 h-5" />
    }
  ];

  const defaultQuickActions: QuickAction[] = [
    {
      id: 'order-history',
      title: 'Histórico de Pedidos',
      subtitle: 'Visualize seu histórico de compras',
      icon: <ShoppingBagIcon className="w-5 h-5 text-primary" />,
      onClick: () => console.log('Navigate to order history'),
      link: '/perfil/pedidos'
    },
    {
      id: 'my-reviews',
      title: 'Minhas Avaliações',
      subtitle: 'Gerencie suas avaliações de produtos',
      icon: <StarIcon className="w-5 h-5 text-primary" />,
      onClick: () => console.log('Navigate to reviews'),
      link: '/perfil/avaliacoes'
    },
    {
      id: 'continue-shopping',
      title: 'Continuar Comprando',
      subtitle: 'Navegue pelos nossos produtos',
      icon: <ShoppingCartIcon className="w-5 h-5 text-primary" />,
      onClick: () => console.log('Navigate to shop'),
      link: '/loja'
    }
  ];

  const actionsToShow = quickActions || defaultQuickActions;

  return (
    <motion.div
      initial={{ opacity: 0, x: -20 }}
      animate={{ opacity: 1, x: 0 }}
      transition={{ duration: 0.5 }}
      className={`bg-white rounded-lg shadow-sm border border-gray-200 p-6 h-fit ${className}`}
    >
      {/* Navigation Items */}
      <div className="space-y-2 mb-8">
        {navigationItems.map((item, index) => (
          <motion.button
            key={item.id}
            initial={{ opacity: 0, x: -20 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.3, delay: index * 0.1 }}
            onClick={() => onSectionChange(item.id)}
            className={`
              w-full flex items-center gap-3 px-4 py-3 rounded-lg text-left transition-all duration-200
              ${activeSection === item.id 
                ? 'bg-blue-900 text-white shadow-sm' 
                : 'text-gray-700 hover:bg-gray-50'
              }
            `}
          >
            <div className={activeSection === item.id ? 'text-white' : 'text-gray-500'}>
              {item.icon}
            </div>
            <Typography 
              variant="body" 
              className={`font-medium ${activeSection === item.id ? 'text-white' : 'text-gray-700'}`}
            >
              {item.title}
            </Typography>
          </motion.button>
        ))}
      </div>

      {/* Quick Actions */}
      <div>
        <Typography variant="h5" className="text-gray-900 font-semibold mb-4">
          Ações Rápidas
        </Typography>
        <div className="space-y-3">
          {actionsToShow.map((action, index) => (
            <motion.button
              key={action.id}
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.3, delay: 0.4 + index * 0.1 }}
              onClick={action.onClick}
              className="w-full p-4 border border-gray-200 rounded-lg hover:border-gray-300 hover:shadow-sm transition-all duration-200 text-left"
            >
              <Link href={action.link} className="flex items-start gap-3"> 
                <div className="flex-shrink-0 mt-1">
                  {action.icon}
                </div>
                <div className="flex-1 min-w-0">
                  <Typography variant="body" className="text-gray-900 font-medium mb-1">
                    {action.title}
                  </Typography>
                  <Typography variant="caption" className="text-gray-600">
                    {action.subtitle}
                  </Typography>
                </div>
              </Link>
            </motion.button>
          ))}
        </div>
      </div>
    </motion.div>
  );
}; 