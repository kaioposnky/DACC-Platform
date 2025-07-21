using DaccApi.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Model;
using DaccApi.Services.User;
using DaccApi.Services.Auth;
using Microsoft.AspNetCore.Authorization;

namespace DaccApi.Controllers.Usuario
{
    [ApiController]
    [Route("api/users")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAuthService _authService;

        public UsuarioController(IUsuarioService usuarioService, IAuthService authService)
        {
            _usuarioService = usuarioService;
            _authService = authService;
        }
        
        [HasPermission(AppPermissions.Users.ViewAll)]
        [HttpGet("")]
        public IActionResult GetUsers()
        {
            // var response = _usuarioService.GetAllUsers();
            // return response;
            throw new NotImplementedException();
        }

        [HasPermission(AppPermissions.Users.View)]
        [HttpGet("{id:int}")]
        public IActionResult GetUser([FromRoute] int id)
        {
            var response = _usuarioService.GetUserById(id);
            return response;
        }
        
        [HasPermission(AppPermissions.Users.Update)]
        [HttpPatch("{id:int}")]
        public IActionResult UpdateUser([FromRoute] int id, [FromBody] RequestUsuario request)
        {
            throw new NotImplementedException();
        }

        [HasPermission(AppPermissions.Users.Delete)]
        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
    }
}   