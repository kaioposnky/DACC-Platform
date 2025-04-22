using DaccApi.Helpers;
using DaccApi.Model;
using DaccApi.Responses;
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
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllProducts()
        {
            var products = _productService.GetAllProducts();
            return products;
        }

        [HttpPost("GetProductById")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult GetProductById([FromBody] RequestProduto requestProduto)
        {
            var products = _productService.GetProductById(requestProduto);
            return products;
        }


        [HttpGet("CreateProduct")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult CreateProduct(RequestProduto requestProduto)
        {
            var response = _productService.CreateProduct(requestProduto);
            return response;
        }

        [HttpGet("RemoveProductById")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult RemoveProductById(RequestProduto requestProduto)
        {
            var response = _productService.RemoveProductById(requestProduto);
            return response;
        }

    }
}
