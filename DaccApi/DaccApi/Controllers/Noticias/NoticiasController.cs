using DaccApi.Responses;
using DaccApi.Responses.UserResponse;
using DaccApi.Services.Noticias;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Noticias
{
    [ApiController]
    [Route("api/[controller]")]
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
        public IActionResult CreateNoticia()
        {
            throw new NotImplementedException();
        }
        
        [HttpGet("{id:int}")]
        public IActionResult GetNoticiaById([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete("{id:int}")]
        public IActionResult DeleteNoticia([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
        
        [HttpPatch("{id:int}")]
        public IActionResult UpdateNoticia([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
        
        
        
    }
}

