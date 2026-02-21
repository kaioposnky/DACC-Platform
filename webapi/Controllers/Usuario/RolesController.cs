using DaccApi.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Usuario
{
    [ApiController]
    [Route("v1/api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly ITipoUsuarioService _tipoUsuarioService;

        public RolesController(ITipoUsuarioService tipoUsuarioService)
        {
            _tipoUsuarioService = tipoUsuarioService;
        }

        /// <summary>
        /// Obtém todos os cargos disponiveis no sistema.
        /// </summary>
        [HttpGet("")]
        public async Task<IActionResult> GetAllRoles()
        {
            return await _tipoUsuarioService.GetAllRoles();
        }
    }
}
