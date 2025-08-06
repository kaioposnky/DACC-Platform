using DaccApi.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Model;
using DaccApi.Services.User;

namespace DaccApi.Controllers.Usuario
{
    [ApiController]
    [Route("api/users")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        
        [HasPermission(AppPermissions.Users.ViewAll)]
        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _usuarioService.GetAllUsers();
            return response;
        }

        [HasPermission(AppPermissions.Users.View)]
        [HttpGet("{id:guid}")]
        public IActionResult GetUser([FromRoute] Guid id)
        {
            var response = _usuarioService.GetUserById(id);
            return response;
        }
        
        [HasPermission(AppPermissions.Users.Update)]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] RequestUpdateUsuario request)
        {
            var response = await _usuarioService.UpdateUser(id, request);
            return response;
        }

        [HasPermission(AppPermissions.Users.Delete)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var response = await _usuarioService.DeleteUser(id);
            return response;
        }
    }
}   