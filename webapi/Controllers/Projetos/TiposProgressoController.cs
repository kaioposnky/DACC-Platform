using DaccApi.Helpers.Attributes;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model.Requests.Projetos;
using DaccApi.Services.Projetos;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Projetos
{
    [ApiController]
    [Route("v1/api/projects/progress-types")]
    public class TiposProgressoController : ControllerBase
    {
        private readonly ITipoProgressoService _service;

        public TiposProgressoController(ITipoProgressoService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll() => await _service.GetAll();

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) => await _service.GetById(id);

        [AuthenticatedPostResponses]
        [HasPermission(AppPermissions.Projetos.TiposProgresso.Create)]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] RequestCreateTipoProgresso request) => await _service.Create(request);

        [AuthenticatedPatchResponses]
        [HasPermission(AppPermissions.Projetos.TiposProgresso.Update)]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RequestUpdateTipoProgresso request) => await _service.Update(id, request);

        [AuthenticatedDeleteResponses]
        [HasPermission(AppPermissions.Projetos.TiposProgresso.Delete)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) => await _service.Delete(id);
    }
}
