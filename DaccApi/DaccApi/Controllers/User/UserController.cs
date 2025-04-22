using Microsoft.AspNetCore.Mvc;
using DaccApi.Model;
using DaccApi.Services.User;
using DaccApi.Services.Auth;
using DaccApi.Responses.UserResponse;
using DaccApi.Responses;
using Microsoft.AspNetCore.Authorization;

namespace DaccApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("CreateUser")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult CreateUser([FromBody] RequestUsuario request)
        {
            var response = _userService.CreateUser(request);
            return response;
            
        }

        [HttpPost("Login")]
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

        [HttpPost("GetUserById")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserById([FromBody] RequestUsuario request)
        {
            var response = _userService.GetUserById(request);
            return response;
        }

        [HttpPost("GetUserByEmail")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserByEmail([FromBody] RequestUsuario request)
        {
            var response = _userService.GetUserByEmail(request);
            return response;
        }
    }
}