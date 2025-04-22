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

        [HttpGet("GetAllNoticias")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllNoticias()
        {
            var response = _noticiasServices.GetAllNoticias();
            return response;
        }
    }
}

