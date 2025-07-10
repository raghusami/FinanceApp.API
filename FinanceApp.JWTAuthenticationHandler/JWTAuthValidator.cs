using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FinanceApp.JWTAuthenticationHandler
{
    public static class JWTAuthValidator
    {
        public const string CustomSecurityKey = "SU5WRU5UU09GVExBQlNKV1RBVVRIRU5USUNBVElPTktFWTIwMjM=";

        public static IServiceCollection JWTConfigValidator(this IServiceCollection services, IConfiguration configuration)
        {
            // Register user claim helpers
            services.AddHttpContextAccessor();
            services.AddScoped<IUserClaimManager, UserClaimManager>();

            string securityKey = configuration["AuthConfiguration:SecurityKey"];
            securityKey = string.IsNullOrEmpty(securityKey) ? CustomSecurityKey : securityKey;
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["AuthConfiguration:Issuer"],
                    ValidAudience = configuration["AuthConfiguration:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("🔴 AUTH ERROR: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        Console.WriteLine("🔴 CHALLENGE ERROR: " + context.ErrorDescription);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token Validated: " + context.Principal?.Identity?.Name);
                        return Task.CompletedTask;
                    },
                };
            });

            services.AddAuthorization();

            return services;
        }
    }

}
