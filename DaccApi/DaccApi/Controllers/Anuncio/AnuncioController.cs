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

        [HttpGet("")]
        public async Task<IActionResult> GetAllAnuncio()
        {
            var response = await _anuncioService.GetAllAnuncio();
            return response;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAnunciosById([FromRoute] Guid id)
        {
            var response = await _anuncioService.GetAnuncioById(id);
            return response;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAnuncio([FromBody] RequestAnuncio anuncio)
        {
            var response = await _anuncioService.CreateAnuncio(anuncio);
            return response;
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAnuncio([FromRoute] Guid id)
        {
            var response = await _anuncioService.DeleteAnuncio(id);
            return response;
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateAnuncio([FromRoute] Guid id, [FromForm] RequestAnuncio anuncio)
        {
            var response = await _anuncioService.UpdateAnuncio(id, anuncio);
            return response;
        }



    }
}