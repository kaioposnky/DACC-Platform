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
        public IActionResult GetAllAvaliacoes()
        {
            var response = _avaliacaoService.GetAllAvaliacoes();
            return response;
        }
        
        [HttpPost("")]
        [HasPermission(AppPermissions.Reviews.Create)]
        public IActionResult CreateAvaliacao([FromBody] RequestAvaliacao request)
        {
            var response = _avaliacaoService.CreateAvaliacao(request);
            return response;
        }
        
        [AllowAnonymous]
        [HttpGet("products/{id:int}")]
        public IActionResult GetProductAvaliacoes([FromRoute] int id)
        {
            var response = _avaliacaoService.GetAvaliacoesProductById(id);
            return response;
        }
        
        [HttpGet("users/{id:int}")]
        [HasPermission(AppPermissions.Reviews.View)]
        public IActionResult GetAvaliacoesUser([FromRoute] int id)
        {
            var response = _avaliacaoService.GetAvaliacoesUserById(id);
            return response;
        }

        [HttpDelete("{id:int}")]
        [HasPermission(AppPermissions.Reviews.Delete)]
        public IActionResult DeleteAvaliacao([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
    }
}
    
