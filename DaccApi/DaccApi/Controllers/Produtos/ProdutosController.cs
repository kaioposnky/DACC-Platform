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
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosService _produtosService;

        public ProdutosController(IProdutosService produtosService)
        {
            _produtosService = produtosService;
        }

        [HttpGet("GetAllProducts")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllProducts()
        {
            var products = _produtosService.GetAllProducts();
            return products;
        }

        [HttpPost("GetProductById")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult GetProductById([FromBody] RequestProduto requestProduto)
        {
            var products = _produtosService.GetProductById(requestProduto);
            return products;
        }


        [HttpGet("CreateProduct")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult CreateProduct(RequestProduto requestProduto)
        {
            var response = _produtosService.CreateProduct(requestProduto);
            return response;
        }

        [HttpGet("RemoveProductById")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult RemoveProductById(RequestProduto requestProduto)
        {
            var response = _produtosService.RemoveProductById(requestProduto);
            return response;
        }

    }
}
