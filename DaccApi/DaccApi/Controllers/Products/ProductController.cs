using DaccApi.Model;
using DaccApi.Responses.UserResponse;
using DaccApi.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Products
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetProducts")]
        [ProducesResponseType(typeof(UserResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<List<Product>> GetProducts()
        {

            Task<List<Product>> products = _productService.GetProducts();

            return products;
        }

        [HttpGet("GetProductById")]
        [ProducesResponseType(typeof(UserResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Product GetProductById(int productId)
        {

            Product product = _productService.GetProductById(productId);

            return product;
        }

        [HttpGet("AddProduct")]
        [ProducesResponseType(typeof(UserResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public String AddProduct(string name, string description, byte[] imageUrl, double price, int id)
        {

            String response = _productService.AddProduct(name, description, imageUrl, price, id);

            return response;
        }

        [HttpGet("RemoveProductById")]
        [ProducesResponseType(typeof(UserResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public String RemoveProductById(int productId)
        {

            String response = _productService.RemoveProductById(productId);

            return response;
        }
    }
}
