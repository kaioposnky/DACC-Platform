using Microsoft.AspNetCore.Mvc;
using DaccApi.Model;
using DaccApi.Services;
using DaccApi.Helpers;
using DaccApi.Responses.UserResponse;
using DaccApi.Services.User;
using DaccApi.Responses;

namespace DaccApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("CreateUser")]
        [ProducesResponseType(typeof(UserResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateUser([FromBody] RequestUsuario request)
        {
            var response = _userService.CreateUser(request);
            return response;
        }
    }
}
