using DaccApi.Model;
using DaccApi.Infrastructure.Repositories.Products;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Helpers;

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

                if (products.Count == 0) return ResponseHelper.CreateBadRequestResponse("Nenhum produto foi encontrado!");


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

                if (product == null) return ResponseHelper.CreateBadRequestResponse("Nenhum produto foi encontrado!");

                return ResponseHelper.CreateSuccessResponse(new { Product = product }, "Produto obtido com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse("Erro ao obter um produto pelo Id!" + ex);
            }
        }
        public IActionResult CreateProduct(RequestProduto requestProduto)
        {
            // Implementar lógica de registro de pessoa que fez esse request de adicionar o produto
            try
            {
                if (string.IsNullOrWhiteSpace(requestProduto.Description) ||
                    string.IsNullOrWhiteSpace(requestProduto.Name) ||
                    requestProduto.ImageUrl == null ||
                    requestProduto.Price == null ||
                    requestProduto.Id == null)
                {
                    return ResponseHelper.CreateBadRequestResponse(
                        "Request inválido. Os campos Id, Name, Description, ImageUrl, Price não podem ser nulos.");
                }

                if (requestProduto.Price <= 0)
                {
                    return ResponseHelper.CreateBadRequestResponse("O campo Price deve conter um valor > 0.");
                }
                
                var newProduct = new Produto
                {
                    Name = requestProduto.Name,
                    ImageUrl = requestProduto.ImageUrl,
                    Price = requestProduto.Price,
                    Description = requestProduto.Description,
                    Id = requestProduto.Id.Value,
                    ReleaseDate = null // Lógica para só liberar produtos que estão com release date
                };

                // Adicionar comentários nos produtos

                _productRepository.CreateProductAsync(newProduct);

                return ResponseHelper.CreateSuccessResponse(
                    new { Product = newProduct }, "Produto adicionado com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse("Erro ao adicionar produto!" + ex);
            }
        }

        public IActionResult RemoveProductById(RequestProduto requestProduto)
        {            // Implementar lógica de registro de pessoa que fez esse request de adicionar o produto
            try
            {
                if (requestProduto.Id == null)
                {
                    return ResponseHelper.CreateBadRequestResponse("Request inválido. O Id precisa não pode ser nulo.");
                }
                
                // Lógica interessante de se adicionar mais tarde
                // if (requestProduto.ReleaseDate != null)
                // {
                //     return ResponseHelper.CreateBadRequestResponse("Você não pode remover um produto que já está lançado!");
                // }
                
                _productRepository.RemoveProductByIdAsync(requestProduto.Id);

                return ResponseHelper.CreateSuccessResponse("", "Produto removido com sucesso!");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao remover produto por id" + ex);
            }
        }
    
    }
}
