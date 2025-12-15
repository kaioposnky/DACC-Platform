'use client';

import { useState, useEffect } from 'react';
import { useParams, useRouter } from 'next/navigation';
import { Product } from '@/types';
import { apiService } from '@/services/api';
import { Navigation, Footer } from '@/components';
import { ShoppingCart, ProductTabs } from '@/components/molecules';
import { ProductRecommendations } from '@/components/organisms';
import { motion } from 'framer-motion';
import Image from 'next/image';
import {
    StarIcon,
    HeartIcon,
    ShoppingCartIcon,
    ArrowLeftIcon,
    CheckIcon,
    XMarkIcon
} from '@heroicons/react/24/solid';
import {
    StarIcon as StarOutlineIcon,
    HeartIcon as HeartOutlineIcon
} from '@heroicons/react/24/outline';
import { useCart } from '@/context/CartContext';
import Link from 'next/link';

export default function ProductDetailPage() {
    const params = useParams();
    const router = useRouter();
    const { addToCart } = useCart();
    const [product, setProduct] = useState<Product | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [selectedImageIndex, setSelectedImageIndex] = useState(0);
    const [selectedSize, setSelectedSize] = useState<string>('');
    const [selectedColor, setSelectedColor] = useState<string>('');
    const [quantity, setQuantity] = useState(1);
    const [isFavorite, setIsFavorite] = useState(false);

    useEffect(() => {
        const fetchProduct = async () => {
            try {
                setLoading(true);
                const productData = await apiService.getProduct(params.id as string);
                setProduct(productData);
                setSelectedSize(productData.sizes[0] || '');
                setSelectedColor(productData.colors[0] || '');
            } catch (err) {
                setError('Produto não encontrado');
                console.error('Error fetching product:', err);
            } finally {
                setLoading(false);
            }
        };

        if (params.id) {
            fetchProduct();
        }
    }, [params.id]);

    const formatPrice = (price: number) => {
        return new Intl.NumberFormat('pt-BR', {
            style: 'currency',
            currency: 'BRL',
        }).format(price);
    };

    const renderStars = (rating: number) => {
        const stars = [];
        const fullStars = Math.floor(rating);
        const hasHalfStar = rating % 1 !== 0;

        for (let i = 0; i < fullStars; i++) {
            stars.push(
                <StarIcon key={i} className="w-5 h-5 text-yellow-400" />
            );
        }

        if (hasHalfStar) {
            stars.push(
                <div key="half" className="relative">
                    <StarOutlineIcon className="w-5 h-5 text-yellow-400" />
                    <StarIcon className="w-5 h-5 text-yellow-400 absolute top-0 left-0" style={{ width: '50%', overflow: 'hidden' }} />
                </div>
            );
        }

        const remainingStars = 5 - Math.ceil(rating);
        for (let i = 0; i < remainingStars; i++) {
            stars.push(
                <StarOutlineIcon key={`outline-${i}`} className="w-5 h-5 text-gray-300" />
            );
        }

        return stars;
    };

    const handleAddToCart = () => {
        if (product) {
            addToCart(product, quantity, selectedSize, selectedColor);
            // You could add a toast notification here
        }
    };

    const handleGoBack = () => {
        router.back();
    };

    if (loading) {
        return (
            <div className="bg-gray-100 min-h-screen">
                <Navigation />
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
                    <div className="animate-pulse">
                        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                            <div className="bg-gray-200 aspect-square rounded-xl"></div>
                            <div className="space-y-4">
                                <div className="h-8 bg-gray-200 rounded w-3/4"></div>
                                <div className="h-6 bg-gray-200 rounded w-1/2"></div>
                                <div className="h-4 bg-gray-200 rounded w-1/4"></div>
                                <div className="h-20 bg-gray-200 rounded"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <Footer />
                <ShoppingCart />
            </div>
        );
    }

    if (error || !product) {
        return (
            <div className="bg-gray-100 min-h-screen">
                <Navigation />
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
                    <div className="text-center">
                        <h1 className="text-2xl font-bold text-gray-900 mb-4">Produto não encontrado</h1>
                        <p className="text-gray-600 mb-8">{error}</p>
                        <button
                            onClick={handleGoBack}
                            className="bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 px-4 rounded-lg transition-colors duration-200"
                        >
                            Voltar
                        </button>
                    </div>
                </div>
                <Footer />
                <ShoppingCart />
            </div>
        );
    }

    const hasDiscount = product.originalPrice && product.originalPrice > product.price;
    const discountPercentage = hasDiscount
        ? Math.round(((product.originalPrice! - product.price) / product.originalPrice!) * 100)
        : 0;

    return (
        <div className="bg-gray-100 min-h-screen">
            <Navigation />

            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
                <Link href="/loja" className="flex items-center gap-2 text-gray-600 hover:text-gray-900 mb-6 transition-colors duration-200">
                    <ArrowLeftIcon className="w-5 h-5" />
                    Voltar para a loja
                </Link>


                {/* Product Detail */}
                <div className="bg-white rounded-xl shadow-lg overflow-hidden">
                    <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 p-8">
                        {/* Product Images */}
                        <div className="space-y-4">
                            {/* Main Image */}
                            <div className="aspect-square rounded-xl overflow-hidden">
                                <Image
                                    src={product.images[selectedImageIndex]}
                                    alt={product.name}
                                    width={600}
                                    height={600}
                                    className="w-full h-full object-cover"
                                />
                            </div>

                            {/* Image Thumbnails */}
                            {product.images.length > 1 && (
                                <div className="flex gap-2 overflow-x-auto">
                                    {product.images.map((image, index) => (
                                        <button
                                            key={index}
                                            onClick={() => setSelectedImageIndex(index)}
                                            className={`flex-shrink-0 w-16 h-16 rounded-lg overflow-hidden border-2 ${selectedImageIndex === index
                                                    ? 'border-blue-500'
                                                    : 'border-gray-200 hover:border-gray-300'
                                                }`}
                                        >
                                            <Image
                                                src={image}
                                                alt={`${product.name} ${index + 1}`}
                                                width={64}
                                                height={64}
                                                className="w-full h-full object-cover"
                                            />
                                        </button>
                                    ))}
                                </div>
                            )}
                        </div>

                        {/* Product Information */}
                        <div className="space-y-6">
                            {/* Category */}
                            <div className="text-sm text-gray-500 uppercase tracking-wide">
                                {product.category === 'tshirts' && 'Camisetas'}
                                {product.category === 'hoodies' && 'Moletons'}
                                {product.category === 'cups' && 'Canecas'}
                                {product.category === 'stickers' && 'Adesivos'}
                                {product.category === 'accessories' && 'Acessórios'}
                            </div>

                            {/* Product Name */}
                            <h1 className="text-3xl font-bold text-gray-900">{product.name}</h1>

                            {/* Rating */}
                            <div className="flex items-center gap-2">
                                <div className="flex items-center">
                                    {renderStars(product.rating)}
                                </div>
                                <span className="text-sm text-gray-500">
                                    {product.rating} ({product.reviews} avaliações)
                                </span>
                            </div>

                            {/* Price */}
                            <div className="flex items-center gap-3">
                                <span className="text-3xl font-bold text-gray-900">
                                    {formatPrice(product.price)}
                                </span>
                                {hasDiscount && (
                                    <>
                                        <span className="text-xl text-gray-500 line-through">
                                            {formatPrice(product.originalPrice!)}
                                        </span>
                                        <span className="bg-red-500 text-white text-sm font-bold px-2 py-1 rounded">
                                            -{discountPercentage}%
                                        </span>
                                    </>
                                )}
                            </div>

                            {/* Description */}
                            <p className="text-gray-600 text-lg leading-relaxed">
                                {product.description}
                            </p>

                            {/* Size Selection */}
                            {product.sizes.length > 1 && (
                                <div>
                                    <h3 className="text-sm font-semibold text-gray-900 mb-3">Tamanho:</h3>
                                    <div className="flex gap-2">
                                        {product.sizes.map((size) => (
                                            <button
                                                key={size}
                                                onClick={() => setSelectedSize(size)}
                                                className={`px-4 py-2 border rounded-lg font-medium transition-colors duration-200 ${selectedSize === size
                                                        ? 'border-blue-500 bg-blue-50 text-blue-600'
                                                        : 'border-gray-300 hover:border-gray-400'
                                                    }`}
                                            >
                                                {size}
                                            </button>
                                        ))}
                                    </div>
                                </div>
                            )}

                            {/* Color Selection */}
                            {product.colors.length > 1 && (
                                <div>
                                    <h3 className="text-sm font-semibold text-gray-900 mb-3">Cor:</h3>
                                    <div className="flex gap-2">
                                        {product.colors.map((color) => (
                                            <button
                                                key={color}
                                                onClick={() => setSelectedColor(color)}
                                                className={`px-4 py-2 border rounded-lg font-medium transition-colors duration-200 ${selectedColor === color
                                                        ? 'border-blue-500 bg-blue-50 text-blue-600'
                                                        : 'border-gray-300 hover:border-gray-400'
                                                    }`}
                                            >
                                                {color}
                                            </button>
                                        ))}
                                    </div>
                                </div>
                            )}

                            {/* Quantity */}
                            <div>
                                <h3 className="text-sm font-semibold text-gray-900 mb-3">Quantidade:</h3>
                                <div className="flex items-center gap-3">
                                    <button
                                        onClick={() => setQuantity(Math.max(1, quantity - 1))}
                                        className="p-2 border border-gray-300 rounded-lg hover:bg-gray-50"
                                    >
                                        <XMarkIcon className="w-4 h-4" />
                                    </button>
                                    <span className="text-lg font-medium min-w-[50px] text-center">
                                        {quantity}
                                    </span>
                                    <button
                                        onClick={() => setQuantity(Math.min(product.stockCount, quantity + 1))}
                                        className="p-2 border border-gray-300 rounded-lg hover:bg-gray-50"
                                    >
                                        <CheckIcon className="w-4 h-4" />
                                    </button>
                                </div>
                            </div>

                            {/* Stock Status */}
                            <div className="flex items-center gap-2">
                                {product.inStock ? (
                                    <>
                                        <CheckIcon className="w-5 h-5 text-green-500" />
                                        <span className="text-green-600 font-medium">
                                            {product.stockCount} em estoque
                                        </span>
                                    </>
                                ) : (
                                    <>
                                        <XMarkIcon className="w-5 h-5 text-red-500" />
                                        <span className="text-red-600 font-medium">Fora de estoque</span>
                                    </>
                                )}
                            </div>

                            {/* Actions */}
                            <div className="flex gap-4 pt-6">
                                <motion.button
                                    whileHover={{ scale: 1.02 }}
                                    whileTap={{ scale: 0.98 }}
                                    onClick={handleAddToCart}
                                    disabled={!product.inStock}
                                    className="flex-1 bg-blue-600 hover:bg-blue-700 disabled:bg-gray-400 text-white font-semibold py-3 px-6 rounded-lg transition-colors duration-200 flex items-center justify-center gap-2"
                                >
                                    <ShoppingCartIcon className="w-5 h-5" />
                                    {product.inStock ? 'Adicionar ao Carrinho' : 'Indisponível'}
                                </motion.button>

                                <button
                                    onClick={() => setIsFavorite(!isFavorite)}
                                    className="p-3 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors duration-200"
                                >
                                    {isFavorite ? (
                                        <HeartIcon className="w-6 h-6 text-red-500" />
                                    ) : (
                                        <HeartOutlineIcon className="w-6 h-6 text-gray-600" />
                                    )}
                                </button>
                            </div>
                        </div>
                    </div>
                </div>

                {/* Product Tabs */}
                <div className="mt-12">
                    <ProductTabs product={product} />
                </div>

                {/* Product Recommendations */}
                <div className="mt-12">
                    <ProductRecommendations currentProduct={product} />
                </div>
            </div>

            <Footer />
            <ShoppingCart />
        </div>
    );
} 