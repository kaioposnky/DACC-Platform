using Microsoft.AspNetCore.Mvc;
using DaccApi.Model;
using DaccApi.Services.User;
using DaccApi.Services.Auth;
using DaccApi.Responses.UserResponse;
using DaccApi.Responses;
using Microsoft.AspNetCore.Authorization;

namespace DaccApi.Controllers.Usuario
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAuthService _authService;

        public UsuarioController(IUsuarioService usuarioService, IAuthService authService)
        {
            _usuarioService = usuarioService;
            _authService = authService;
        }

        [HttpPost("criar")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult CreateUser([FromBody] RequestUsuario request)
        {
            var response = _usuarioService.CreateUser(request);
            return response;
            
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult Login([FromBody] RequestUsuario request)
        {
            if (_authService.ValidateCredentials(request))
            {
                var token = _authService.GenerateToken(request);
                return Ok(new { Token = token });
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }

        [HttpGet("user")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult GetUser([FromQuery] Guid? id, [FromQuery] string? email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var response = _usuarioService.GetUserByEmail(email);
                return response;
            }
            else
            {
                var response = _usuarioService.GetUserById(id);
                return response;
            }
        }
    }
}