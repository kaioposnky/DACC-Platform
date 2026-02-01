using DaccApi.Helpers;
using DaccApi.Helpers.Attributes;
using DaccApi.Infrastructure.Authentication;
using DaccApi.Responses;
using DaccApi.Services.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Controllers.Statistics
{
    [Authorize]
    [ApiController]
    [Route("v1/api/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        /// <summary>
        /// Retorna estatísticas gerais do sistema para o dashboard administrativo.
        /// </summary>
        [AuthenticatedGetResponses]
        [HttpGet("dashboard")]
        [HasPermission(AppPermissions.Dashboard.View)]
        public async Task<IActionResult> GetDashboardStats()
        {
            try
            {
                var stats = await _statisticsService.GetDashboardStatsAsync();
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(stats));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
