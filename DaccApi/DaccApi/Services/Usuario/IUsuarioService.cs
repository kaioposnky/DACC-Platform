using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.User
{
    public interface IUsuarioService
    {
        public IActionResult CreateUser(RequestUsuario request);
        public IActionResult GetUserById(RequestUsuario request);
        public IActionResult GetUserByEmail(RequestUsuario request);
    }
}
