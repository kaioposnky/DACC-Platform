using DaccApi.Helpers.Attributes;
using DaccApi.Model;
using DaccApi.Responses;
using DaccApi.Services.Projetos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;

namespace DaccApi.Controllers.Projetos
{
    [Authorize]
    [ApiController]
    [Route("v1/api/projects")]
    public class ProjetosController : ControllerBase
    {
        private readonly IProjetosService _projetosService;

        public ProjetosController(IProjetosService projetosService)
        {
            _projetosService = projetosService;
        }

        [PublicGetResponses]
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetAllProjetos()
        {

            var response = await _projetosService.GetAllProjetos();
            return response;
        }

        [PublicGetResponses]
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProjetoById(Guid id)
        {
            var response = await _projetosService.GetProjetoById(id);
            return response;
        }

        [AuthenticatedPostResponses]
        [HttpPost("")]
        [HasPermission(AppPermissions.Projetos.Create)]
        public async Task<IActionResult> CreateProjeto([FromBody] RequestProjeto projeto)
        {
            var response = await _projetosService.CreateProjeto(projeto);
            return response;
        }

        [AuthenticatedPatchResponses]
        [HttpPatch("{id:guid}")]
        [HasPermission(AppPermissions.Projetos.Update)]
        public async Task<IActionResult> UpdateProjeto([FromRoute] Guid id, [FromForm] RequestProjeto projeto)
        {
            var response = await _projetosService.UpdateProjeto(id, projeto);
            return response;
        }

        [AuthenticatedDeleteResponses]
        [HttpDelete("{id:guid}")]
        [HasPermission(AppPermissions.Projetos.Delete)]
        public async Task<IActionResult> DeleteProjeto([FromRoute] Guid id)
        {
            var response = await _projetosService.DeleteProjeto(id);
            return response;
        }
        
    }
}
