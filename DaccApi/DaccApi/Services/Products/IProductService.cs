using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Products
{
    public interface IProductService
    {

        public IActionResult GetAllProducts();
        public IActionResult GetProductById(RequestProduto requestProduto);
    }
}
