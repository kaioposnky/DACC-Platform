using DaccApi.Helpers;
using DaccApi.Helpers.Attributes;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Infrastructure.Mail;
using DaccApi.Model;
using DaccApi.Responses;
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
    [Route("v1/api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AuthController"/>.
        /// </summary>
        public AuthController(IAuthService authService, IMailService mailService)
        {
            _authService = authService;
            _mailService = mailService;
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
            try
            {
                var usuario = await _authService.RegisterUser(requestCreate);

                // Envia e-mail de boas-vindas de forma assíncrona (Fire and Forget)
                // Usamos Task.Run para não bloquear a resposta da API enquanto o SMTP processa
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _mailService.SendWelcomeEmailAsync(usuario);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao enviar e-mail de boas-vindas: {ex.Message}");
                    }
                });

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new { users = usuario.ToResponse() }));
            }
            catch (ArgumentException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,
                    "Ocorreu um erro ao tentar cadastrar o usuário. " + ex.Message);
            }
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

        /// <summary>
        /// Solicita a recuperação de senha enviando um e-mail com um token.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] RequestForgotPassword request)
        {
            var response = await _authService.ForgotPassword(request);
            return response;
        }

        /// <summary>
        /// Valida se um token de reset de senha ainda é válido.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("validate-reset-token")]
        public async Task<IActionResult> ValidateResetToken([FromQuery] string token)
        {
            var response = await _authService.ValidateResetToken(token);
            return response;
        }

        /// <summary>
        /// Redefine a senha do usuário usando um token válido.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] RequestResetPassword request)
        {
            var response = await _authService.ResetPassword(request);
            return response;
        }

        /// <summary>
        /// Altera a senha do usuário autenticado.
        /// </summary>
        [AuthenticatedPatchResponses]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] RequestChangePassword request)
        {
            var userId = ClaimsHelper.GetUserId(User);
            var response = await _authService.ChangePassword(userId, request);
            return response;
        }
    }
}
