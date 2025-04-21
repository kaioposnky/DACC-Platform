using DaccApi.Helpers;
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

        [HttpGet("GetAllProducts")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllProducts()
        {
            var products = _productService.GetAllProducts();
            return products;
        }

        [HttpPost("GetProductById")]
        [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetProductById([FromBody] RequestProduto requestProduto)
        {
            var products = _productService.GetProductById(requestProduto);
            return products;
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
        public String RemoveProductById(Guid? productId)
        {

            String response = _productService.RemoveProductById(productId);

            return response;
        }

        [HttpGet("AddProductRating")]
        [ProducesResponseType(typeof(UserResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public String AddProductRating(int productId, int userId, string? comment, float rating)
        {

            String response = _productService.AddProductRating(productId, userId, comment, rating);

            return response;
        }

    }
}
