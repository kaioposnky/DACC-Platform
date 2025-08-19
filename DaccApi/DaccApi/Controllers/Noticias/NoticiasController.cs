using DaccApi.Helpers;
using DaccApi.Helpers.Attributes;
using DaccApi.Model;
using DaccApi.Responses;
using DaccApi.Services.Noticias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model.Requests;

namespace DaccApi.Controllers.Noticias
{
    [Authorize]
    [ApiController]
    [Route("v1/api/news")]
    public class NoticiasController : ControllerBase
    {
        private readonly INoticiasServices _noticiasServices;
        public NoticiasController(INoticiasServices noticiasServices)
        {
            _noticiasServices = noticiasServices;
        }

        [PublicGetResponses]
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetAllNoticias()
        {
            var response = await _noticiasServices.GetAllNoticias();
            return response;
        }
        
        [AuthenticatedPostResponses]
        [HttpPost("")]
        [HasPermission(AppPermissions.Noticias.Create)]
        public async Task<IActionResult> CreateNoticia([FromBody] RequestNoticia request)
        {
            var autorId = ClaimsHelper.GetUserId(User);
            var response = await _noticiasServices.CreateNoticia(autorId, request);
            return response;
        }

        [HttpPost("{id:guid}")]
        [HasPermission(AppPermissions.Noticias.Update)]
        public async Task<IActionResult> AddNoticiaImage([FromRoute] Guid id, [FromForm] ImageRequest request)
        {
            var response = await _noticiasServices.AddNoticiaImage(id, request);
            return response;
        }
        
        [PublicGetResponses]
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetNoticiaById([FromRoute] Guid id)
        {
            var response = await _noticiasServices.GetNoticiaById(id);
            return response;
        }
        
        [AuthenticatedDeleteResponses]
        [HttpDelete("{id:guid}")]
        [HasPermission(AppPermissions.Noticias.Delete)]
        public async Task<IActionResult> DeleteNoticia([FromRoute] Guid id)
        {
            var response = await _noticiasServices.DeleteNoticia(id);
            return response;
        }
        
        [AuthenticatedPatchResponses]
        [HttpPatch("{id:guid}")]
        [HasPermission(AppPermissions.Noticias.Update)]
        public async Task<IActionResult> UpdateNoticia([FromRoute] Guid id, [FromForm] RequestNoticia request)
        {
            var response = await _noticiasServices.UpdateNoticia(id, request);
            return response;
        }
    }
}

