using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Products
{
    public interface IProdutosRepository
    {
        public Task<List<Produto>> GetAllProducts();
        public Task<Produto?> GetProductById(Guid? id);
        public void CreateProductAsync(Produto product);
        public void RemoveProductByIdAsync(Guid? id);
    }
}
