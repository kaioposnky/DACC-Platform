using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.User
{
    public interface IUsuarioService
    {
        public IActionResult GetUserById(int id);
        public IActionResult GetUserByEmail(string email);
        Task<IActionResult> GetAllUsers();
        Task<IActionResult> UpdateUser(int id, RequestUpdateUsuario newUserData);
        Task<IActionResult> DeleteUser(int id);
    }
}
