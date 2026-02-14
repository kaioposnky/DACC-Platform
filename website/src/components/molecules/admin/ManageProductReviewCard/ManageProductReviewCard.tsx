"use client";

import { ProductReview } from "@/types";
import { motion } from "framer-motion";
import { StarIcon, EyeIcon } from "@heroicons/react/24/solid";
import Image from "next/image";
import Link from "next/link";

interface ManageProductReviewCardProps {
  review: ProductReview;
  className?: string;
}

export default function ManageProductReviewCard({
  review,
  className = "",
}: ManageProductReviewCardProps) {
  const formatDate = (dateString: string) => {
    if (!dateString) return "";
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

  return (
    <Link href={`/admin/avaliacoes/${review.id}`} className="block">
      <motion.div
        initial={{ opacity: 0, y: 10 }}
        animate={{ opacity: 1, y: 0 }}
        whileHover={{ y: -2 }}
        className={`bg-white w-full rounded-xl shadow-sm border border-gray-100 p-4 cursor-pointer hover:shadow-md transition-all duration-200 flex items-center justify-between gap-6 ${className}`}
      >
        {/* 1. Estilo das imagens e o nome em baixo */}
        <div className="flex items-center gap-6 shrink-0">
          {/* User - Circular Style */}
          <div className="flex flex-col items-center gap-2">
            <div className="relative w-12 h-12 rounded-full overflow-hidden bg-gray-50 border border-gray-100 shrink-0">
              {review.userAvatar ? (
                <Image
                  src={review.userAvatar}
                  alt={review.userName || "User"}
                  fill
                  className="object-cover"
                />
              ) : (
                <div className="w-full h-full flex items-center justify-center bg-blue-50 text-blue-600 font-bold text-sm">
                  {review.userName?.charAt(0) || "U"}
                </div>
              )}
            </div>
            <p className="text-xs font-semibold text-gray-700 max-w-20 text-center truncate">
              {review.userName || "Usuário"}
            </p>
          </div>

          {/* Product - Square Style */}
          <div className="flex flex-col items-center gap-2">
            <div className="relative w-12 h-12 rounded-lg overflow-hidden bg-gray-50 border border-gray-100 shrink-0">
              {review.productImage ? (
                <Image
                  src={review.productImage}
                  alt={review.productName || "Product"}
                  fill
                  className="object-cover"
                  unoptimized
                />
              ) : (
                <div className="w-full h-full flex items-center justify-center bg-gray-100 text-gray-400 font-bold text-[10px]">
                  IMG
                </div>
              )}
            </div>
            <p className="text-xs font-semibold text-gray-700 max-w-32 text-center truncate">
              {review.productName || "Produto"}
            </p>
          </div>
        </div>

        {/* 2. ID da Avaliação */}
        <div className="flex flex-col items-center gap-1 shrink-0 min-w-30">
          <span className="text-[10px] text-gray-400 uppercase tracking-wider font-bold">Review ID</span>
          <span className="inline-block px-3 py-1 bg-gray-50 text-gray-600 rounded text-[10px] font-mono border border-gray-100 break-all text-center">
            {review.id || "----"}
          </span>
        </div>

        {/* 3. Avaliação e Comentário */}
        <div className="flex-1 min-w-0 space-y-1">
          <div className="flex items-center gap-1">
            <div className="flex items-center gap-0.5">
              {Array.from({ length: 5 }, (_, i) => (
                <StarIcon
                  key={i}
                  className={`w-4 h-4 ${i < review.rating ? getRatingColor(review.rating) : "text-gray-200"}`}
                />
              ))}
            </div>
            <span className="text-xs font-bold text-gray-900 ml-1">{review.title}</span>
          </div>
          <p className="text-sm text-gray-500 line-clamp-1 italic">
            "{review.comment || "Sem comentário"}"
          </p>
        </div>

        {/* 4. Data e Ação */}
        <div className="flex items-center gap-6 shrink-0">
          <div className="text-right">
            <p className="text-[10px] text-gray-400 font-medium uppercase tracking-tighter">Postado em</p>
            <p className="text-xs font-bold text-gray-600">{formatDate(review.createdAt || "")}</p>
          </div>

          <div className="pl-6 border-l border-gray-100">
            <button
              title="Ver detalhes"
              className="p-2.5 rounded-full bg-gray-50 text-gray-400 hover:bg-blue-50 hover:text-blue-600 transition-all active:scale-90"
            >
              <EyeIcon className="w-5 h-5" />
            </button>
          </div>
        </div>
      </motion.div>
    </Link>
  );
}
