using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Auth
{
    public interface IAuthService
    {
        Task<IActionResult> LoginUser(RequestLogin request);
        Task<IActionResult> RegisterUser(RequestRegistro requestCreate);
        Task<IActionResult> RefreshUserToken(string refreshToken);
        Task<IActionResult> Logout(Guid userId);
        
        // Métodos de Recuperação e Troca de Senha
        Task<IActionResult> ForgotPassword(RequestForgotPassword request);
        Task<IActionResult> ValidateResetToken(string token);
        Task<IActionResult> ResetPassword(RequestResetPassword request);
        Task<IActionResult> ChangePassword(Guid userId, RequestChangePassword request);
    }
}
