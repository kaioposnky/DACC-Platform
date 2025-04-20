using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Products
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetListProductsAsync();
    }
}
