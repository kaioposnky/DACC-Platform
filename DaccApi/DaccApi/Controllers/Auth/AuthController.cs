using DaccApi.Infrastructure.Authentication;
using DaccApi.Model;
using DaccApi.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Auth
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] RequestLogin request)
        {
            var response = await _authService.LoginUser(request);
            return response;
        }
        
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RequestCreateUsuario requestCreate)
        {
            var response = await _authService.RegisterUser(requestCreate);
            return response;
        }
        
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromForm] string refreshToken)
        {
            var response = await _authService.RefreshUserToken(refreshToken);
            return response;
        }
        
        [HasPermission(AppPermissions.Users.Logout)]
        [HttpPost("logout")]
        public IActionResult LogoutUser()
        {
            throw new NotImplementedException();
        }
        
    }
}