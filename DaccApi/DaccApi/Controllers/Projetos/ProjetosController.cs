using DaccApi.Model;
using DaccApi.Responses;
using DaccApi.Responses.UserResponse;
using DaccApi.Services.Projetos;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Projetos
{
    [ApiController]
    [Route("api/projects")]
    public class ProjetosController : ControllerBase
    {
        private readonly IProjetosService _projetosService;

        public ProjetosController(IProjetosService projetosService)
        {
            _projetosService = projetosService;
        }

        [HttpGet("")]
        public IActionResult GetProjetos()
        {

            var projetos = _projetosService.GetAllProjetos();
            return projetos;
        }

        [HttpPost("")]
        public IActionResult CreateProjeto([FromBody] object request)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{id:int}")]
        public IActionResult UpdateProjeto([FromRoute] int id, [FromBody] object request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteProjeto([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("{projectId:int}/members/{userId:int}")]

        public IActionResult AddProjetoMember([FromRoute] int projectId, int userId)
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete("{projectId:int}/members/{userId:int}")]
        public IActionResult DeleteProjetoMember([FromRoute] int projectId, int userId)
        {
            throw new NotImplementedException();
        }
        
    }
}
