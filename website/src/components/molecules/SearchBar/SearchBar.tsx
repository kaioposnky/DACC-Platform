'use client';
import { useState } from 'react';
import { Input, Button } from '@/components/atoms';
import { motion } from 'framer-motion';

interface SearchBarProps {
  onSearch: (query: string) => void;
  placeholder?: string;
  className?: string;
}

export const SearchBar = ({
  onSearch,
  placeholder = 'Pesquisar...',
  className = ''
}: SearchBarProps) => {
  const [query, setQuery] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSearch(query);
  };

  return (
    <motion.div
      className={`bg-gray-50 border-b border-gray-200 py-8 ${className}`}
      initial={{ opacity: 0, y: -20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.6 }}
    >
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <form onSubmit={handleSubmit} className="flex flex-col sm:flex-row gap-4 items-end">
          <div className="flex-1 w-full">
            <Input
              type="text"
              label="Pesquisar"
              placeholder={placeholder}
              value={query}
              onChange={(e) => setQuery(e.target.value)}
              className="h-11 w-full"
            />
          </div>
          <Button type="submit" variant="primary" className="h-11 px-8">
            Pesquisar
          </Button>
        </form>
      </div>
    </motion.div>
  );
}; 