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

        public IActionResult GetAllProducts()
        {
            try
            {
                var products = _productRepository.GetAllProductsAsync().Result;

                if (products == null || products.Count == 0)
                {
                    return ResponseHelper.CreateBadRequestResponse("Nenhum produto foi encontrado!");
                }

                return ResponseHelper.CreateSuccessResponse(new { Products = products }, "Produtos obtidos com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse("Erro ao obter os produtos!" + ex);
            }
        }

        public IActionResult GetProductById(RequestProduto requestProduto)
        {
            try
            {
                if (requestProduto.Id == null)
                {
                    return ResponseHelper.CreateBadRequestResponse("Requisição inválida. O ProdutoId não pode ser nulo!");
                }
                var product = _productRepository.GetProductByIdAsync(requestProduto.Id).Result;

                if (product == null)
                {
                    return ResponseHelper.CreateBadRequestResponse("Nenhum produto foi encontrado!");
                }

                return ResponseHelper.CreateSuccessResponse(new { Product = product }, "Produto obtido com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse("Erro ao obter um produto pelo Id!" + ex);
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

        public String AddProductRating(int productId, int userId, string? comment, float rating)
        {

            if (rating > 5 || rating < 0)
            {
                throw new ArgumentException("A nota da avaliação deve ser um valor entre 0 e 5!");
            }

            var newProductRating = new ProductRating();
            newProductRating.ProductId = productId;
            newProductRating.UserId = userId;
            newProductRating.Commentary = comment;
            newProductRating.Rating = rating;
            newProductRating.DatePosted = DateTime.Now;


            _productRepository.AddProductRatingAsync(newProductRating);

            String productName = GetProductById(productId).Name;

            // Implementar lógica para obter nome de usuário que fez a operação
            // Implementar Salvar ação de compra do usuário numa base de dados para obtermos informações úteis para melhorar as vendas

            return "Avaliação de nota" + rating + " feita ao produto " + productName + " com sucesso!";
        }
    
    }
}
