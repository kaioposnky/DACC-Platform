using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;

namespace DaccApi.Controllers.Eventos
{
    [Authorize]
    [ApiController]
    [Route("api/events")]
    public class EventosController : ControllerBase
    {
        
        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult GetEventos()
        {
            throw new NotImplementedException();
        }

        [HttpPost("")]
        [HasPermission(AppPermissions.Eventos.Create)]
        public IActionResult CreateEvento()
        {
            throw new NotImplementedException();
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetEventoById([FromRoute] int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        [HasPermission(AppPermissions.Eventos.Delete)]
        public IActionResult DeleteEvento([FromRoute] int id)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{id:int}")]
        [HasPermission(AppPermissions.Eventos.Update)]
        public IActionResult UpdateEvento([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("{id:int}/register")]
        [HasPermission(AppPermissions.Eventos.Register)]
        public IActionResult RegisterEvento([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete("{id:int}/register")]
        [HasPermission(AppPermissions.Eventos.Register)]
        public IActionResult UnregisterEvento([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
    }
}