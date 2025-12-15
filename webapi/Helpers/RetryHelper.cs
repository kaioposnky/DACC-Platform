namespace DaccApi.Helpers
{
    public class RetryHelper
    {
        public static async Task<T> ExecuteAndRetryAsync<T>(Func<Task<T>> action, int retryCount = 3, int retryDelaySeconds = 2)
        {
            var tryAmount = 0;
            while (tryAmount++ != retryCount)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex)
                {
                    // Gera um delay para esperar X segundos antes de tentar executar novamente
                    var delay = TimeSpan.FromSeconds(retryDelaySeconds);
                    await Task.Delay(delay);
                }
            }

            return await action();
        }
        
        public static T ExecuteAndRetry<T>(Func<T> action, int retryCount = 3, int retryDelaySeconds = 2)
        {
            var tryAmount = 0;
            while (tryAmount++ != retryCount)
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    // Gera um delay para esperar X segundos antes de tentar executar novamente
                    var delay = TimeSpan.FromSeconds(retryDelaySeconds); 
                    Task.Delay(delay).Wait();
                }
            }

            return action();
        }
    }
}