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
        public IActionResult GetAllAvaliacoes([FromBody] RequestAvaliacao request)
        {
            var response = _avaliacaoService.GetAllAvaliacoes();
            return response;
        }
        
        [HttpGet("AddProductRating")]
        [ProducesResponseType(typeof(UserResponseRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddProductRating(RequestAvaliacao request)
        {
            var response = _avaliacaoService.AddAvaliacaoProduct(request);
            return response;
        }
    }
}
    
