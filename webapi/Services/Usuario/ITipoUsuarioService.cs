using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.User
{
    public interface ITipoUsuarioService
    {
        Task<IActionResult> GetAllRoles();
    }
}
