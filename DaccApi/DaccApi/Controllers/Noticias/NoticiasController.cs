using DaccApi.Model;
using DaccApi.Responses;
using DaccApi.Responses.UserResponse;
using DaccApi.Services.Noticias;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Noticias
{
    [ApiController]
    [Route("api/news")]
    public class NoticiasController : ControllerBase
    {
        private readonly INoticiasServices _noticiasServices;
        public NoticiasController(INoticiasServices noticiasServices)
        {
            _noticiasServices = noticiasServices;
        }

        [HttpGet("")]
        public IActionResult GetAllNoticias()
        {
            var response = _noticiasServices.GetAllNoticias();
            return response;
        }
        
        [HttpPost("")]
        public IActionResult CreateNoticia([FromBody] RequestNoticia request)
        {
            var response = _noticiasServices.CreateNoticia(request);
            return response;
        }
        
        [HttpGet("{id:int}")]
        public IActionResult GetNoticiaById([FromRoute] int id)
        {
            var response = _noticiasServices.GetNoticiaById(id);
            return response;
        }
        
        [HttpDelete("{id:int}")]
        public IActionResult DeleteNoticia([FromRoute] int id)
        {
            var response = _noticiasServices.DeleteNoticia(id);
            return response;
        }
        
        [HttpPatch("{id:int}")]
        public IActionResult UpdateNoticia([FromRoute] int id, [FromBody] RequestNoticia request)
        {
            var response = _noticiasServices.UpdateNoticia(id, request);
            return response;
        }
        
        
        
    }
}

