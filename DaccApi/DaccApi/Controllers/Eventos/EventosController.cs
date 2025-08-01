using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Services.Eventos;
using DaccApi.Responses;
using DaccApi.Responses.UserResponse;
using DaccApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;



namespace DaccApi.Controllers.Eventos
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventosService _eventosService;
        
        public EventosController(IEventosService eventosService)
        {
            _eventosService = eventosService;
        }
        
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetAllEventos()
        {
            var response = await _eventosService.GetAllEventos();
            return response;
        }

        [HttpPost("")]
        [HasPermission(AppPermissions.Eventos.Create)]
        public async Task<IActionResult> CreateEvento([FromBody] RequestEvento request)
        {
            var response = await _eventosService.CreateEvento(request);
            return response;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEventoById([FromRoute] int id)
        {
            var response = await _eventosService.GetEventoById(id);
            return response;
        }

        [HttpDelete("{id:int}")]
        [HasPermission(AppPermissions.Eventos.Delete)]
        public async Task<IActionResult> DeleteEvento([FromRoute] int id)
        {
            var response = await _eventosService.DeleteEvento(id);
            return response;
        }

        [HttpPatch("{id:int}")]
        [HasPermission(AppPermissions.Eventos.Update)]
        public async Task<IActionResult> UpdateEvento([FromRoute] int id,[FromBody] RequestEvento request)
        {
            var response = await _eventosService.UpdateEvento(id,request);
            return response;
        }
        
        [HttpPost("{id:int}/register")]
        [HasPermission(AppPermissions.Eventos.Register)]
        public async Task<IActionResult> RegisterEvento([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete("{id:int}/register")]
        [HasPermission(AppPermissions.Eventos.Register)]
        public async Task<IActionResult> UnregisterEvento([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
    }
}