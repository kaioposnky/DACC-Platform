using DaccApi.Model;
using DaccApi.Responses.UserResponse;
using DaccApi.Services.Projetos;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Projetos
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjetosController : ControllerBase
    {
        private readonly IProjetosService _projetosService;

        public ProjetosController(IProjetosService projetosService)
        {
            _projetosService = projetosService;
        }

        [HttpGet("GetProjetos")]
        [ProducesResponseType(typeof(UserResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetProjetos()
        {

            var projetos = _projetosService.GetAllProjetos();
            return projetos;
        }

    }
}
