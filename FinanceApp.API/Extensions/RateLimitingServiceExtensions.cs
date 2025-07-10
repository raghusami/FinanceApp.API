using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace FinanceApp.API.Extensions
{
    
        public static class RateLimitingServiceExtensions
        {
            public static IServiceCollection AddCustomRateLimiting(this IServiceCollection services, IConfiguration configuration)
            {
                services.AddRateLimiter(options =>
                {
                    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                    // ✅ Fixed Window Rate Limiter
                    options.AddFixedWindowLimiter("FixedWindowRateLimiter", policy =>
                    {
                        policy.PermitLimit = Convert.ToInt32(configuration["RateLimiter:FixedWindowRateLimiter"]);
                        policy.Window = TimeSpan.FromSeconds(Convert.ToInt32(configuration["RateLimiter:FixedWindowSize"]));
                        policy.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                        policy.QueueLimit = Convert.ToInt32(configuration["RateLimiter:FixedWindowQueueLimit"]);
                    });

                    // ✅ Concurrency Limiter
                    options.AddConcurrencyLimiter("ConcurrencyRateLimiter", policy =>
                    {
                        policy.PermitLimit = Convert.ToInt32(configuration["RateLimiter:ConcurrencyPermitLimit"]);
                        policy.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                        policy.QueueLimit = Convert.ToInt32(configuration["RateLimiter:ConcurrencyQueueLimit"]);
                    });

                    // ✅ Custom response when rate limit is hit
                    options.OnRejected = async (context, token) =>
                    {
                        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                        context.HttpContext.Response.ContentType = "application/json";

                        var response = new
                        {
                            responseStatusCode = 429,
                            responseMessage = "Too many requests. Please try again later.",
                            success = false
                        };

                        await context.HttpContext.Response.WriteAsJsonAsync(response, cancellationToken: token);
                    };
                });

                return services;
            }
        }
   
}
