using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinanceApp.JWTAuthenticationHandler
{
    public class JWTAuthentication : IJwtTokenService
    {
        private readonly string authIssuer;
        private readonly string authAudience;
        private readonly string securityKey;
        private readonly int expiresInMinutes;
        public JWTAuthentication(IConfiguration _IConfiguration)
        {
            this.authIssuer = Convert.ToString(_IConfiguration["AuthConfiguration:Issuer"]);
            this.authAudience = Convert.ToString(_IConfiguration["AuthConfiguration:Audience"]);
            this.securityKey = Convert.ToString(_IConfiguration["AuthConfiguration:SecurityKey"]);
            this.expiresInMinutes = Convert.ToInt32(_IConfiguration["AuthConfiguration:ExpiresInMinutes"]);

        }
        public string GenerateToken(UserInformation userInformation)
        {
            string keyValue = string.IsNullOrEmpty(this.securityKey) ? JWTAuthValidator.CustomSecurityKey : this.securityKey;
            byte[] securityKeyBytes = Encoding.UTF8.GetBytes(keyValue);

            JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
            IList<Claim> claimData = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userInformation?.UserName),
                new Claim(ClaimTypes.Email, userInformation?.EmailId ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, userInformation?.UserUniqueId),
                new Claim("CreatedAt", DateTime.UtcNow.ToString()),
            };
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimData),
                Expires = expiresInMinutes > 0 ? DateTime.UtcNow.AddMinutes(expiresInMinutes) : DateTime.UtcNow.AddMinutes(30),
                Issuer = !string.IsNullOrEmpty(this.authIssuer) ? this.authIssuer : "INVENTSOFTLABS",
                Audience = !string.IsNullOrEmpty(this.authAudience) ? this.authAudience : "INVENTSOFTLABS",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKeyBytes), SecurityAlgorithms.HmacSha256),
            };
            return securityTokenHandler.WriteToken(securityTokenHandler.CreateToken(securityTokenDescriptor));
        }
    }
    
}
