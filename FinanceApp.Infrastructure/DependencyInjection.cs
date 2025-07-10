using FinanceApp.Application.Interfaces;
using FinanceApp.Infrastructure.Services;
using FinanceApp.JWTAuthenticationHandler;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IIncomeRecordService, IncomeRecordService>();
            services.AddScoped<IJwtTokenService, JWTAuthentication>();
            return services;
        }
    }
}
