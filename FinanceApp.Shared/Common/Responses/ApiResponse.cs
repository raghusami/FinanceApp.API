namespace FinanceApp.Shared.Common.Responses
{
    public class ApiResponse<T>
    {
        public int ResponseStatusCode { get; set; }
        public string ResponseMessage { get; set; } = string.Empty;
        public T? ResponseData { get; set; }
        public object? ValidationError { get; set; }
        public bool Success => ResponseStatusCode >= 200 && ResponseStatusCode < 300;

        public static ApiResponse<T> CreateSuccess(T data, string message = "Success", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                ResponseData = data,
                ResponseMessage = message,
                ResponseStatusCode = statusCode
            };
        }

        public static ApiResponse<T> Fail(string message, int statusCode = 400, object? validationError = null)
        {
            return new ApiResponse<T>
            {
                ResponseMessage = message,
                ResponseStatusCode = statusCode,
                ValidationError = validationError
            };
        }

        public static ApiResponse<T> Empty(string message = "No content", int statusCode = 204)
        {
            return new ApiResponse<T>
            {
                ResponseMessage = message,
                ResponseStatusCode = statusCode
            };
        }

    }
}
