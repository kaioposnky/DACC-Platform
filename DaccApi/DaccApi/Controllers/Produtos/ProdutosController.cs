using DaccApi.Helpers;
using DaccApi.Model;
using DaccApi.Responses;
using DaccApi.Responses.UserResponse;
using DaccApi.Services.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;

namespace DaccApi.Controllers.Produtos
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosService _produtosService;

        public ProdutosController(IProdutosService produtosService)
        {
            _produtosService = produtosService;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult GetAllProducts()
        {
            var products = _produtosService.GetAllProducts();
            return products;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetProductById([FromRoute] int id)
        {
            var products = _produtosService.GetProductById(id);
            return products;
        }


        [HttpPost("")]
        [HasPermission(AppPermissions.Produtos.Create)]
        public IActionResult CreateProduct([FromBody] RequestProduto requestProduto)
        {
            var response = _produtosService.CreateProduct(requestProduto);
            return response;
        }

        [HttpDelete("{id:int}")]
        [HasPermission(AppPermissions.Produtos.Delete)]
        public IActionResult RemoveProduct([FromRoute] int id)
        {
            var response = _produtosService.RemoveProductById(id);
            return response;
        }
        
        [HttpDelete("{id:int}")]
        [HasPermission(AppPermissions.Produtos.Update)]
        public IActionResult UpdateProduct([FromRoute] int id, [FromBody] RequestProduto requestProduto)
        {
            throw new NotImplementedException();
        }

    }
}
