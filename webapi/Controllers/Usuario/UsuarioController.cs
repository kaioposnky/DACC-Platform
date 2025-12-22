 using DaccApi.Infrastructure.Authentication;
using DaccApi.Helpers.Attributes;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Model;
using DaccApi.Services.User;
using System;
using System.Threading.Tasks;
using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Responses;

namespace DaccApi.Controllers.Usuario
{
    /// <summary>
    /// Controlador para gerenciar usuários.
    /// </summary>
    [ApiController]
    [Route("v1/api/users")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioRepository _usuarioRepository;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="UsuarioController"/>.
        /// </summary>
        public UsuarioController(IUsuarioService usuarioService, IUsuarioRepository usuarioRepository)
        {
            _usuarioService = usuarioService;
            _usuarioRepository = usuarioRepository;
        }
        
        /// <summary>
        /// Obtém todos os usuários.
        /// </summary>
        [AuthenticatedGetResponses]
        [HasPermission(AppPermissions.Users.ViewAll)]
        [HttpGet("")]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _usuarioService.GetAllUsers();
            return response;
        }

        /// <summary>
        /// Obtém um usuário específico pelo seu ID.
        /// </summary>
        [AuthenticatedGetResponses]
        [HasPermission(AppPermissions.Users.View)]
        [HttpGet("{id:guid}")]
        public IActionResult GetUser([FromRoute] Guid id)
        {
            var response = _usuarioService.GetUserById(id);
            return response;
        }
        
        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        [AuthenticatedPatchResponses]
        [HasPermission(AppPermissions.Users.Update)]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromForm] RequestUpdateUsuario request)
        {
            var response = await _usuarioService.UpdateUser(id, request);
            return response;
        }
        
        /// <summary>
        /// Deleta um usuário existente.
        /// </summary>
        [AuthenticatedDeleteResponses]
        [HasPermission(AppPermissions.Users.Delete)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var response = await _usuarioService.DeleteUser(id);
            return response;
        }

        /// <summary>
        /// Obtém um usuário específico pelo seu ID.
        /// </summary>
        [AuthenticatedGetResponses]
        [HttpGet("{id:guid}/stats")]
        public async Task<IActionResult> GetUserStats([FromRoute] Guid id)
        {
            var requestUserId = ClaimsHelper.GetUserId(User);
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario != null && (requestUserId != usuario.Id))
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INVALID_CREDENTIALS);
            }

            var userStats = await _usuarioRepository.GetUserStats(id);
            if (userStats == null)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST,
                    "Não foi possível encontrar as estatísticas do usuário!");
            }
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(userStats));
        }
    }
}   