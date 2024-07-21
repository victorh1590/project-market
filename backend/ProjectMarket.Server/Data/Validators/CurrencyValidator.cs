using FluentValidation;
using ProjectMarket.Server.Data.Model.ValueObjects;

namespace ProjectMarket.Server.Data.Validators;

public class CurrencyValidator : AbstractValidator<CurrencyVo>
{
    public CurrencyValidator()
    {
        const int nameMaximumLength = 64;
        const int prefixMaximumLength = 8;

        RuleFor(currency => currency.CurrencyName)
            .NotEmpty()
            .MaximumLength(nameMaximumLength)
            .WithName("CurrencyName");
        RuleFor(currency => currency.Prefix)
            .NotEmpty()
            .MaximumLength(prefixMaximumLength)
            .WithName("Prefix");
    }
}