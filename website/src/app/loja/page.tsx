"use client";

import { useState } from 'react';
import { Navigation, Footer } from "@/components";
import { PageBanner, ProductGrid } from "@/components/organisms";
import { ShopFeatures, ProductFilter, type ProductFilterOptions } from "@/components/molecules";

export default function Loja() {
  const [filters, setFilters] = useState<ProductFilterOptions>({
    category: 'all',
    sortBy: 'featured',
    searchQuery: ''
  });

  const handleFilterChange = (newFilters: ProductFilterOptions) => {
    setFilters(newFilters);
    console.log('Product filters changed:', newFilters);
  };

  return (
    <div className="bg-gray-100">
      <Navigation />
      
      <PageBanner
        title="Coruja Overflow Shop"
        subtitle="Show your CS pride with our exclusive merchandise collection"
        showSearch={false}
        backgroundColor="gradient"
        showDecorations={true}
        className="pb-0"
      />
      
      <ShopFeatures />
      
      <ProductFilter onFilterChange={handleFilterChange} />
      
      {/* Product Grid */}
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12 ">
        <ProductGrid 
          filters={{
            category: filters.category === 'all' ? undefined : filters.category,
            sortBy: filters.sortBy,
            search: filters.searchQuery
          }}
        />
      </div>
      
      <Footer />
    </div>
  );
}