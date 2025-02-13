using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Products
{
    public interface IProductService
    {

        public Task<List<Product>> GetProducts();
        public Product GetProductById(int ProductId);
        public String AddProduct(string name, string description, byte[] imageUrl, double price, int id);
        public String RemoveProductById(int productId);
        public String AddProductRating(int productId, int userId, string? comment, float score);
    }
}
