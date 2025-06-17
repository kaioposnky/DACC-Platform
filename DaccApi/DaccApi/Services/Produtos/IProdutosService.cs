using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Products
{
    public interface IProdutosService
    {

        public IActionResult GetAllProducts();
        public IActionResult GetProductById(int userId);
        public IActionResult CreateProduct(RequestProduto requestProduto);
        public IActionResult RemoveProductById(int produtoId);
    }
}
