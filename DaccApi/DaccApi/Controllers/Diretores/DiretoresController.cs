using DaccApi.Responses;
using DaccApi.Responses.UserResponse;
using DaccApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Services.Diretores;

namespace DaccApi.Controllers.Diretores
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class DiretoresController : ControllerBase
    {
        private readonly IDiretoresService _diretoresService;

        public DiretoresController(IDiretoresService diretoresService)
        {
            _diretoresService = diretoresService;
        }

        [HttpGet("")]
        [HasPermission(AppPermissions.Faculty.View)]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllDiretores()
        {
            var response = _diretoresService.GetAllDiretores();
            return response;
        }
        [HttpGet("{id:int}")]
        public IActionResult GetDiretorById([FromRoute] int id)
        {
            var response = _diretoresService.GetDiretorById(id);
            return response;
        }

        
        [HttpPost("")]
        public IActionResult CreateDiretor([FromBody] RequestDiretor request)
        {
            var response = _diretoresService.CreateDiretor(request);
            return response;
        }
        

        [HttpDelete("{id:int}")]
        public IActionResult DeleteDiretor([FromRoute] int id)
        {
            var response = _diretoresService.DeleteDiretor(id);
            return response;
        }
        
        [HttpPatch("{id:int}")]

        public IActionResult UpdateDiretor([FromRoute] int id, [FromBody] RequestDiretor request)
        {
            var response = _diretoresService.UpdateDiretor(id, request);
            return response;
        }
        
        

    }
}
