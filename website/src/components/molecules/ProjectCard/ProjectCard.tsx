"use client";

import { motion } from 'framer-motion';
import { Card, Typography } from '@/components/atoms';
import { Project } from '@/types';

interface ProjectCardProps {
  project: Project;
  className?: string;
}

export const ProjectCard = ({ project, className = '' }: ProjectCardProps) => {
  const getIcon = (iconName: string) => {
    switch (iconName) {
      case 'mobile':
        return (
          <svg className="w-15 h-15" fill="currentColor" viewBox="0 0 24 24">
            <path d="M16 1H8C6.34 1 5 2.34 5 4v16c0 1.66 1.34 3 3 3h8c1.66 0 3-1.34 3-3V4c0-1.66-1.34-3-3-3zm-2 20h-4v-1h4v1zm3.25-3H6.75V4h10.5v14z"/>
          </svg>
        );
      case 'robot':
        return (
          <svg className="w-15 h-15" fill="currentColor" viewBox="0 0 24 24">
            <path d="M12 2l3.09 6.26L22 9l-5 4.74L18.18 21L12 17.27L5.82 21L7 13.74L2 9l6.91-1.26L12 2zM12 15.4l.76 3.36 2.74-1.92-2.05-1.44H12zm0-6.8h1.45l2.05-1.44L12.76 5.8L12 8.6z"/>
          </svg>
        );
      case 'chart':
        return (
          <svg className="w-15 h-15" fill="currentColor" viewBox="0 0 24 24">
            <path d="M5 9.2h3V19H5V9.2zM10.6 5h2.8v14h-2.8V5zm5.6 8H19v6h-2.8v-6z"/>
          </svg>
        );
      default:
        return (
          <svg className="w-15 h-15" fill="currentColor" viewBox="0 0 24 24">
            <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/>
          </svg>
        );
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'completed':
        return 'bg-green-100 text-green-800';
      case 'in_progress':
        return 'bg-yellow-100 text-yellow-800';
      case 'planned':
        return 'bg-blue-100 text-blue-800';
      default:
        return 'bg-gray-100 text-gray-800';
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case 'completed':
        return 'Concluído';
      case 'in_progress':
        return 'Em Progresso';
      case 'planned':
        return 'Planejado';
      default:
        return 'Desconhecido';
    }
  };

  return (
    <motion.div
      className={className}
      initial={{ opacity: 0, y: 30 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.6, delay: 0.2 }}
    >
      <div className='shadow-lg rounded-lg hover:shadow-xl hover:scale-105 transition-all duration-500'>
        {/* Icon */}
          <div className="w-full h-40 rounded-t-lg bg-primary flex items-center justify-center text-white">
            {getIcon(project.icon)}
          </div>

      <Card className="p-6 h-full">
      

        {/* Title */}
        <Typography variant="h4" className="mb-3 text-primary font-bold">
          {project.title}
        </Typography>

        {/* Description */}
        <Typography variant="body" color="gray" className="mb-4 line-clamp-3">
          {project.description}
        </Typography>

        {/* Status */}
        <div className="mb-4">
          <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${getStatusColor(project.status)}`}>
            {getStatusText(project.status)}
          </span>
        </div>

        {/* Technologies */}
        <div className="mb-4">
          <div className="flex flex-wrap gap-2">
            {project.technologies.map((tech, index) => (
              <span
                key={index}
                className="px-3 py-1 bg-gray-200 text-gray-700 rounded-full text-sm"
              >
                {tech}
              </span>
            ))}
          </div>
        </div>
      </Card>
      </div>
    </motion.div>
  );
}; 