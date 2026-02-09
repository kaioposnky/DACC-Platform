"use client";

import {
  EditPageHeader,
  PageLoader,
  AdminCard,
} from "@/components";
import { apiService } from "@/services/api";
import { ProductReview, ProductVariation } from "@/types";
import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { toast } from "sonner";
import { StarIcon } from "@heroicons/react/24/solid";
import { StarIcon as StarIconOutline } from "@heroicons/react/24/outline";
import {
  CalendarIcon,
  UserIcon,
  ShoppingBagIcon,
  ChatBubbleLeftIcon,
} from "@heroicons/react/24/outline";
import Image from "next/image";

export default function AdminViewAvaliacaoPage() {
  const router = useRouter();
  const params = useParams();

  const [review, setReview] = useState<ProductReview | null>(null);
  const [variation, setVariation] = useState<ProductVariation | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const reviewResponse = await apiService.getReview(params.id as string);
        setReview(reviewResponse);

        const productResponse = await apiService.getProduct(reviewResponse.productId);
        const variation = productResponse.variations.find(variation => variation.id === reviewResponse.productVariationId);
        setVariation(variation || null);
      } catch (error) {
        console.error(error);
        toast.error("Erro ao carregar avaliação");
      } finally {
        setIsLoading(false);
      }
    };
    fetchData();
  }, [params.id]);

  const formatDate = (dateString?: string) => {
    if (!dateString) return "Não informado";
    try {
      const date = new Date(dateString);
      return new Intl.DateTimeFormat("pt-BR", {
        day: "2-digit",
        month: "short",
        year: "numeric",
      }).format(date);
    } catch {
      return dateString;
    }
  };

  const getRatingColor = (rating: number) => {
    if (rating >= 4) return "text-yellow-400";
    if (rating >= 3) return "text-orange-400";
    return "text-red-400";
  };

  const renderStars = (rating: number) => {
    const stars = [];
    const colorClass = getRatingColor(rating);

    for (let i = 1; i <= 5; i++) {
      if (i <= rating) {
        stars.push(
          <StarIcon key={i} className={`w-6 h-6 ${colorClass}`} />
        );
      } else {
        stars.push(
          <StarIconOutline key={i} className="w-6 h-6 text-gray-300" />
        );
      }
    }
    return stars;
  };

  const handleGoBack = () => router.push("/admin/avaliacoes");
  const handleGoToProduct = () => {
    if (review?.productId) {
      router.push(`/admin/produtos/${review.productId}`);
    }
  };

  if (isLoading) return <PageLoader />;
  if (!review)
    return (
      <div className="flex flex-col items-center justify-center h-[60vh]">
        <h1 className="text-2xl font-bold text-gray-500">
          Avaliação não encontrada
        </h1>
      </div>
    );

  return (
    <div className="pb-20">
      <EditPageHeader
        title={`Avaliação de ${review.userName}`}
        id={review.id}
        onBack={handleGoBack}
        showDelete={false}
        showSave={false}
        label="Visualizando"
      />

      {/* Content */}
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">

          {/* Main Content - 2 columns */}
          <div className="lg:col-span-2 space-y-6">
            {/* Review Content */}
            <AdminCard
              title="Conteúdo da Avaliação"
              icon={<StarIcon className="w-5 h-5" />}
            >
              <div className="space-y-6">
                {/* Rating */}
                <div>
                  <p className="text-sm text-gray-500 mb-2">Avaliação</p>
                  <div className="flex items-center gap-3">
                    <div className="flex gap-1">
                      {renderStars(review.rating)}
                    </div>
                    <span className={`text-2xl font-bold ${getRatingColor(review.rating)}`}>
                      {review.rating.toFixed(1)}
                    </span>
                  </div>
                </div>

                {/* Title */}
                {review.title && (
                  <div>
                    <p className="text-sm text-gray-500 mb-2">Título</p>
                    <h3 className="text-xl font-semibold text-gray-900">
                      {review.title}
                    </h3>
                  </div>
                )}

                {/* Comment */}
                <div>
                  <p className="text-sm text-gray-500 mb-2">Comentário</p>
                  <div className="bg-gray-50 rounded-lg p-4 border border-gray-200">
                    <p className="text-gray-700 leading-relaxed whitespace-pre-wrap">
                      {review.comment || "Sem comentário"}
                    </p>
                  </div>
                </div>
              </div>
            </AdminCard>

            {/* Product Information */}
            <AdminCard
              title="Produto Avaliado"
              icon={<ShoppingBagIcon className="w-5 h-5" />}
            >
              <div className="space-y-4">
                <div className="flex items-center gap-4">
                  {review.productImage ? (
                    <Image
                      src={review.productImage}
                      alt={review.productName}
                      width={80}
                      height={80}
                      className="rounded-lg object-cover border border-gray-200"
                      unoptimized
                    />
                  ) : (
                    <div className="w-20 h-20 rounded-lg bg-gray-200 flex items-center justify-center border border-gray-200">
                      <ShoppingBagIcon className="w-10 h-10 text-gray-400" />
                    </div>
                  )}
                  <div className="flex-1">
                    <button
                      onClick={handleGoToProduct}
                      className="text-lg font-semibold text-blue-600 hover:text-blue-700 hover:underline transition-colors text-left"
                    >
                      {review.productName}
                    </button>
                    <p className="text-sm text-gray-500 font-mono">
                      ID: {review.productId}
                    </p>
                  </div>
                </div>

                {/* Variation Details */}
                {variation && (
                  <div className="pt-4 border-t border-gray-100">
                    <p className="text-xs font-semibold text-gray-500 uppercase tracking-wider mb-3">
                      Variação Avaliada
                    </p>
                    <div className="grid grid-cols-2 gap-4">
                      <div>
                        <p className="text-xs text-gray-500 mb-1">Cor</p>
                        <div className="flex items-center gap-2">
                          <p className="text-sm font-medium text-gray-900 capitalize">
                            {variation.color}
                          </p>
                        </div>
                      </div>

                      <div>
                        <p className="text-xs text-gray-500 mb-1">Tamanho</p>
                        <p className="text-sm font-medium text-gray-900 uppercase">
                          {variation.size}
                        </p>
                      </div>

                      <div>
                        <p className="text-xs text-gray-500 mb-1">SKU</p>
                        <p className="text-sm font-mono text-gray-700">
                          {variation.sku}
                        </p>
                      </div>

                      <div>
                        <p className="text-xs text-gray-500 mb-1">Estoque</p>
                        <p className="text-sm font-medium text-gray-900">
                          {variation.stock} unidades
                        </p>
                      </div>
                    </div>
                  </div>
                )}
              </div>
            </AdminCard>

            {/* Info Note */}
            <div className="bg-blue-50 border border-blue-200 rounded-lg p-4">
              <p className="text-sm text-blue-800">
                <strong>Nota:</strong> As avaliações não podem ser editadas ou excluídas
                para manter a transparência e autenticidade das opiniões dos usuários.
              </p>
            </div>
          </div>

          {/* Sidebar - 1 column */}
          <div className="space-y-6">
            {/* User Information */}
            <AdminCard
              title="Usuário"
              icon={<UserIcon className="w-5 h-5" />}
            >
              <div className="flex flex-col items-center gap-4 text-center">
                {review.userAvatar ? (
                  <Image
                    src={review.userAvatar}
                    alt={review.userName}
                    width={80}
                    height={80}
                    className="rounded-full object-cover"
                  />
                ) : (
                  <div className="w-20 h-20 rounded-full bg-gray-200 flex items-center justify-center">
                    <UserIcon className="w-10 h-10 text-gray-400" />
                  </div>
                )}
                <div>
                  <h3 className="text-lg font-semibold text-gray-900">
                    {review.userName}
                  </h3>
                  <p className="text-xs text-gray-500 font-mono mt-1">
                    ID: {review.userId}
                  </p>
                </div>
              </div>
            </AdminCard>

            {/* Review Information */}
            <AdminCard
              title="Informações"
              icon={<ChatBubbleLeftIcon className="w-5 h-5" />}
            >
              <div className="space-y-4">
                <div className="flex items-start gap-3">
                  <CalendarIcon className="w-5 h-5 text-gray-400 mt-0.5" />
                  <div className="flex-1">
                    <p className="text-xs text-gray-500">Criado em</p>
                    <p className="text-sm font-medium text-gray-900">
                      {formatDate(review.createdAt)}
                    </p>
                  </div>
                </div>

                <div className="flex items-start gap-3">
                  <CalendarIcon className="w-5 h-5 text-gray-400 mt-0.5" />
                  <div className="flex-1">
                    <p className="text-xs text-gray-500">Atualizado em</p>
                    <p className="text-sm font-medium text-gray-900">
                      {formatDate(review.updatedAt)}
                    </p>
                  </div>
                </div>

                <div className="flex items-start gap-3">
                  <ChatBubbleLeftIcon className="w-5 h-5 text-gray-400 mt-0.5" />
                  <div className="flex-1">
                    <p className="text-xs text-gray-500 mb-1">ID da Avaliação</p>
                    <p className="text-xs font-mono text-gray-700 break-all bg-gray-50 p-2 rounded border border-gray-200">
                      {review.id}
                    </p>
                  </div>
                </div>
              </div>
            </AdminCard>
          </div>

        </div>
      </div>
    </div>
  );
}
