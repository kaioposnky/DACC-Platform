 using DaccApi.Helpers;
using DaccApi.Helpers.Attributes;
using DaccApi.Model;
using DaccApi.Services.Noticias;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Model.Requests;
using System.Threading.Tasks;
using DaccApi.Model.Responses;
using DaccApi.Responses;

namespace DaccApi.Controllers.Noticias
{
    /// <summary>
    /// Controlador para gerenciar notícias.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v1/api/news")]
    public class NoticiasController : ControllerBase
    {
        private readonly INoticiasServices _noticiasServices;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="NoticiasController"/>.
        /// </summary>
        public NoticiasController(INoticiasServices noticiasServices)
        {
            _noticiasServices = noticiasServices;
        }

        /// <summary>
        /// Obtém todas as notícias.
        /// </summary>
        [PublicGetResponses]
        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetAllNoticias([FromQuery] RequestQueryNoticia requestQuery)
        {
            try
            {
                var noticias = await _noticiasServices.GetAllNoticias(requestQuery);

                if (noticias.Count == 0)
                {
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT.WithData(new {}));
                }

                var response = noticias.Select((noticia) =>  new ResponseNoticia(noticia));

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(response));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,
                    "Ocorreu um erro ao obter as notícias.");
            }
        }
        
        /// <summary>
        /// Cria uma nova notícia.
        /// </summary>
        [AuthenticatedPostResponses]
        [HttpPost("")]
        [HasPermission(AppPermissions.Noticias.Create)]
        public async Task<IActionResult> CreateNoticia([FromBody] RequestNoticia request)
        {
            var autorId = ClaimsHelper.GetUserId(User);
            var response = await _noticiasServices.CreateNoticia(autorId, request);
            return response;
        }

        /// <summary>
        /// Atualiza a imagem de uma notícia existente.
        /// </summary>
        [HttpPatch("{id:guid}/image")]
        [HasPermission(AppPermissions.Noticias.Update)]
        public async Task<IActionResult> UpdateNoticiaImage([FromRoute] Guid id, [FromForm] ImageRequest request)
        {
            var response = await _noticiasServices.UpdateNoticiaImage(id, request);
            return response;
        }
        
        /// <summary>
        /// Obtém uma notícia específica pelo seu ID.
        /// </summary>
        [PublicGetResponses]
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetNoticiaById([FromRoute] Guid id)
        {
            var response = await _noticiasServices.GetNoticiaById(id);
            return response;
        }
        
        /// <summary>
        /// Deleta uma notícia existente.
        /// </summary>
        [AuthenticatedDeleteResponses]
        [HttpDelete("{id:guid}")]
        [HasPermission(AppPermissions.Noticias.Delete)]
        public async Task<IActionResult> DeleteNoticia([FromRoute] Guid id)
        {
            var response = await _noticiasServices.DeleteNoticia(id);
            return response;
        }
        
        /// <summary>
        /// Atualiza uma notícia existente.
        /// </summary>
        [AuthenticatedPatchResponses]
        [HttpPatch("{id:guid}")]
        [HasPermission(AppPermissions.Noticias.Update)]
        public async Task<IActionResult> UpdateNoticia([FromRoute] Guid id, [FromForm] RequestNoticia request)
        {
            var response = await _noticiasServices.UpdateNoticia(id, request);
            return response;
        }
    }
}
