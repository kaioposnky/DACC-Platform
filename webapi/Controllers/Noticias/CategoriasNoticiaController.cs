using DaccApi.Helpers.Attributes;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model.Requests.Noticias;
using DaccApi.Services.Noticias;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Noticias
{
    [ApiController]
    [Route("v1/api/news/categories")]
    public class CategoriasNoticiaController : ControllerBase
    {
        private readonly ICategoriaNoticiaService _service;

        public CategoriasNoticiaController(ICategoriaNoticiaService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll() => await _service.GetAll();

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) => await _service.GetById(id);

        [AuthenticatedPostResponses]
        [HasPermission(AppPermissions.Noticias.Categorias.Create)]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] RequestCreateCategoriaNoticia request) => await _service.Create(request);

        [AuthenticatedPatchResponses]
        [HasPermission(AppPermissions.Noticias.Categorias.Update)]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RequestUpdateCategoriaNoticia request) => await _service.Update(id, request);

        [AuthenticatedDeleteResponses]
        [HasPermission(AppPermissions.Noticias.Categorias.Delete)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) => await _service.Delete(id);
    }
}
