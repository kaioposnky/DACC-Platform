using DaccApi.Helpers.Attributes;
using DaccApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Services.Diretores;

namespace DaccApi.Controllers.Diretores
{
    /// <summary>
    /// Controlador para gerenciar os diretores do DACC.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/api/[controller]")]

    public class DiretoresController : ControllerBase
    {
        private readonly IDiretoresService _diretoresService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="DiretoresController"/>.
        /// </summary>
        public DiretoresController(IDiretoresService diretoresService)
        {
            _diretoresService = diretoresService;
        }

        /// <summary>
        /// Obtém todos os diretores.
        /// </summary>
        [AllowAnonymous]
        [PublicGetResponses]
        [HttpGet("")]
        [HasPermission(AppPermissions.Faculty.View)]
        public async Task<IActionResult> GetAllDiretores()
        {
            var response = await _diretoresService.GetAllDiretores();
            return response;
        }
        
        /// <summary>
        /// Obtém um diretor específico pelo seu ID.
        /// </summary>
        [AllowAnonymous]
        [PublicGetResponses]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDiretorById([FromRoute] Guid id)
        {
            var response = await _diretoresService.GetDiretorById(id);
            return response;
        }

        /// <summary>
        /// Cria um novo diretor.
        /// </summary>
        [AuthenticatedPostResponses]
        [HttpPost("")]
        public async Task<IActionResult> CreateDiretor([FromForm] RequestDiretor request)
        {
            var response = await _diretoresService.CreateDiretor(request);
            return response;
        }
        
        /// <summary>
        /// Deleta um diretor existente.
        /// </summary>
        [AuthenticatedDeleteResponses]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDiretor([FromRoute] Guid id)
        {
            var response = await _diretoresService.DeleteDiretor(id);
            return response;
        }
        
        /// <summary>
        /// Atualiza um diretor existente.
        /// </summary>
        [AuthenticatedPatchResponses]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateDiretor([FromRoute] Guid id, [FromForm] RequestDiretor request)
        {
            var response = await _diretoresService.UpdateDiretor(id, request);
            return response;
        }
    }
}
