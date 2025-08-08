using DaccApi.Responses;
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
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDiretores()
        {
            var response = await _diretoresService.GetAllDiretores();
            return response;
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDiretorById([FromRoute] Guid id)
        {
            var response = await _diretoresService.GetDiretorById(id);
            return response;
        }

        
        [HttpPost("")]
        public async Task<IActionResult> CreateDiretor([FromBody] RequestDiretor request)
        {
            var response = await _diretoresService.CreateDiretor(request);
            return response;
        }
        

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDiretor([FromRoute] Guid id)
        {
            var response = await _diretoresService.DeleteDiretor(id);
            return response;
        }
        
        [HttpPatch("{id:guid}")]

        public async Task<IActionResult> UpdateDiretor([FromRoute] Guid id, [FromBody] RequestDiretor request)
        {
            var response = await _diretoresService.UpdateDiretor(id, request);
            return response;
        }
        
        

    }
}
