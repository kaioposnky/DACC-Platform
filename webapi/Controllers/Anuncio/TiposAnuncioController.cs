using DaccApi.Helpers.Attributes;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model.Requests.Anuncio;
using DaccApi.Services.Anuncios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Anuncio
{
    [Authorize]
    [ApiController]
    [Route("v1/api/announcements/types")]
    public class TiposAnuncioController : ControllerBase
    {
        private readonly ITipoAnuncioService _tipoAnuncioService;

        public TiposAnuncioController(ITipoAnuncioService tipoAnuncioService)
        {
            _tipoAnuncioService = tipoAnuncioService;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            return await _tipoAnuncioService.GetAllTiposAnuncio();
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            return await _tipoAnuncioService.GetTipoAnuncioById(id);
        }

        [AuthenticatedPostResponses]
        [HasPermission(AppPermissions.Anuncios.TiposAnuncio.Create)]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] RequestCreateTipoAnuncio request)
        {
            return await _tipoAnuncioService.CreateTipoAnuncio(request);
        }

        [AuthenticatedPatchResponses]
        [HasPermission(AppPermissions.Anuncios.TiposAnuncio.Update)]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RequestUpdateTipoAnuncio request)
        {
            return await _tipoAnuncioService.UpdateTipoAnuncio(id, request);
        }

        [AuthenticatedDeleteResponses]
        [HasPermission(AppPermissions.Anuncios.TiposAnuncio.Delete)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return await _tipoAnuncioService.DeleteTipoAnuncio(id);
        }
    }
}
