using DaccApi.Helpers.Attributes;
using DaccApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Services.Professores;
using DaccApi.Model.Requests;

namespace DaccApi.Controllers.Professores
{
    /// <summary>
    /// Controlador para gerenciar os professores/corpo docente (faculty) do DACC.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/api/faculty")]
    public class ProfessoresController : ControllerBase
    {
        private readonly IProfessoresService _professoresService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProfessoresController"/>.
        /// </summary>
        public ProfessoresController(IProfessoresService professoresService)
        {
            _professoresService = professoresService;
        }

        /// <summary>
        /// Obtém todos os professores.
        /// </summary>
        [AllowAnonymous]
        [PublicGetResponses]
        [HttpGet("")]
        public async Task<IActionResult> GetAllProfessores()
        {
            var response = await _professoresService.GetAllProfessores();
            return response;
        }
        
        /// <summary>
        /// Busca professores com filtros e paginação.
        /// </summary>
        [AllowAnonymous]
        [PublicGetResponses]
        [HttpGet("search")]
        public async Task<IActionResult> SearchProfessores([FromQuery] RequestQueryProfessor query)
        {
            var response = await _professoresService.SearchProfessores(query);
            return response;
        }

        /// <summary>
        /// Obtém um professor específico pelo seu ID.
        /// </summary>
        [AllowAnonymous]
        [PublicGetResponses]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProfessorById([FromRoute] Guid id)
        {
            var response = await _professoresService.GetProfessorById(id);
            return response;
        }

        /// <summary>
        /// Cria um novo professor.
        /// </summary>
        [AuthenticatedPostResponses]
        [HasPermission(AppPermissions.Faculty.Create)]
        [HttpPost("")]
        public async Task<IActionResult> CreateProfessor([FromBody] RequestProfessor request)
        {
            var response = await _professoresService.CreateProfessor(request);
            return response;
        }
        
        /// <summary>
        /// Deleta um professor existente.
        /// </summary>
        [AuthenticatedDeleteResponses]
        [HasPermission(AppPermissions.Faculty.Delete)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProfessor([FromRoute] Guid id)
        {
            var response = await _professoresService.DeleteProfessor(id);
            return response;
        }
        
        /// <summary>
        /// Atualiza um professor existente.
        /// </summary>
        [AuthenticatedPatchResponses]
        [HttpPatch("{id:guid}")]
        [HasPermission(AppPermissions.Faculty.Update)]
        public async Task<IActionResult> UpdateProfessor(Guid id, [FromBody] RequestProfessor request)
        {
            var response = await _professoresService.UpdateProfessor(id, request);
            return response;
        }
    }
}
