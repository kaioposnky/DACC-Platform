using DaccApi.Helpers.Attributes;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model;
using DaccApi.Services.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Produtos
{
    /// <summary>
    /// Controlador para gerenciar produtos, suas variações e imagens.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/api/products")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosService _produtosService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProdutosController"/>.
        /// </summary>
        public ProdutosController(IProdutosService produtosService)
        {
            _produtosService = produtosService;
        }

        /// <summary>
        /// Obtém todos os produtos.
        /// </summary>
        [PublicGetResponses]
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _produtosService.GetAllProductsAsync();
            return products;
        }

        /// <summary>
        /// Obtém um produto específico pelo seu ID.
        /// </summary>
        [PublicGetResponses]
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProductById([FromRoute] Guid id)
        {
            var products = await _produtosService.GetProductByIdAsync(id);
            return products;
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        [AuthenticatedPostResponses]
        [HttpPost("")]
        [HasPermission(AppPermissions.Produtos.Create)]
        public async Task<IActionResult> CreateProduct(
            [FromBody] RequestCreateProduto requestCreateProduto
        )
        {
            var response = await _produtosService.CreateProductAsync(requestCreateProduto);
            return response;
        }

        /// <summary>
        /// Busca produtos com base em filtros de consulta.
        /// </summary>
        [PaginatedListResponses]
        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] RequestQueryProdutos query)
        {
            var response = await _produtosService.SearchProductsAsync(query);
            return response;
        }

        /// <summary>
        /// Remove um produto existente.
        /// </summary>
        [AuthenticatedDeleteResponses]
        [HttpDelete("{id:guid}")]
        [HasPermission(AppPermissions.Produtos.Delete)]
        public async Task<IActionResult> RemoveProduct([FromRoute] Guid id)
        {
            var response = await _produtosService.RemoveProductByIdAsync(id);
            return response;
        }

        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        [AuthenticatedPatchResponses]
        [HttpPatch("{id:guid}")]
        [HasPermission(AppPermissions.Produtos.Update)]
        public async Task<IActionResult> UpdateProduct(
            [FromRoute] Guid id,
            [FromForm] RequestUpdateProduto requestUpdateProduto
        )
        {
            var response = await _produtosService.UpdateProductAsync(id, requestUpdateProduto);
            return response;
        }

        /// <summary>
        /// Cria uma nova variação para um produto.
        /// </summary>
        [AuthenticatedPostResponses]
        [HttpPost("{id:guid}/variations")]
        [HasPermission(AppPermissions.Produtos.Create)]
        public async Task<IActionResult> CreateVariation(
            [FromRoute] Guid id,
            [FromForm] RequestProdutoVariacaoCreate request
        )
        {
            var response = await _produtosService.CreateVariationAsync(id, request);
            return response;
        }

        /// <summary>
        /// Obtém todas as variações de um produto.
        /// </summary>
        [PublicGetResponses]
        [AllowAnonymous]
        [HttpGet("{id:guid}/variations")]
        public async Task<IActionResult> GetVariations([FromRoute] Guid id)
        {
            var response = await _produtosService.GetVariationsAsync(id);
            return response;
        }

        /// <summary>
        /// Atualiza uma variação de produto existente.
        /// </summary>
        [AuthenticatedPatchResponses]
        [HttpPatch("{id:guid}/variations/{variationId:guid}")]
        [HasPermission(AppPermissions.Produtos.Update)]
        public async Task<IActionResult> UpdateVariation(
            [FromRoute] Guid id,
            [FromRoute] Guid variationId,
            [FromForm] RequestUpdateProdutoVariacao request
        )
        {
            var response = await _produtosService.UpdateVariationAsync(id, variationId, request);
            return response;
        }

        /// <summary>
        /// Deleta uma variação de produto existente.
        /// </summary>
        [AuthenticatedDeleteResponses]
        [HttpDelete("{id:guid}/variations/{variationId:guid}")]
        [HasPermission(AppPermissions.Produtos.Delete)]
        public async Task<IActionResult> DeleteVariation(
            [FromRoute] Guid id,
            [FromRoute] Guid variationId
        )
        {
            var response = await _produtosService.DeleteVariationAsync(id, variationId);
            return response;
        }

        /// <summary>
        /// Adiciona uma nova imagem a uma variação de produto.
        /// </summary>
        [FileUploadResponses]
        [HttpPost("{productId:guid}/variations/{variationId:guid}/images")]
        [HasPermission(AppPermissions.Produtos.Create)]
        public async Task<IActionResult> CreateVariationImage(
            [FromRoute] Guid productId,
            [FromRoute] Guid variationId,
            [FromForm] RequestCreateProdutoImagem request
        )
        {
            var response = await _produtosService.CreateVariationImageAsync(
                productId,
                variationId,
                request
            );
            return response;
        }

        /// <summary>
        /// Obtém uma imagem específica pelo seu ID.
        /// </summary>
        [PublicGetResponses]
        [HttpGet("images/{imageId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImage([FromRoute] Guid imageId)
        {
            var response = await _produtosService.GetImageAsync(imageId);
            return response;
        }

        /// <summary>
        /// Atualiza os detalhes de uma imagem de produto.
        /// </summary>
        [FileUploadResponses]
        [HttpPatch("images/{imageId:guid}")]
        [HasPermission(AppPermissions.Produtos.Update)]
        public async Task<IActionResult> UpdateImage(
            [FromRoute] Guid imageId,
            [FromForm] RequestUpdateProdutoImagem request
        )
        {
            var response = await _produtosService.UpdateImageAsync(imageId, request);
            return response;
        }

        /// <summary>
        /// Deleta uma imagem de produto.
        /// </summary>
        [AuthenticatedDeleteResponses]
        [HttpDelete("images/{imageId:guid}")]
        [HasPermission(AppPermissions.Produtos.Delete)]
        public async Task<IActionResult> DeleteImage([FromRoute] Guid imageId)
        {
            var response = await _produtosService.DeleteImageAsync(imageId);
            return response;
        }
    }
}
