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
        public IActionResult GetProjetos()
        {

            var projetos = _projetosService.GetAllProjetos();
            return projetos;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetProjeto(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("")]
        [HasPermission(AppPermissions.Projetos.Create)]
        public IActionResult CreateProjeto([FromBody] object request)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{id:int}")]
        [HasPermission(AppPermissions.Projetos.Update)]
        public IActionResult UpdateProjeto([FromRoute] int id, [FromBody] object request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        [HasPermission(AppPermissions.Projetos.Delete)]
        public IActionResult DeleteProjeto([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("{projectId:int}/members/{userId:int}")]
        [HasPermission(AppPermissions.Projetos.AddMembers)]
        public IActionResult AddProjetoMember([FromRoute] int projectId, int userId)
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete("{projectId:int}/members/{userId:int}")]
        [HasPermission(AppPermissions.Projetos.RemoveMembers)]
        public IActionResult DeleteProjetoMember([FromRoute] int projectId, int userId)
        {
            throw new NotImplementedException();
        }
        
    }
}
