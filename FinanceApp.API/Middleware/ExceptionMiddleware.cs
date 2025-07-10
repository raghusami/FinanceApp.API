using FinanceApp.Shared.Common.Responses;
using FinanceApp.Shared.Exceptions;
using System.Text.Json;

namespace FinanceApp.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GLOBAL] Unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new MiddlewareApiResponse();
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            switch (exception)
            {
                case UnauthorizedAccessException:
                    response.ResponseStatusCode = StatusCodes.Status401Unauthorized;
                    response.ResponseMessage = "You are not authorized to access this resource.";
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    break;

                case ForbiddenAccessException:
                    response.ResponseStatusCode = StatusCodes.Status403Forbidden;
                    response.ResponseMessage = exception.Message;
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    break;

                case KeyNotFoundException:
                    response.ResponseStatusCode = StatusCodes.Status404NotFound;
                    response.ResponseMessage = "The requested resource was not found.";
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    break;

                case CustomAppException appEx:
                    response.ResponseStatusCode = appEx.StatusCode;
                    response.ResponseMessage = appEx.Message;
                    context.Response.StatusCode = appEx.StatusCode;
                    break;

                case ArgumentException argEx:
                    response.ResponseStatusCode = StatusCodes.Status400BadRequest;
                    response.ResponseMessage = argEx.Message;
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;

                default:
                    response.ResponseStatusCode = StatusCodes.Status500InternalServerError;
                    response.ResponseMessage = "An unexpected error occurred. Please try again later.";
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }
            context.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(response, options);
            return context.Response.WriteAsync(json);
        }
    }

}
