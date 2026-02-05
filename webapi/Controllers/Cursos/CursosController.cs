using DaccApi.Helpers.Attributes;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model.Requests.Curso;
using DaccApi.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Cursos
{
    [ApiController]
    [Route("v1/api/cursos")]
    public class CursosController : ControllerBase
    {
        private readonly ICursoService _cursoService;

        public CursosController(ICursoService cursoService)
        {
            _cursoService = cursoService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            return await _cursoService.GetAllCursos();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            return await _cursoService.GetCursoById(id);
        }

        [AuthenticatedPostResponses]
        [HasPermission(AppPermissions.Cursos.Create)]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] RequestCreateCurso request)
        {
            return await _cursoService.CreateCurso(request);
        }

        [AuthenticatedPatchResponses]
        [HasPermission(AppPermissions.Cursos.Update)]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RequestUpdateCurso request)
        {
            return await _cursoService.UpdateCurso(id, request);
        }

        [AuthenticatedDeleteResponses]
        [HasPermission(AppPermissions.Cursos.Delete)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return await _cursoService.DeleteCurso(id);
        }
    }
}
