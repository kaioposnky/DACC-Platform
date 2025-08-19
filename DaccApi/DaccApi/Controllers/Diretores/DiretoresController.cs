using DaccApi.Helpers.Attributes;
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
    [Route("v1/api/[controller]")]

    public class DiretoresController : ControllerBase
    {
        private readonly IDiretoresService _diretoresService;

        public DiretoresController(IDiretoresService diretoresService)
        {
            _diretoresService = diretoresService;
        }

        [PublicGetResponses]
        [HttpGet("")]
        [HasPermission(AppPermissions.Faculty.View)]
        public async Task<IActionResult> GetAllDiretores()
        {
            var response = await _diretoresService.GetAllDiretores();
            return response;
        }
        
        [PublicGetResponses]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDiretorById([FromRoute] Guid id)
        {
            var response = await _diretoresService.GetDiretorById(id);
            return response;
        }

        [AuthenticatedPostResponses]
        [HttpPost("")]
        public async Task<IActionResult> CreateDiretor([FromBody] RequestDiretor request)
        {
            var response = await _diretoresService.CreateDiretor(request);
            return response;
        }
        
        [AuthenticatedDeleteResponses]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDiretor([FromRoute] Guid id)
        {
            var response = await _diretoresService.DeleteDiretor(id);
            return response;
        }
        
        [AuthenticatedPatchResponses]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateDiretor([FromRoute] Guid id, [FromBody] RequestDiretor request)
        {
            var response = await _diretoresService.UpdateDiretor(id, request);
            return response;
        }
        
        

    }
}
