using DaccApi.Model;
using DaccApi.Responses;
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
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllAvaliacoes([FromBody] RequestAvaliacao request)
        {
            var response = _avaliacaoService.GetAllAvaliacoes();
            return response;
        }
        
        [HttpGet("CreateAvaliacao")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult CreateAvaliacao(RequestAvaliacao request)
        {
            var response = _avaliacaoService.CreateAvaliacao(request);
            return response;
        }
        
        [HttpPost("GetProductAvaliacao")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult GetProductAvaliacao(RequestAvaliacao request)
        {
            var response = _avaliacaoService.GetAvaliacoesProduct(request);
            return response;
        }
        
        [HttpPost("GetAvaliacoesUser")]
        [ProducesResponseType(typeof(ResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequest), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorRequest), StatusCodes.Status500InternalServerError)]
        public IActionResult GetAvaliacoesUser(RequestAvaliacao request)
        {
            var response = _avaliacaoService.GetAvaliacoesUser(request);
            return response;
        }
    }
}
    
