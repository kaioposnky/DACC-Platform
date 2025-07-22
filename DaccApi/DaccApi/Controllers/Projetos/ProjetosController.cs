using DaccApi.Model;
using DaccApi.Responses;
using DaccApi.Responses.UserResponse;
using DaccApi.Services.Projetos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;

namespace DaccApi.Controllers.Projetos
{
    [Authorize]
    [ApiController]
    [Route("api/projects")]
    public class ProjetosController : ControllerBase
    {
        private readonly IProjetosService _projetosService;

        public ProjetosController(IProjetosService projetosService)
        {
            _projetosService = projetosService;
        }

        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult GetAllProjetos()
        {

            var response = _projetosService.GetAllProjetos();
            return response;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetProjetoById(int id)
        {
            var response = _projetosService.GetProjetoById(id);
            return response;
        }

        [HttpPost("")]
        [HasPermission(AppPermissions.Projetos.Create)]
        public IActionResult CreateProjeto([FromBody] RequestProjeto projeto)
        {
            var response = _projetosService.CreateProjeto(projeto);
            return response;
        }

        [HttpPatch("{id:int}")]
        [HasPermission(AppPermissions.Projetos.Update)]
        public IActionResult UpdateProjeto([FromRoute] int id, [FromBody] RequestProjeto projeto)
        {
            var response = _projetosService.UpdateProjeto(id, projeto);
            return response;
        }

        [HttpDelete("{id:int}")]
        [HasPermission(AppPermissions.Projetos.Delete)]
        public IActionResult DeleteProjeto([FromRoute] int id)
        {
            var response = _projetosService.DeleteProjeto(id);
            return response;
        }
        
        [HttpPost("{projectId:int}/members/{userId:int}")]
        [HasPermission(AppPermissions.Projetos.AddMembers)]
        public IActionResult AddProjetoMember([FromRoute] int projectId, int userId)
        {
            var response = _projetosService.AddProjetoMember(projectId, userId);
            return response;
        }
        
        [HttpDelete("{projectId:int}/members/{userId:int}")]
        [HasPermission(AppPermissions.Projetos.RemoveMembers)]
        public IActionResult DeleteProjetoMember([FromRoute] int projectId, int userId)
        {
            var response = _projetosService.DeleteProjetoMember(projectId, userId);
            return response;
        }
        
    }
}
