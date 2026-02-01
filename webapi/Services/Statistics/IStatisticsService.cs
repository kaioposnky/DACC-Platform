using DaccApi.Model.Responses.Statistics;

namespace DaccApi.Services.Statistics
{
    public interface IStatisticsService
    {
        Task<ResponseDashboardStats> GetDashboardStatsAsync();
    }
}
