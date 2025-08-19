namespace DaccApi.Middlewares
{
    /// <summary>
    /// Middleware para logging básico de requisições HTTP com cores
    /// </summary>
    public class LoggerMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggerMiddleware> _logger;
        
        private const string Green = "\u001b[32m";
        private const string Yellow = "\u001b[33m";
        private const string Red = "\u001b[31m";
        private const string Blue = "\u001b[34m";
        private const string Cyan = "\u001b[36m";
        private const string Gray = "\u001b[90m";
        private const string Reset = "\u001b[0m";
        
        public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var startTime = DateTime.UtcNow;
            
            try
            {
                await _next(httpContext);
                var duration = DateTime.UtcNow - startTime;

                var (color, emoji) = GetColorAndEmoji(httpContext.Response.StatusCode);
                var statusColor = GetStatusColor(httpContext.Response.StatusCode);
                var durationColor = GetDurationColor(duration.TotalMilliseconds);
                
                _logger.LogInformation(
                    "{Gray}[{Time}]{Reset} {Color}{Emoji} {Method}{Reset} {Path} -> {StatusColor}{StatusCode} {DurationColor}({Duration}ms)", 
                    Gray,
                    startTime.ToString("HH:mm:ss"),
                    Reset,
                    color,
                    emoji,
                    httpContext.Request.Method,
                    Reset,
                    httpContext.Request.Path,
                    statusColor,
                    httpContext.Response.StatusCode,
                    durationColor,
                    Math.Round(duration.TotalMilliseconds, 1)
                    );
            }
            catch (Exception e)
            {
                var duration = DateTime.UtcNow - startTime;
                
                _logger.LogError(
                    "{Gray}[{Time}]{Reset} {Red}💥 {Method} {Path} → ERROR {DurationColor}({Duration}ms){Reset}\n{Exception}", 
                    Gray,
                    startTime.ToString("HH:mm:ss"),
                    Reset,
                    Red,
                    httpContext.Request.Method, 
                    httpContext.Request.Path,
                    GetDurationColor(duration.TotalMilliseconds),
                    Math.Round(duration.TotalMilliseconds, 1),
                    Reset,
                    e);
                
                throw;
            }
        }
        
        private static (string Color, string Emoji) GetColorAndEmoji(int statusCode)
        {
            return statusCode switch
            {
                >= 200 and < 300 => (Green, "[OK]"),
                >= 300 and < 400 => (Cyan, "[>>]"),
                >= 400 and < 500 => (Yellow, "[!!]"),
                >= 500 => (Red, "[XX]"),
                _ => (Reset, "?")
            };
        }
        
        private static string GetStatusColor(int statusCode)
        {
            return statusCode switch
            {
                >= 200 and < 300 => Green,
                >= 300 and < 400 => Cyan,
                >= 400 and < 500 => Yellow,
                >= 500 => Red,
                _ => Reset
            };
        }
        
        private static string GetDurationColor(double milliseconds)
        {
            return milliseconds switch
            {
                < 100 => Green,
                < 500 => Yellow,
                _ => Red
            };
        }
    }
}