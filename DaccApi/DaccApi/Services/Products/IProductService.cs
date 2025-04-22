using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Products
{
    public interface IProductService
    {

        public IActionResult GetAllProducts();
        public IActionResult GetProductById(RequestProduto requestProduto);
        public IActionResult CreateProduct(RequestProduto requestProduto);
        public IActionResult RemoveProductById(RequestProduto requestProduto);
    }
}
