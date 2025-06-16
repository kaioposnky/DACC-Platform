using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Products
{
    public interface IProdutosRepository
    {
        public List<Produto> GetAllProducts();
        public Produto? GetProductById(Guid? id);
        public void CreateProductAsync(Produto product);
        public void RemoveProductByIdAsync(Guid? id);
    }
}
