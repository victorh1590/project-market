using FluentValidation;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Validators;

public class PaymentFrequencyValidator : AbstractValidator<PaymentFrequencyVO>
{
    public PaymentFrequencyValidator()
    {
        const int nameMaximumLength = 64;
        const int suffixMaximumLength = 8;

        RuleFor(paymentFrequency => paymentFrequency.PaymentFrequencyName)
            .NotEmpty()
            .MaximumLength(nameMaximumLength)
            .WithName("PaymentFrequencyName");
        RuleFor(paymentFrequency => paymentFrequency.Suffix)
            .NotEmpty()
            .MaximumLength(suffixMaximumLength)
            .WithName("Suffix");
    }
}