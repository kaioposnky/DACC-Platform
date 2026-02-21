using DaccApi.Helpers.Attributes;
using DaccApi.Services.Produtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Produtos
{
    /// <summary>
    /// Controller para gerenciar categorias e subcategorias de produtos.
    /// </summary>
    [ApiController]
    [Route("v1/api/products")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        /// <summary>
        /// Obtém todas as categorias de produtos.
        /// </summary>
        [AllowAnonymous]
        [PublicGetResponses]
        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategorias()
        {
            return await _categoriaService.GetAllCategoriasAsync();
        }

        /// <summary>
        /// Obtém todas as subcategorias de produtos.
        /// </summary>
        [AllowAnonymous]
        [PublicGetResponses]
        [HttpGet("subcategories")]
        public async Task<IActionResult> GetAllSubcategorias()
        {
            return await _categoriaService.GetAllSubcategoriasAsync();
        }
    }
}
