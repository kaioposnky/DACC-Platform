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
        public IActionResult GetAllNoticias()
        {
            var response = _noticiasServices.GetAllNoticias();
            return response;
        }
        
        [HttpPost("")]
        [HasPermission(AppPermissions.Noticias.Create)]
        public IActionResult CreateNoticia([FromBody] RequestNoticia request)
        {
            var response = _noticiasServices.CreateNoticia(request);
            return response;
        }
        
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetNoticiaById([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete("{id:int}")]
        [HasPermission(AppPermissions.Noticias.Delete)]
        public IActionResult DeleteNoticia([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
        
        [HttpPatch("{id:int}")]
        [HasPermission(AppPermissions.Noticias.Update)]
        public IActionResult UpdateNoticia([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
    }
}

