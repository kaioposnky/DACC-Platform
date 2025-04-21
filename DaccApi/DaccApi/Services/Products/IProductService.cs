using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Products
{
    public interface IProductService
    {

        public IActionResult GetAllProducts();
        public IActionResult GetProductById(RequestProduto requestProduto);
        public String AddProduct(string name, string description, byte[] imageUrl, double price, int id);
        public String RemoveProductById(Guid? productId);
        public String AddProductRating(int productId, int userId, string? comment, float score);
    }
}
