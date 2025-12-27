using System.Data;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Products
{
    /// <summary>
    /// Define a interface para o repositório de produtos.
    /// </summary>
    public interface IProdutosRepository
    {
        /// <summary>
        /// Obtém todos os produtos.
        /// </summary>
        Task<List<Produto>> GetAllProductsAsync();
        /// <summary>
        /// Obtém um produto específico pelo seu ID.
        /// </summary>
        Task<Produto?> GetProductByIdAsync(Guid id);
        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        Task CreateProductAsync(Produto product);
        /// <summary>
        /// Cria uma nova variação de produto.
        /// </summary>
        Task CreateProductVariationAsync(ProdutoVariacao variation);
        /// <summary>
        /// Adiciona imagens a um produto.
        /// </summary>
        Task AddProductImagesAsync(ProdutoImagem imagem);
        /// <summary>
        /// Remove um produto pelo seu ID.
        /// </summary>
        Task RemoveProductByIdAsync(Guid id);
        /// <summary>
        /// Busca produtos com base em filtros.
        /// </summary>
        Task<List<Produto>> SearchProductsAsync(RequestQueryProdutos query);
        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        Task UpdateProductAsync(Produto product);
        /// <summary>
        /// Obtém uma variação específica pelo seu ID.
        /// </summary>
        Task<ProdutoVariacao?> GetVariationByIdAsync(Guid variationId);
        /// <summary>
        /// Obtém todas as variações de um produto.
        /// </summary>
        Task<List<ProdutoVariacao>> GetVariationsByProductIdAsync(Guid productId);
        /// <summary>
        /// Verifica se uma variação específica já existe.
        /// </summary>
        Task<bool> VariationExistsAsync(Guid productId, string cor, string tamanho);
        /// <summary>
        /// Atualiza uma variação de produto existente.
        /// </summary>
        Task UpdateVariationAsync(ProdutoVariacao variation);
        /// <summary>
        /// Deleta uma variação de produto.
        /// </summary>
        Task DeleteVariationAsync(Guid variationId);
        /// <summary>
        /// Deleta todas as imagens de uma variação.
        /// </summary>
        Task DeleteVariationImagesAsync(Guid variationId);
        /// <summary>
        /// Verifica o estoque de uma variação de produto.
        /// </summary>
        Task<int> CheckProductVariationStock(Guid productVariationId);
        /// <summary>
        /// Remove uma quantidade do estoque de uma variação de produto.
        /// </summary>
        Task<bool> RemoveProductVariationStockAsync(Guid productVariationId, int amount);
        /// <summary>
        /// Obtém informações de múltiplas variações e seus produtos.
        /// </summary>
        Task<List<ProdutoVariacaoInfo>> GetVariationsWithProductByIdsAsync(List<Guid> variationIds);
        /// <summary>
        /// Verifica o estoque de múltiplos produtos.
        /// </summary>
        Task<bool> CheckMultipleProductsStockAsync(List<Guid> variationIds, List<int> quantities);
        /// <summary>
        /// Remove uma quantidade do estoque de múltiplos produtos de forma atômica.
        /// </summary>
        Task<bool> RemoveMultipleProductsStockDirectAsync(List<Guid> variationIds, List<int> quantities, IDbTransaction? transaction = null);
        /// <summary>
        /// Obtém uma imagem específica pelo seu ID.
        /// </summary>
        Task<ProdutoImagem?> GetImageByIdAsync(Guid imageId);
        /// <summary>
        /// Atualiza uma imagem de produto.
        /// </summary>
        Task UpdateProductImageAsync(ProdutoImagem imagem);
        /// <summary>
        /// Deleta uma imagem de produto.
        /// </summary>
        Task DeleteImageAsync(Guid imageId);
        /// <summary>
        /// Obtém um produto a partir do ID de uma de suas variações.
        /// </summary>
        Task<Produto?> GetProductByProductVariationIdAsync(Guid productVariationId);
        /// <summary>
        /// Obtém as avaliações de um produto.
        /// </summary>
        Task<List<AvaliacaoProduto>> GetProductReviewsAsync(Guid productId);
    }
}
