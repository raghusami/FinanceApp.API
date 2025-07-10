using FinanceApp.Application.Validator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceApp.Infrastructure.Extensions
{
    public static class ValidationServiceCollectionExtensions
    {
        public static IServiceCollection AddValidationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<IncomeRecordDtoValidator>();
            return services;
        }
    }
}
