using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Products
{
    public interface IProdutosRepository
    {
        public Task<List<Produto>> GetAllProductsAsync();
        public Task<Produto?> GetProductByIdAsync(Guid? id);
        public void CreateProductAsync(Produto product);
        public void RemoveProductByIdAsync(Guid? id);
    }
}
