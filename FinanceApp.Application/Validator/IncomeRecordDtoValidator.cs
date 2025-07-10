using FinanceApp.Application.DTOs;
using FluentValidation;

namespace FinanceApp.Application.Validator
{
    public class IncomeRecordDtoValidator : AbstractValidator<IncomeRecordDto>
    {
        public IncomeRecordDtoValidator()
        {
            RuleFor(x => x.IncomeDate)
             .NotNull().WithMessage("Income date is required.");

            RuleFor(x => x.IncomeSource)
                .NotEmpty().WithMessage("Income source is required.")
                .MaximumLength(100);

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.IncomeType)
                .NotNull().WithMessage("Income type is required.");
        }
    }
}
