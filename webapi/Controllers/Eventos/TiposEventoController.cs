using DaccApi.Helpers.Attributes;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model.Requests.Evento;
using DaccApi.Services.Eventos;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Eventos
{
    [ApiController]
    [Route("v1/api/events/types")]
    public class TiposEventoController : ControllerBase
    {
        private readonly ITipoEventoService _service;

        public TiposEventoController(ITipoEventoService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll() => await _service.GetAll();

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id) => await _service.GetById(id);

        [AuthenticatedPostResponses]
        [HasPermission(AppPermissions.Eventos.TiposEvento.Create)]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] RequestCreateTipoEvento request) => await _service.Create(request);

        [AuthenticatedPatchResponses]
        [HasPermission(AppPermissions.Eventos.TiposEvento.Update)]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RequestUpdateTipoEvento request) => await _service.Update(id, request);

        [AuthenticatedDeleteResponses]
        [HasPermission(AppPermissions.Eventos.TiposEvento.Delete)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) => await _service.Delete(id);
    }
}
