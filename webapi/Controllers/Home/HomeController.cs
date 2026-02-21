using DaccApi.Helpers;
using DaccApi.Helpers.Attributes;
using DaccApi.Responses;
using DaccApi.Services.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Home
{
    [Authorize]
    [ApiController]
    [Route("v1/api/home")]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        /// <summary>
        /// Obtém o feed unificado contendo Notícias, Eventos e Projetos.
        /// </summary>
        [AllowAnonymous]
        [PublicGetResponses]
        [HttpGet("feed")]
        public async Task<IActionResult> GetUnifiedFeed([FromQuery] int limit = 5)
        {
            try
            {
                var feed = await _homeService.GetUnifiedFeed(limit);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(feed));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
