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

        [HttpPost("add")]
        [ProducesResponseType(typeof(UserResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Add([FromBody] RequestUsuario request)
        {
            var usuario = new Usuario
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                RegistrationDate = request.RegistrationDate,
            };

            if (usuario == null)
                return ResponseHelper.CreateBadRequestResponse("Usuário não pode ser nulo.");

            try
            {
                _userService.Add(usuario);
                return ResponseHelper.CreateSuccessResponse(usuario, "Usuário adicionado com sucesso.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse($"Ocorreu um erro crítico na plataforma, favor acionar o suporte em: contato.dacc@gmail.com");
            }
        }
    }
}
