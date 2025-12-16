 using DaccApi.Helpers;
using DaccApi.Helpers.Attributes;
using DaccApi.Model;
using DaccApi.Services.Avaliacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;

namespace DaccApi.Controllers.Avaliacao
{
    /// <summary>
    /// Controlador para gerenciar avaliações de produtos.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/api/ratings")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacaoService _avaliacaoService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="AvaliacaoController"/>.
        /// </summary>
        public AvaliacaoController(IAvaliacaoService avaliacaoService)
        {
            _avaliacaoService = avaliacaoService;
        }

        /// <summary>
        /// Obtém todas as avaliações.
        /// </summary>
        [AuthenticatedGetResponses]
        [HttpGet("")]
        [HasPermission(AppPermissions.Reviews.View)]
        public async Task<IActionResult> GetAllAvaliacoes()
        {
            var response = await _avaliacaoService.GetAllAvaliacoes();
            return response;
        }

        /// <summary>
        /// Obtém uma avaliação específica pelo seu ID.
        /// </summary>
        [AuthenticatedGetResponses]
        [HttpGet("{id:guid}")]
        [HasPermission(AppPermissions.Reviews.View)]
        public async Task<IActionResult> GetAvaliacaoById([FromRoute] Guid id)
        {
            var response = await _avaliacaoService.GetAvaliacaoById(id);
            return response;
        }

        /// <summary>
        /// Cria uma nova avaliação para um produto.
        /// </summary>
        [AuthenticatedPostResponses]
        [HttpPost("")]
        [HasPermission(AppPermissions.Reviews.Create)]
        public async Task<IActionResult> CreateAvaliacao([FromBody] RequestCreateAvaliacao avaliacao)
        {
            var userId = ClaimsHelper.GetUserId(User);
            var response = await _avaliacaoService.CreateAvaliacao(userId, avaliacao);
            return response;
        }

        /// <summary>
        /// Obtém todas as avaliações de um produto específico.
        /// </summary>
        [PublicGetResponses]
        [AllowAnonymous]
        [HttpGet("products/{produtoId:guid}")]
        public async Task<IActionResult> GetAvaliacoesByProductId([FromRoute] Guid produtoId)
        {
            var response = await _avaliacaoService.GetAvaliacoesByProductId(produtoId);
            return response;
        }

        /// <summary>
        /// Obtém todas as avaliações de um usuário específico.
        /// </summary>
        [AuthenticatedGetResponses]
        [HttpGet("users/{usuarioId:int}")]
        [HasPermission(AppPermissions.Reviews.View)]
        public async Task<IActionResult> GetAvaliacoesByUserId([FromRoute] Guid usuarioId)
        {
            var response = await _avaliacaoService.GetAvaliacoesByUserId(usuarioId);
            return response;
        }

        /// <summary>
        /// Deleta uma avaliação existente.
        /// </summary>
        [AuthenticatedDeleteResponses]
        [HttpDelete("{id:guid}")]
        [HasPermission(AppPermissions.Reviews.Delete)]
        public async Task<IActionResult> DeleteAvaliacao([FromRoute] Guid id)
        {
            var response = await _avaliacaoService.DeleteAvaliacao(id);
            return response;
        }
        
        /// <summary>
        /// Atualiza uma avaliação existente.
        /// </summary>
        [AuthenticatedPatchResponses]
        [HttpPatch("{id:guid}")]
        [HasPermission(AppPermissions.Reviews.Update)]
        public async Task<IActionResult> UpdateAvaliacao([FromRoute] Guid id, [FromBody] RequestUpdateAvaliacao avaliacao)
        {
            var response = await _avaliacaoService.UpdateAvaliacao(id, avaliacao);
            return response;
        }
    }
}
