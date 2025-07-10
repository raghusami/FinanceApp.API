using Microsoft.AspNetCore.Http;


namespace FinanceApp.Shared.Exceptions
{

    public class CustomAppException : Exception
    {
        public int StatusCode { get; }

        public CustomAppException(string message, int statusCode = StatusCodes.Status400BadRequest)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
