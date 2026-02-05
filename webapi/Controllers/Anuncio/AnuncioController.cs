using DaccApi.Helpers;
using DaccApi.Helpers.Attributes;
using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model.Requests;
using DaccApi.Services.Anuncios;

namespace DaccApi.Controllers.Anuncio
{
    /// <summary>
    /// Controlador para gerenciar anúncios.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/api/announcements")]
    public class AnuncioController : ControllerBase
    {
        private readonly IAnuncioService _anuncioService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AnuncioController"/>.
        /// </summary>
        public AnuncioController(IAnuncioService anuncioService)
        {
            _anuncioService = anuncioService;
        }

        /// <summary>
        /// Obtém todos os anúncios.
        /// </summary>
        [AllowAnonymous]
        [PublicGetResponses]
        [HttpGet("")]
        public async Task<IActionResult> GetAllAnuncio()
        {
            var response = await _anuncioService.GetAllAnuncio();
            return response;
        }

        [AllowAnonymous]
        [PublicGetResponses]
        [HttpGet("search")]
        public async Task<IActionResult> SearchAnuncio([FromQuery] RequestQueryAnuncio query)
        {
            var response = await _anuncioService.SearchAnuncio(query);
            return response;
        }

        /// <summary>
        /// Obtém um anúncio específico pelo seu ID.
        /// </summary>
        [AllowAnonymous]
        [PublicGetResponses]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAnunciosById([FromRoute] Guid id)
        {
            var response = await _anuncioService.GetAnuncioById(id);
            return response;
        }

        /// <summary>
        /// Cria um novo anúncio.
        /// </summary>
        [AuthenticatedPostResponses]
        [HasPermission(AppPermissions.Anuncios.Create)]
        [HttpPost("")]
        public async Task<IActionResult> CreateAnuncio([FromBody] RequestAnuncio anuncio)
        {
            var userId = ClaimsHelper.GetUserId(User);
            var response = await _anuncioService.CreateAnuncio(anuncio, userId);
            return response;
        }

        /// <summary>
        /// Adiciona uma imagem a um anúncio existente.
        /// </summary>
        [HttpPost("{id:guid}")]
        [HasPermission(AppPermissions.Anuncios.Update)]
        [AuthenticatedDeleteResponses]
        public async Task<IActionResult> AddAnuncioImage([FromRoute] Guid id, [FromBody] ImageRequest request)
        {
            var response = await _anuncioService.AddAnuncioImage(id, request);
            return response;
        }

        /// <summary>
        /// Deleta um anúncio existente.
        /// </summary>
        [AuthenticatedDeleteResponses]
        [HasPermission(AppPermissions.Anuncios.Delete)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAnuncio([FromRoute] Guid id)
        {
            var response = await _anuncioService.DeleteAnuncio(id);
            return response;
        }

        /// <summary>
        /// Atualiza um anúncio existente.
        /// </summary>
        [AuthenticatedPatchResponses]
        [HttpPatch("{id}")]
        [HasPermission(AppPermissions.Anuncios.Update)]
        public async Task<IActionResult> UpdateAnuncio(Guid id, [FromBody] RequestAnuncio request)
        {
            var response = await _anuncioService.UpdateAnuncio(id, request);
            return response;
        }
    }
}
