using Microsoft.AspNetCore.Mvc;
using FinanceApp.Shared.Models;
using FinanceApp.JWTAuthenticationHandler;
namespace FinanceApp.API.Controllers
{   
    public class AuthController : BaseController
    {
       
         private readonly IJwtTokenService _jwtTokenService;
        public AuthController(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Email == "admin@example.com" && request.Password == "admin123")
            {
                UserInformation userInformation = new UserInformation
                {
                    UserUniqueId = "1001",
                    EmailId = request.Email,
                    UserName = "Admin"
                };
                var token = _jwtTokenService.GenerateToken(userInformation);
                return Ok(new LoginResponse { Email = request.Email, Token = token });
            }
            return UnauthorizedResponse("Invalid credentials");
        }

    }
}
