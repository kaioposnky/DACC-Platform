using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Products
{
    public interface IProductService
    {

        public Task<List<Product>> GetProducts();
    }
}
