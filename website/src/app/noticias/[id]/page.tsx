'use client'

import { useState, useEffect } from 'react'
import { useParams } from 'next/navigation'
import { Footer, Navigation } from "@/components"
import { NewsArticleBanner, NewsArticleContent } from "@/components/organisms"
import { apiService } from '@/services/api'
import { News } from '@/types'
import Link from 'next/link'

export default function NewsDetailPage() {
  const params = useParams()
  const [article, setArticle] = useState<News | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    const fetchArticle = async () => {
      try {
        setLoading(true)
        const id = params.id as string
        const data = await apiService.getNewsItem(id)
        setArticle(data)
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to fetch article')
        console.error('Error fetching article:', err)
      } finally {
        setLoading(false)
      }
    }

    if (params.id) {
      fetchArticle()
    }
  }, [params.id])

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Navigation />
        
        {/* Loading Banner */}
        <div className="bg-gray-900 py-24">
          <div className="max-w-6xl mx-auto px-4 sm:px-6 lg:px-8">
            <div className="animate-pulse space-y-4">
              <div className="h-4 bg-gray-700 rounded w-1/4"></div>
              <div className="h-12 bg-gray-700 rounded w-3/4"></div>
              <div className="h-6 bg-gray-700 rounded w-1/2"></div>
            </div>
          </div>
        </div>

        {/* Loading Content */}
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
          <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-8">
            <div className="animate-pulse space-y-4">
              <div className="h-4 bg-gray-200 rounded w-full"></div>
              <div className="h-4 bg-gray-200 rounded w-5/6"></div>
              <div className="h-4 bg-gray-200 rounded w-4/6"></div>
              <div className="h-4 bg-gray-200 rounded w-3/4"></div>
            </div>
          </div>
        </div>

        <Footer />
      </div>
    )
  }

  if (error || !article) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Navigation />
        
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-24">
          <div className="text-center">
            <div className="text-gray-400 mb-4">
              <svg className="h-16 w-16 mx-auto" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z" />
              </svg>
            </div>
            <h1 className="text-2xl font-bold text-gray-900 mb-2">Artigo não encontrado</h1>
            <p className="text-gray-600 mb-8">
              O artigo que você está procurando não existe ou foi removido.
            </p>
              <Link
              href="/noticias"
              className="bg-primary text-white px-6 py-3 rounded-md hover:bg-primary-dark transition-colors"
            >
              Voltar para Notícias
            </Link>
          </div>
        </div>

        <Footer />
      </div>
    )
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />
      
      <NewsArticleBanner
        title={article.title}
        description={article.description}
        image={article.image || 'https://images.unsplash.com/photo-1585776245991-cf89dd7fc73a?ixlib=rb-4.0.3&auto=format&fit=crop&w=1000&q=80'}
        author={article.author || 'Equipe de Redação'}
        date={article.date}
        readTime={article.readTime || 5}
        category={article.category}
        tags={article.tags || []}
      />
      
      <NewsArticleContent 
        content={article.content || article.description}
      />
      
      <Footer />
    </div>
  )
} 