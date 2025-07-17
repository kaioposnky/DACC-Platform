using DaccApi.Model;
using DaccApi.Responses;
using DaccApi.Responses.UserResponse;
using DaccApi.Services.Avaliacao;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Avaliacao
{
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
        public IActionResult GetAllAvaliacoes()
        {
            var response = _avaliacaoService.GetAllAvaliacoes();
            return response;
        }
        
        [HttpPost("")]
        public IActionResult CreateAvaliacao([FromBody] RequestAvaliacao request)
        {
            var response = _avaliacaoService.CreateAvaliacao(request);
            return response;
        }
        
        [HttpGet("products/{id:int}")]
        public IActionResult GetProductAvaliacoes([FromRoute] int id)
        {
            var response = _avaliacaoService.GetAvaliacoesProductById(id);
            return response;
        }
        
        [HttpGet("users/{id:int}")]
        public IActionResult GetAvaliacoesUser([FromRoute] int id)
        {
            var response = _avaliacaoService.GetAvaliacoesUserById(id);
            return response;
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteAvaliacao([FromRoute] int id)
        {
            throw new NotImplementedException();
        }
    }
}
    
