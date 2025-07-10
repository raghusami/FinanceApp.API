namespace FinanceApp.Shared.Common.Responses
{
    public class MiddlewareApiResponse
    {
        public int ResponseStatusCode { get; set; }             // HTTP status code (e.g., 200, 400)
        public string ResponseMessage { get; set; } = string.Empty; // User-readable message
        public object? ResponseData { get; set; }               // Main data (optional)
        public object? ValidationError { get; set; }            // Error details (optional)
        public bool Success => ResponseStatusCode >= 200 && ResponseStatusCode < 300; // Computed
    }
}
