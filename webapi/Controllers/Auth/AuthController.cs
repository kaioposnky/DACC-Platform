using DaccApi.Helpers;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model;
using DaccApi.Services.Auth;
using DaccApi.Helpers.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Auth
{
    [Authorize]
    [ApiController]
    [Route("v1/api/[controller]")]
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
        
        [AuthenticatedDeleteResponses]
        [HasPermission(AppPermissions.Users.Logout)]
        [HttpPost("logout")]
        public async Task <IActionResult> LogoutUser()
        {
            var userId = ClaimsHelper.GetUserId(User);
            var response = await _authService.Logout(userId);
            return response;
        }
        
    }
}