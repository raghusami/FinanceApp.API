using FinanceApp.Shared.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApp.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseController : ControllerBase
    {
        protected OkObjectResult SuccessResponse(string? message = null)
        {
            var response = ApiResponse<string>.CreateSuccess(null, GetMessageOrDefault(message, "Operation successful"));
            return Ok(response);
        }

        protected OkObjectResult SuccessResponseWithData<T>(T data, string? message = null)
        {
            var response = ApiResponse<T>.CreateSuccess(data, GetMessageOrDefault(message, "Operation successful"));
            return Ok(response);
        }

        protected UnauthorizedObjectResult UnauthorizedResponse(string? message = null)
        {
            var response = ApiResponse<string>.Fail(GetMessageOrDefault(message, "Unauthorized access"), StatusCodes.Status401Unauthorized);
            return Unauthorized(response);
        }

        protected OkObjectResult DeleteSuccessResponse(string? message = null)
        {
            var response = ApiResponse<string>.CreateSuccess(null, GetMessageOrDefault(message, "Deleted successfully"));
            return Ok(response);
        }

        protected BadRequestObjectResult BadRequestResponse(string? message = null, object? validationError = null)
        {
            var response = ApiResponse<string>.Fail(GetMessageOrDefault(message, "Bad request"), StatusCodes.Status400BadRequest, validationError);
            return BadRequest(response);
        }

        protected ObjectResult InternalServerErrorResponse(string? message = null)
        {
            var response = ApiResponse<string>.Fail(GetMessageOrDefault(message, "Internal server error"), StatusCodes.Status500InternalServerError);
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }

        protected NotFoundObjectResult NotFoundResponse(string? message = null)
        {
            var response = ApiResponse<string>.Fail(GetMessageOrDefault(message, "Resource not found"), StatusCodes.Status404NotFound);
            return NotFound(response);
        }

        protected OkObjectResult TransactionExistResponse(string? message = null)
        {
            var response = ApiResponse<string>.CreateSuccess(null, GetMessageOrDefault(message, "Transaction already exists"));
            return Ok(response);
        }

        private string GetMessageOrDefault(string? message, string defaultMessage)
        {
            return string.IsNullOrWhiteSpace(message) ? defaultMessage : message;
        }
    }

}
