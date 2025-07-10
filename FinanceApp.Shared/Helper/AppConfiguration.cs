namespace FinanceApp.Shared.Helper
{
    public class AppConfiguration
    {
        public DBConfiguration? DBConfiguration { get; set; } = new();
        public AuthConfiguration? authConfiguration { get; set; } = new();
    }


    public record AuthConfiguration
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiresInMinutes { get; set; } = 60;
    }
    public record DBConfiguration
    {
        public string CoreDBConnectionString { get; set; }= string.Empty;
    }
}
