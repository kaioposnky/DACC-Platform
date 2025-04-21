using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Products
{
    public interface IProductRepository
    {
        public Task<List<Produto>> GetAllProductsAsync();
        public Task<Produto?> GetProductByIdAsync(Guid? id);
        public void AddProductAsync(Produto product);
        public void RemoveProductByIdAsync(Guid? id);
        public void AddProductRatingAsync(ProductRating productRating);
    }
}
