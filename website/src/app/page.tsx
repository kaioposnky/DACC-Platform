'use client';

import { useEffect, useState, useCallback } from 'react';
import { Navigation, FeaturesSection, EventCalendar, SupportSection, ProjectsSection, NewsSection, Footer } from '@/components/organisms';
import { HeroSlider } from '@/components/organisms';
import { apiService } from '@/services/api';
import { Announcement, Slide, SlideDetail } from '@/types';

export default function Home() {
  const [heroSlides, setHeroSlides] = useState<Slide[]>([{
    id: 0,
    title: 'Coruja Overflow o site oficial do DACC',
    content: 'Acompanhe as notícias, eventos e projetos do DACC',
    primaryButtonText: 'Sobre Nós',
    secondaryButtonText: 'Nossos Projetos',
    primaryButtonLink: '/sobre',
    secondaryButtonLink: '/sobre#projetos',
    type: 'code' as const,
  }]);

  const fetchAnnouncements = useCallback(async () => {
    try {
      const fetchedAnnouncements = await apiService.getAnnouncements();
      // Convert announcements to slides format
      const announcementSlides: Slide[] = fetchedAnnouncements.map((announcement: Announcement, index: number) => ({
        id: index + 1, // Ensure numeric ID
        title: announcement.title,
        content: announcement.content,
        type: announcement.type === 'event' ? 'event' : 'default' as const,
        icon: announcement.icon,
        details: announcement.details as SlideDetail[],
        primaryButtonText: announcement.primaryButtonText,
        secondaryButtonText: announcement.secondaryButtonText,
        primaryButtonLink: announcement.primaryButtonLink,
        secondaryButtonLink: announcement.secondaryButtonLink,
        imageSrc: announcement.imageSrc,
        imageAlt: announcement.imageAlt,
      }));
      setHeroSlides(prevSlides => [...prevSlides, ...announcementSlides]);
    } catch (err) {
      console.error('Error fetching announcements:', err);
    }
  }, []);

  useEffect(() => {
    fetchAnnouncements();
  }, [fetchAnnouncements]);

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header/Navigation */}
      <Navigation />
      
      {/* Hero Section - Full width below header */}
      <HeroSlider slides={heroSlides} />
      
      {/* Features Section */}
      <FeaturesSection />
      
      {/* Event Calendar Section */}
      <EventCalendar />
      
      {/* Support Section */}
      <SupportSection />
      
      {/* Projects Section */}
      <ProjectsSection />
      
      {/* News Section */}
      <NewsSection />
      
      {/* Footer */}
      <Footer />
    </div>
  );
}
