"use client"

import { EditPageHeader, NewsForm } from "@/components";
import { apiService } from "@/services/api";
import { News } from "@/types";
import { useRouter } from "next/navigation";
import { useState, useEffect } from "react";
import { toast } from "sonner";

export default function AdminNoticiasNewPage() {

  const router = useRouter();
  const [users, setUsers] = useState<any[]>([]);

  const [isCreating, setIsCreating] = useState(false);
  const [news, setNews] = useState<Partial<News>>({
    title: '',
    description: '',
    content: '',
    image: '',
    tags: [],
    date: new Date().toISOString(),
    readTime: 0,
  });

  useState(() => {
    apiService.getUsers().then(setUsers).catch(console.error);
  });

  const handleGoBack = () => router.back();
  const handleCreate = () => {
    if (!news.title || !news.description || !news.content) {
      toast.error('Preencha os campos obrigatórios');
      return;
    }

    const payload: any = {
      ...news,
      categoryId: news.categoryId || news.category?.id,
      authorId: news.author?.id,
      icon: news.icon || '',
      gradient: news.gradient || '',
      readMoreLink: news.readMoreLink || '',
    };

    setIsCreating(true);
    apiService.createNews(payload).then(() => {
      router.push('/admin/conteudo');
      toast.success('Notícia criada com sucesso!');
    }).catch((error) => {
      console.error(error);
      toast.error('Erro ao criar notícia');
    }).finally(() => {
      setIsCreating(false);
    });
  }

  const handleChange = (field: keyof News, value: any) => {
    setNews({ ...news, [field]: value });
  }

  return (
    <div className="mb-10">
      <EditPageHeader
        title="Nova Notícia"
        label="Criando"
        onBack={handleGoBack}
        onSave={handleCreate}
        loadingSave={isCreating}
        saveButtonText="Publicar Notícia"
      />

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        <NewsForm
          news={news}
          users={users}
          onChange={handleChange}
          mode="create"
        />

      </div>
    </div>
  )
}
