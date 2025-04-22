using DaccApi.Model;
using DaccApi.Responses.UserResponse;
using DaccApi.Services.Avaliacao;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Avaliacao
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacaoService _avaliacaoService;
        public AvaliacaoController(IAvaliacaoService avaliacaoService)
        {
            _avaliacaoService = avaliacaoService;
        }
        [HttpPost("GetAllAvaliacoes")]
        [ProducesResponseType(typeof(UserResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllAvaliacoes([FromBody] RequestAvaliacao request)
        {
            var response = _avaliacaoService.GetAllAvaliacoes();
            return response;
        }
        
        [HttpGet("CreateProductRating")]
        [ProducesResponseType(typeof(UserResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateProductRating(RequestAvaliacao request)
        {
            var response = _avaliacaoService.CreateAvaliacaoProduct(request);
            return response;
        }
        
        [HttpPost("GetProductAvaliacao")]
        [ProducesResponseType(typeof(UserResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetProductAvaliacao(RequestAvaliacao request)
        {
            var response = _avaliacaoService.GetAvaliacoesProduct(request);
            return response;
        }
        
        [HttpPost("GetAvaliacoesUser")]
        [ProducesResponseType(typeof(UserResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAvaliacoesUser(RequestAvaliacao request)
        {
            var response = _avaliacaoService.GetAvaliacoesUser(request);
            return response;
        }
    }
}
    
