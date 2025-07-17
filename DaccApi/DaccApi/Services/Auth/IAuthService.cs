using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Auth
{
    public interface IAuthService
    {
        IActionResult LoginUser(RequestUsuario request);
    }
}
