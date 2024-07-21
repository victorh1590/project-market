using FluentValidation;
using ProjectMarket.Server.Data.Model.Interface;

namespace ProjectMarket.Server.Data.Validators;

public class PaymentFrequencyValidator : AbstractValidator<IPaymentFrequency>
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