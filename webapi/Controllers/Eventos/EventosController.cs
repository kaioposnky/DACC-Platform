 using DaccApi.Helpers;
using DaccApi.Helpers.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Services.Eventos;
using DaccApi.Model;

namespace DaccApi.Controllers.Eventos
{
    /// <summary>
    /// Controlador para gerenciar eventos.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventosService _eventosService;
        
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="EventosController"/>.
        /// </summary>
        public EventosController(IEventosService eventosService)
        {
            _eventosService = eventosService;
        }
        
        /// <summary>
        /// Obtém todos os eventos.
        /// </summary>
        [PublicGetResponses]
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetAllEventos()
        {
            var response = await _eventosService.GetAllEventos();
            return response;
        }

        /// <summary>
        /// Cria um novo evento.
        /// </summary>
        [AuthenticatedPostResponses]
        [HttpPost("")]
        [HasPermission(AppPermissions.Eventos.Create)]
        public async Task<IActionResult> CreateEvento([FromBody] RequestEvento request)
        {
            var userId = ClaimsHelper.GetUserId(User);
            var response = await _eventosService.CreateEvento(userId, request);
            return response;
        }

        /// <summary>
        /// Obtém um evento específico pelo seu ID.
        /// </summary>
        [PublicGetResponses]
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEventoById([FromRoute] Guid id)
        {
            var response = await _eventosService.GetEventoById(id);
            return response;
        }

        /// <summary>
        /// Deleta um evento existente.
        /// </summary>
        [AuthenticatedDeleteResponses]
        [HttpDelete("{id:guid}")]
        [HasPermission(AppPermissions.Eventos.Delete)]
        public async Task<IActionResult> DeleteEvento([FromRoute] Guid id)
        {
            var response = await _eventosService.DeleteEvento(id);
            return response;
        }

        /// <summary>
        /// Atualiza um evento existente.
        /// </summary>
        [AuthenticatedPatchResponses]
        [HttpPatch("{id:guid}")]
        [HasPermission(AppPermissions.Eventos.Update)]
        public async Task<IActionResult> UpdateEvento([FromRoute] Guid id,[FromBody] RequestEvento request)
        {
            var response = await _eventosService.UpdateEvento(id,request);
            return response;
        }
        
    }
}