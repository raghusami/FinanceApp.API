using System.Diagnostics;

namespace FinanceApp.API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Optional: Skip logging for Swagger or static files
            if (context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/favicon.ico"))
            {
                await _next(context);
                return;
            }
            // Mask sensitive headers
            var headers = context.Request.Headers
                .Where(h => h.Key != "Authorization")
                .ToDictionary(h => h.Key, h => h.Value.ToString());

            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation(
                "[REQUEST] {method} {path} | Status: {statusCode} | Time: {time} ms | Headers: {@headers}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                headers
            );
        }
    }
}
