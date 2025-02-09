using DaccApi.Model;
using DaccApi.Infrastructure.Repositories.Products;

namespace DaccApi.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<List<Product>> GetProducts()
        {

            try
            {
                // Lógica de obter produtos usando Query do SQL
                var products = _productRepository.GetListProductsAsync();

                return products;
            }
            catch (Exception ex) {

                throw ex;
            }
        }
        
        public Product GetProductById(int ProductId)
        {
            // Implementar lógica de registro de pessoa que abriu o produto para deixar salvo

            // TODO: Implementar no Repository
            var product = _productRepository.GetProductByIdAsync(ProductId);

            return product;

        }

        public String AddProduct(string productName, string productDescription, byte[] productImageUrl, double productPrice, int productId)
        {
            // Implementar lógica de registro de pessoa que fez esse request de adicionar o produto

            var newProduct = new Product();

            newProduct.Name = productName;
            newProduct.ImageUrl = productImageUrl;
            newProduct.Price = productPrice;
            newProduct.Description = productDescription;
            newProduct.Id = productId;
            newProduct.ReleaseDate = DateTime.Now;
            // Adicionar comentários nos produtos

            _productRepository.AddProductAsync(newProduct);

            return "Produto " + productName + " adicionado com sucesso!";
        }

        public String RemoveProductById(int productId)
        {
            // Implementar lógica de registro de pessoa que fez esse request de adicionar o produto

            _productRepository.RemoveProductByIdAsync(productId);

            String productName = GetProductById(productId).Name;

            return "Produto " + productName + " removido com sucesso!";
        }
    }
}
