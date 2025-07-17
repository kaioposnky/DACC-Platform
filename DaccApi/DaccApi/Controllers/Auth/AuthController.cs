using DaccApi.Model;
using DaccApi.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] RequestUsuario request)
        {
            var response = _authService.LoginUser(request);
            return response;
        }
        
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] RequestUsuario request)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("refresh")]
        public IActionResult RefreshToken()
        {
            throw new NotImplementedException();
        }
        
        [HttpPost("logout")]
        public IActionResult LogoutUser()
        {
            throw new NotImplementedException();
        }
        
    }
}