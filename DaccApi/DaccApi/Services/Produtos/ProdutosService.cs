using DaccApi.Model;
using DaccApi.Infrastructure.Repositories.Products;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Helpers;

namespace DaccApi.Services.Products
{
    public class ProdutosService : IProdutosService
    {
        private readonly IProdutosRepository _produtosRepository;

        public ProdutosService(IProdutosRepository produtosRepository)
        {
            _produtosRepository = produtosRepository;
        }

        public IActionResult GetAllProducts()
        {
            try
            {
                var products = _produtosRepository.GetAllProducts().Result;

                if (products.Count == 0) 
                    return ResponseHelper.CreateBadRequestResponse("Nenhum produto foi encontrado!");


                return ResponseHelper.CreateSuccessResponse(new { Products = products }, "Produtos obtidos com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse("Erro ao obter os produtos!" + ex);
            }
        }

        public IActionResult GetProductById(int produtoId)
        {
            try
            {
                var product = _produtosRepository.GetProductById(produtoId).Result;

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
                if (string.IsNullOrWhiteSpace(requestProduto.Descricao) ||
                    string.IsNullOrWhiteSpace(requestProduto.Nome) ||
                    requestProduto.ImagemUrl == null ||
                    requestProduto.Preco == null)
                {
                    return ResponseHelper.CreateBadRequestResponse(
                        "Request inválido. Os campos Id, Name, Description, ImageUrl, Price não podem ser nulos.");
                }

                if (requestProduto.Preco <= 0)
                {
                    return ResponseHelper.CreateBadRequestResponse("O campo Price deve conter um valor > 0.");
                }
                
                var newProduct = new Produto
                {
                    Nome = requestProduto.Nome,
                    ImagemUrl = requestProduto.ImagemUrl,
                    Preco = requestProduto.Preco,
                    Descricao = requestProduto.Descricao,
                    Id = requestProduto.Id,
                };

                // Adicionar comentários nos produtos

                _produtosRepository.CreateProductAsync(newProduct);

                return ResponseHelper.CreateSuccessResponse(
                    new { Product = newProduct }, "Produto adicionado com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse("Erro ao adicionar produto!" + ex);
            }
        }

        public IActionResult RemoveProductById(int produtoId)
        {
            try
            {

                _produtosRepository.RemoveProductByIdAsync(produtoId);

                return ResponseHelper.CreateSuccessResponse("", "Produto removido com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse("Erro ao remover produto!" + ex);
            }
        }
    
    }
}
