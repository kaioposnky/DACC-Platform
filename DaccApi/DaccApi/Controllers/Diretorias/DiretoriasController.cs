using DaccApi.Model;
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
        [ProducesResponseType(typeof(UserResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public List<Diretoria> GetDiretorias()
        {

            List<Diretoria> diretorias = _diretoriasService.GetDiretorias();

            return diretorias;
        }

    }
}
