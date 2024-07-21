using FluentValidation;
using ProjectMarket.Server.Data.Model.ValueObjects;

namespace ProjectMarket.Server.Data.Validators
{
    public class CurrencyValidator : AbstractValidator<CurrencyVo>
    {
        public CurrencyValidator()
        {
            RuleFor(currency => currency.CurrencyName)
                .SetValidator(new CurrencyNameValidator())
                .WithName("CurrencyName");

            RuleFor(currency => currency.Prefix)
                .SetValidator(new CurrencyPrefixValidator())
                .WithName("Prefix");
        }
    }

    public class CurrencyNameValidator : AbstractValidator<string>
    {
        private static int NameMaximumLength => 64;
    
        public CurrencyNameValidator()
        {
            RuleFor(currencyName => currencyName)
                .NotEmpty()
                .MaximumLength(NameMaximumLength)
                .WithName("CurrencyName");
        }
    }

    public class CurrencyPrefixValidator : AbstractValidator<string>
    {
        private static int PrefixMaximumLength => 8;

        public CurrencyPrefixValidator()
        {
            RuleFor(prefix => prefix)
                .NotEmpty()
                .MaximumLength(PrefixMaximumLength)
                .WithName("Prefix");
        }
    }
}