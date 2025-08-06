using DaccApi.Helpers;
using DaccApi.Model;
using DaccApi.Responses;
using DaccApi.Responses.UserResponse;
using DaccApi.Services.Avaliacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;

namespace DaccApi.Controllers.Avaliacao
{
    [Authorize]
    [ApiController]
    [Route("api/ratings")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacaoService _avaliacaoService;

        public AvaliacaoController(IAvaliacaoService avaliacaoService)
        {
            _avaliacaoService = avaliacaoService;
        }

        [HttpGet("")]
        [HasPermission(AppPermissions.Reviews.View)]
        public async Task<IActionResult> GetAllAvaliacoes()
        {
            var response = await _avaliacaoService.GetAllAvaliacoes();
            return response;
        }

        [HttpGet("{id:guid}")]
        [HasPermission(AppPermissions.Reviews.View)]
        public async Task<IActionResult> GetAvaliacaoById([FromRoute] Guid id)
        {
            var response = await _avaliacaoService.GetAvaliacaoById(id);
            return response;
        }

        [HttpPost("")]
        [HasPermission(AppPermissions.Reviews.Create)]
        public async Task<IActionResult> CreateAvaliacao([FromBody] RequestCreateAvaliacao avaliacao)
        {
            var userId = ClaimsHelper.GetUserId(User);
            var response = await _avaliacaoService.CreateAvaliacao(userId, avaliacao);
            return response;
        }

        [AllowAnonymous]
        [HttpGet("products/{produtoId:guid}")]
        public async Task<IActionResult> GetAvaliacoesByProductId([FromRoute] Guid produtoId)
        {
            var response = await _avaliacaoService.GetAvaliacoesByProductId(produtoId);
            return response;
        }

        [HttpGet("users/{usuarioId:int}")]
        [HasPermission(AppPermissions.Reviews.View)]
        public async Task<IActionResult> GetAvaliacoesByUserId([FromRoute] Guid usuarioId)
        {
            var response = await _avaliacaoService.GetAvaliacoesByUserId(usuarioId);
            return response;
        }

        [HttpDelete("{id:guid}")]
        [HasPermission(AppPermissions.Reviews.Delete)]
        public async Task<IActionResult> DeleteAvaliacao([FromRoute] Guid id)
        {
            var response = await _avaliacaoService.DeleteAvaliacao(id);
            return response;
        }
        
        
        
        [HttpPatch("{id:guid}")]
        [HasPermission(AppPermissions.Reviews.Update)]
        public async Task<IActionResult> UpdateAvaliacao([FromRoute] Guid id,[FromBody] RequestUpdateAvaliacao avaliacao)
        {
            var response = await _avaliacaoService.UpdateAvaliacao(id, avaliacao);
            return response;
        }



    }
}
    
