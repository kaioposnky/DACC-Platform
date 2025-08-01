using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.User
{
    public interface IUsuarioService
    {
        public IActionResult GetUserById(Guid id);
        public IActionResult GetUserByEmail(string email);
        Task<IActionResult> GetAllUsers();
        Task<IActionResult> UpdateUser(Guid id, RequestUpdateUsuario newUserData);
        Task<IActionResult> DeleteUser(Guid id);
    }
}
