using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.User
{
    public interface IUsuarioService
    {
        public IActionResult CreateUser(RequestUsuario request);
        public IActionResult GetUserById(int id);
        public IActionResult GetUserByEmail(string email);
    }
}
