using DaccApi.Infrastructure.Repositories.Reservas;

namespace DaccApi.Infrastructure.BackgroundServices
{
    public class ReservationCleanupService : BackgroundService
    {
        private readonly ILogger<ReservationCleanupService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval;
        
        public ReservationCleanupService(
            ILogger<ReservationCleanupService> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _interval = TimeSpan.FromMinutes(int.Parse(configuration["ReservationCleanup:IntervalInMinutes"]));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    // Recebe o Repositório das reservas de produtos e executa a limpeza
                    await scope.ServiceProvider.GetRequiredService<IReservaRepository>().LimparReservasExpiradas();
                    _logger.LogInformation("Reservas expiradas limpas com sucesso!");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao limpar reservas expiradas!");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}