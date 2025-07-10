using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.API.Extensions
{
    public static class ApiBehaviorOptionsExtensions
    {
        public static IServiceCollection ConfigureCustomModelValidation(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value?.Errors.Count > 0)
                        .Select(e => new
                        {
                            field = e.Key,
                            errors = e.Value?.Errors.Select(x => x.ErrorMessage).ToList()
                        });

                    var response = new
                    {
                        responseStatusCode = StatusCodes.Status400BadRequest,
                        responseMessage = "Validation failed",
                        validationError = errors,
                        success = false
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }
    }
}
