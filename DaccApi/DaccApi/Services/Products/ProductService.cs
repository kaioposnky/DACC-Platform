using DaccApi.Helpers;
using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Repositories;
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
        }
    }
}
