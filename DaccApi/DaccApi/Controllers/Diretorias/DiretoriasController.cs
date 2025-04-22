using DaccApi.Responses;
using DaccApi.Responses.UserResponse;
using DaccApi.Services.Diretorias;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Products
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiretoriasController : ControllerBase
    {
        private readonly IDiretoriasService _diretoriasService;

        public DiretoriasController(IDiretoriasService diretoriasService)
        {
            _diretoriasService = diretoriasService;
        }

        [HttpGet("GetDiretorias")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult GetDiretorias()
        {
            var response = _diretoriasService.GetAllDiretorias();
            return response;
        }

    }
}
