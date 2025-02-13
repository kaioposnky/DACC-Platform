using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Products
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetListProductsAsync();
        public Product GetProductByIdAsync(int id);

        public void AddProductAsync(Product product);

        public void RemoveProductByIdAsync(int id);

        public void AddProductRatingAsync(ProductRating productRating);

    }
}
