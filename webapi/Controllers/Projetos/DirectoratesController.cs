using DaccApi.Helpers.Attributes;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model.Requests.Projetos;
using DaccApi.Services.Projetos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Projetos
{
    [Authorize]
    [ApiController]
    [Route("v1/api/directorates")]
    public class DirectoratesController : ControllerBase
    {
        private readonly IDiretoriaService _diretoriaService;

        public DirectoratesController(IDiretoriaService diretoriaService)
        {
            _diretoriaService = diretoriaService;
        }

        [AllowAnonymous]
        [PublicGetResponses]
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            return await _diretoriaService.GetAllDiretorias();
        }

        [AllowAnonymous]
        [PublicGetResponses]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] RequestQueryDiretoria query)
        {
            return await _diretoriaService.SearchDiretorias(query);
        }

        [AllowAnonymous]
        [PublicGetResponses]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return await _diretoriaService.GetDiretoriaById(id);
        }

        [HasPermission(AppPermissions.Diretorias.Create)]
        [AuthenticatedPostResponses]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] RequestCreateDiretoria request)
        {
            return await _diretoriaService.CreateDiretoria(request);
        }

        [HasPermission(AppPermissions.Diretorias.Update)]
        [AuthenticatedPatchResponses]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RequestUpdateDiretoria request)
        {
            return await _diretoriaService.UpdateDiretoria(id, request);
        }

        [HasPermission(AppPermissions.Diretorias.Delete)]
        [AuthenticatedDeleteResponses]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _diretoriaService.DeleteDiretoria(id);
        }
    }
}
