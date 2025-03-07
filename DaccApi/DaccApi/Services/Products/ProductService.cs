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

        public List<Product> GetProducts()
        {
            try
            {
                List<Product> products = _productRepository.GetListProducts();

                return products;
            }
            catch (Exception ex) {

                throw ex;
            }
        } 
    }
}
