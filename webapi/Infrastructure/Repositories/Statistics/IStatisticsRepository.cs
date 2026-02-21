using DaccApi.Model.Responses.Statistics;

namespace DaccApi.Infrastructure.Repositories.Statistics
{
    public interface IStatisticsRepository
    {
        Task<ResponseDashboardStats> GetDashboardStatsAsync();
    }
}
