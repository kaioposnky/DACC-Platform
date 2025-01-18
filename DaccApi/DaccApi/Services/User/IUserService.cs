using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.User
{
    public interface IUserService
    {
        public IActionResult CreateUser(RequestUsuario request);
    }
}
