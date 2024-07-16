using FluentValidation;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Validators;

public class CurrencyValidator : AbstractValidator<CurrencyVO>
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