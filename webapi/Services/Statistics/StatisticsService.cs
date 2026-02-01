using DaccApi.Infrastructure.Repositories.Statistics;
using DaccApi.Model.Responses.Statistics;

namespace DaccApi.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public StatisticsService(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        public async Task<ResponseDashboardStats> GetDashboardStatsAsync()
        {
            return await _statisticsRepository.GetDashboardStatsAsync();
        }
    }
}
