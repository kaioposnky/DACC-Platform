"use client";

import { useState, useEffect } from 'react';
import { Navigation, Footer } from "@/components";
import { NewsArticles, PageBanner, NewsletterWidget } from "@/components/organisms";
import { NewsFilter, type FilterOptions } from "@/components/molecules";

export default function Noticias() {
  const [searchQuery, setSearchQuery] = useState('');
  const [filters, setFilters] = useState<FilterOptions>({
    category: 'all',
    date: 'all',
    sortBy: 'latest',
    viewMode: 'grid'
  });
  const [showWidget, setShowWidget] = useState(false);

  // Show widget when page loads
  useEffect(() => {
    // Add a small delay to ensure page is fully loaded
    const timer = setTimeout(() => {
      setShowWidget(true);
    }, 1500);

    return () => clearTimeout(timer);
  }, []);

  const handleSearch = (query: string) => {
    setSearchQuery(query);
  };

  const handleFilterChange = (newFilters: FilterOptions) => {
    setFilters(newFilters);
  };

  const handleWidgetClose = () => {
    setShowWidget(false);
  };

  const handleNewsletterSubmit = async (email: string) => {
    // Here you would typically send the email to your newsletter service
    console.log('Newsletter subscription:', email);
    
    // You could also send this to an API endpoint
    // await fetch('/api/newsletter', {
    //   method: 'POST',
    //   headers: { 'Content-Type': 'application/json' },
    //   body: JSON.stringify({ email })
    // });
  };

  return (
    <>
      <Navigation />
      
      <PageBanner
        title="Notícias"
        subtitle="Fique por dentro das últimas notícias do DACC"
        showSearch={true}
        searchPlaceholder="Pesquisar notícias..."
        onSearch={handleSearch}
        backgroundColor="primary"
        showDecorations={true}
      />
      
      <NewsFilter onFilterChange={handleFilterChange} />
      
      <NewsArticles 
        filters={filters}
        searchQuery={searchQuery}
      />
      
      <Footer />

      {/* Newsletter Widget */}
      <NewsletterWidget
        isVisible={showWidget}
        onClose={handleWidgetClose}
        onNewsletterSubmit={handleNewsletterSubmit}
      />
    </>
  );
}   