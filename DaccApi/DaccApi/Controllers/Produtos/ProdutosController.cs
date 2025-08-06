using DaccApi.Model;
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
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _produtosService.GetAllProductsAsync();
            return products;
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProductById([FromRoute] Guid id)
        {
            var products = await _produtosService.GetProductByIdAsync(id);
            return products;
        }

        [HttpPost("")]
        [HasPermission(AppPermissions.Produtos.Create)]
        public async Task<IActionResult> CreateProduct([FromBody] RequestCreateProduto requestCreateProduto)
        {
            var response = await _produtosService.CreateProductAsync(requestCreateProduto);
            return response;
        }

        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] RequestQueryProdutos query)
        {
            var response = await _produtosService.SearchProductsAsync(query);
            return response;
        }

        [HttpDelete("{id:guid}")]
        [HasPermission(AppPermissions.Produtos.Delete)]
        public async Task<IActionResult> RemoveProduct([FromRoute] Guid id)
        {
            var response = await _produtosService.RemoveProductByIdAsync(id);
            return response;
        }
        
        [HttpPatch("{id:guid}")]
        [HasPermission(AppPermissions.Produtos.Update)]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromForm] RequestUpdateProduto requestUpdateProduto)
        {
            var response = await _produtosService.UpdateProductAsync(id, requestUpdateProduto);
            return response;
        }

        [HttpPost("{id:guid}/variations")]
        [HasPermission(AppPermissions.Produtos.Create)]
        public async Task<IActionResult> CreateVariation([FromRoute] Guid id, [FromForm] RequestProdutoVariacaoCreate request)
        {
            var response = await _produtosService.CreateVariationAsync(id, request);
            return response;
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}/variations")]
        public async Task<IActionResult> GetVariations([FromRoute] Guid id)
        {
            var response = await _produtosService.GetVariationsAsync(id);
            return response;
        }
        
        [HttpPatch("{id:guid}/variations/{variationId:guid}")]
        [HasPermission(AppPermissions.Produtos.Update)]
        public async Task<IActionResult> UpdateVariation([FromRoute] Guid id, [FromRoute] Guid variationId, [FromForm] RequestUpdateProdutoVariacao request)
        {
            var response = await _produtosService.UpdateVariationAsync(id, variationId, request);
            return response;
        }
        
        [HttpDelete("{id:guid}/variations/{variationId:guid}")]
        [HasPermission(AppPermissions.Produtos.Delete)]
        public async Task<IActionResult> DeleteVariation([FromRoute] Guid id, [FromRoute] Guid variationId)
        {
            var response = await _produtosService.DeleteVariationAsync(id, variationId);
            return response;
        }

        [HttpPost("{productId:guid}/variations/{variationId:guid}/images")]
        [HasPermission(AppPermissions.Produtos.Create)]
        public async Task<IActionResult> CreateVariationImage(
            [FromRoute] Guid productId, [FromRoute] Guid variationId, 
            [FromForm] RequestCreateProdutoImagem request)
        {
            var response = await _produtosService.CreateVariationImageAsync(productId, variationId, request);
            return response;
        }
        
        [HttpGet("images/{imageId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImage([FromRoute] Guid imageId)
        {
            var response = await _produtosService.GetImageAsync(imageId);
            return response;
        }
        
        [HttpPatch("images/{imageId:guid}")]
        [HasPermission(AppPermissions.Produtos.Update)]
        public async Task<IActionResult> UpdateImage(
            [FromRoute] Guid imageId, [FromForm] RequestUpdateProdutoImagem request)
        {
            var response = await _produtosService.UpdateImageAsync(imageId, request);
            return response;
        }
        
        [HttpDelete("images/{imageId:guid}")]
        [HasPermission(AppPermissions.Produtos.Delete)]
        public async Task<IActionResult> DeleteImage([FromRoute] Guid imageId)
        {
            var response = await _produtosService.DeleteImageAsync(imageId);
            return response;
        }
    }
}