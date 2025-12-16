 using DaccApi.Helpers.Attributes;
using DaccApi.Model;
using DaccApi.Services.Projetos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model.Requests;
using System;
using System.Threading.Tasks;

namespace DaccApi.Controllers.Projetos
{
    /// <summary>
    /// Controlador para gerenciar projetos.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/api/projects")]
    public class ProjetosController : ControllerBase
    {
        private readonly IProjetosService _projetosService;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProjetosController"/>.
        /// </summary>
        public ProjetosController(IProjetosService projetosService)
        {
            _projetosService = projetosService;
        }

        /// <summary>
        /// Obtém todos os projetos.
        /// </summary>
        [PublicGetResponses]
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetAllProjetos()
        {

            var response = await _projetosService.GetAllProjetos();
            return response;
        }

        /// <summary>
        /// Obtém um projeto específico pelo seu ID.
        /// </summary>
        [PublicGetResponses]
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProjetoById(Guid id)
        {
            var response = await _projetosService.GetProjetoById(id);
            return response;
        }

        /// <summary>
        /// Cria um novo projeto.
        /// </summary>
        [AuthenticatedPostResponses]
        [HttpPost("")]
        [HasPermission(AppPermissions.Projetos.Create)]
        public async Task<IActionResult> CreateProjeto([FromBody] RequestProjeto projeto)
        {
            var response = await _projetosService.CreateProjeto(projeto);
            return response;
        }
        
        /// <summary>
        /// Adiciona uma imagem a um projeto existente.
        /// </summary>
        [HttpPost("{id:guid}")]
        [HasPermission(AppPermissions.Projetos.Update)]
        public async Task<IActionResult> AddProjetoImage([FromRoute] Guid id, [FromForm] ImageRequest request)
        {
            var response = await _projetosService.AddProjetoImage(id, request);
            return response;
        }

        /// <summary>
        /// Atualiza um projeto existente.
        /// </summary>
        [AuthenticatedPatchResponses]
        [HttpPatch("{id:guid}")]
        [HasPermission(AppPermissions.Projetos.Update)]
        public async Task<IActionResult> UpdateProjeto([FromRoute] Guid id, [FromForm] RequestProjeto projeto)
        {
            var response = await _projetosService.UpdateProjeto(id, projeto);
            return response;
        }

        /// <summary>
        /// Deleta um projeto existente.
        /// </summary>
        [AuthenticatedDeleteResponses]
        [HttpDelete("{id:guid}")]
        [HasPermission(AppPermissions.Projetos.Delete)]
        public async Task<IActionResult> DeleteProjeto([FromRoute] Guid id)
        {
            var response = await _projetosService.DeleteProjeto(id);
            return response;
        }
        
    }
}
