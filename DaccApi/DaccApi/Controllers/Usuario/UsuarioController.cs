using Microsoft.AspNetCore.Mvc;
using DaccApi.Model;
using DaccApi.Services.User;
using DaccApi.Services.Auth;
using DaccApi.Responses.UserResponse;
using DaccApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using BadRequest = DaccApi.Responses.BadRequest;

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
        
        [HttpGet("")]
        public IActionResult GetUsers()
        {
            // var response = _usuarioService.GetAllUsers();
            // return response;
            throw new NotImplementedException();
        }
        
        [HttpPost("")]
        public IActionResult CreateUser([FromBody] RequestUsuario request)
        {
            var response = _usuarioService.CreateUser(request);
            return response;
            
        }

        [HttpGet("{id:int}")]
        public IActionResult GetUser([FromRoute] int id)
        {
            var response = _usuarioService.GetUserById(id);
            return response;
        }
        
        [HttpPatch("{id:int}")]
        public IActionResult UpdateUser([FromRoute] int id, [FromBody] RequestUsuario request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
    }
}