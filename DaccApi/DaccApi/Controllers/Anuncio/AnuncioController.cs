using DaccApi.Helpers.Attributes;
using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Services.Anuncios;

namespace DaccApi.Controllers.Anuncio
{
    [Authorize]
    [ApiController]
    [Route("v1/api/announcements")]
    public class AnuncioController : ControllerBase
    {
        private readonly IAnuncioService _anuncioService;

        public AnuncioController(IAnuncioService anuncioService)
        {
            _anuncioService = anuncioService;
        }

        [PublicGetResponses]
        [HttpGet("")]
        public async Task<IActionResult> GetAllAnuncio()
        {
            var response = await _anuncioService.GetAllAnuncio();
            return response;
        }
        
        [PublicGetResponses]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAnunciosById([FromRoute] Guid id)
        {
            var response = await _anuncioService.GetAnuncioById(id);
            return response;
        }
        
        [AuthenticatedPostResponses]
        [HttpPost("")]
        public async Task<IActionResult> CreateAnuncio([FromBody] RequestAnuncio anuncio)
        {
            var response = await _anuncioService.CreateAnuncio(anuncio);
            return response;
        }
        
        [AuthenticatedDeleteResponses]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAnuncio([FromRoute] Guid id)
        {
            var response = await _anuncioService.DeleteAnuncio(id);
            return response;
        }
        
        [AuthenticatedPatchResponses]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateAnuncio([FromRoute] Guid id, [FromBody] RequestAnuncio anuncio)
        {
            var response = await _anuncioService.UpdateAnuncio(id, anuncio);
            return response;
        }



    }
}