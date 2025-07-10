namespace FinanceApp.JWTAuthenticationHandler
{
    public interface IJwtTokenService
    {
        string GenerateToken(UserInformation userInfo);
    }
}
