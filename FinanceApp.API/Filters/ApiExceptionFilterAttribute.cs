using FinanceApp.Shared.Common.Responses;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinanceApp.API.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<ApiExceptionFilterAttribute> _logger;

        public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "[API EXCEPTION] Unhandled exception occurred.");

            var response = new ApiResponse<string>
            {
                ResponseStatusCode = (int)HttpStatusCode.InternalServerError,
                ResponseMessage = "Something went wrong!",
                ValidationError = context.Exception.Message
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
