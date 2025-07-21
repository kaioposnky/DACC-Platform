using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Auth
{
    public interface IAuthService
    {
        Task<IActionResult> LoginUser(RequestLogin request);
        Task<IActionResult> RegisterUser(RequestUsuario request);
    }
}
