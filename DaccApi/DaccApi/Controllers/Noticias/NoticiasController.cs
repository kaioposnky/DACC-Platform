using DaccApi.Model;
using DaccApi.Responses;
using DaccApi.Responses.UserResponse;
using DaccApi.Services.Noticias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;

namespace DaccApi.Controllers.Noticias
{
    [Authorize]
    [ApiController]
    [Route("api/news")]
    public class NoticiasController : ControllerBase
    {
        private readonly INoticiasServices _noticiasServices;
        public NoticiasController(INoticiasServices noticiasServices)
        {
            _noticiasServices = noticiasServices;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetAllNoticias()
        {
            var response = await _noticiasServices.GetAllNoticias();
            return response;
        }
        
        [HttpPost("")]
        [HasPermission(AppPermissions.Noticias.Create)]
        public async Task<IActionResult> CreateNoticia([FromBody] RequestNoticia request)
        {
            var response = await _noticiasServices.CreateNoticia(request);
            return response;
        }
        
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetNoticiaById([FromRoute] int id)
        {
            var response = await _noticiasServices.GetNoticiaById(id);
            return response;
        }
        
        [HttpDelete("{id:int}")]
        [HasPermission(AppPermissions.Noticias.Delete)]
        public async Task<IActionResult> DeleteNoticia([FromRoute] int id)
        {
            var response = await _noticiasServices.DeleteNoticia(id);
            return response;
        }
        
        [HttpPatch("{id:int}")]
        [HasPermission(AppPermissions.Noticias.Update)]
        public async Task<IActionResult> UpdateNoticia([FromRoute] int id, [FromBody] RequestNoticia request)
        {
            var response = await _noticiasServices.UpdateNoticia(id, request);
            return response;
        }
    }
}

