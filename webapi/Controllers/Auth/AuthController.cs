using DaccApi.Helpers;
using DaccApi.Helpers.Attributes;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model;
using DaccApi.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Auth
{
    /// <summary>
    /// Controlador para gerenciar autenticação, registro e sessões de usuário.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AuthController"/>.
        /// </summary>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        /// <summary>
        /// Autentica um usuário e retorna um token de acesso.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] RequestLogin request)
        {
            var response = await _authService.LoginUser(request);
            return response;
        }
        
        /// <summary>
        /// Registra um novo usuário no sistema.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RequestRegistro requestCreate)
        {
            var response = await _authService.RegisterUser(requestCreate);
            return response;
        }
        
        /// <summary>
        /// Atualiza o token de acesso de um usuário usando um refresh token.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var response = await _authService.RefreshUserToken(refreshToken);
            return response;
        }
        
        /// <summary>
        /// Realiza o logout do usuário autenticado.
        /// </summary>
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