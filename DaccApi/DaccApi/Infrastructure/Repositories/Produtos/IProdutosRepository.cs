using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Products
{
    public interface IProdutosRepository
    {
        Task<List<Produto>> GetAllProductsAsync();
        Task<Produto?> GetProductByIdAsync(Guid id);
        Task CreateProductAsync(Produto product);
        Task CreateProductVariationAsync(ProdutoVariacao variation);
        Task AddProductImagesAsync(ProdutoImagem imagem);
        Task RemoveProductByIdAsync(Guid id);
        Task<List<Produto>> SearchProductsAsync(RequestQueryProdutos query);
        Task UpdateProductAsync(Produto product);

        // New variation management methods
        Task<ProdutoVariacao?> GetVariationByIdAsync(Guid variationId);
        Task<List<ProdutoVariacao>> GetVariationsByProductIdAsync(Guid productId);
        Task<bool> VariationExistsAsync(Guid productId, string cor, string tamanho);
        Task UpdateVariationAsync(ProdutoVariacao variation);
        Task DeleteVariationAsync(Guid variationId);
        Task DeleteVariationImagesAsync(Guid variationId);
        Task<int> CheckProductVariationStock(Guid productVariationId);
        Task<bool> RemoveProductVariationStockAsync(Guid productVariationId, int amount);
        Task<List<ProdutoVariacaoInfo>> GetVariationsWithProductByIdsAsync(List<Guid> variationIds);
        Task<bool> RemoveMultipleProductsStockAsync(List<Guid> variationIds, List<int> quantities);
        Task<ProdutoImagem?> GetImageByIdAsync(Guid imageId);
        Task UpdateProductImageAsync(ProdutoImagem imagem);
        Task DeleteImageAsync(Guid imageId);
        Task<Produto?> GetProductByProductVariationIdAsync(Guid productVariationId);
    }
}