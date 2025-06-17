using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Products
{
    public interface IProdutosRepository
    {
        public Task<List<Produto>> GetAllProducts();
        public Task<Produto?> GetProductById(int id);
        public void CreateProductAsync(Produto product);
        public void RemoveProductByIdAsync(int id);
    }
}
