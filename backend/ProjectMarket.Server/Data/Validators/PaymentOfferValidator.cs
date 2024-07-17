using FluentValidation;
using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Validators;

public class PaymentOfferValidator : AbstractValidator<PaymentOffer>
{
    public PaymentOfferValidator()
    {
        const decimal maximumValue = 1_000_000_000_000_000.00M;
        const int valueMaxSize = 17;
        const int valuePrecision = 2;
        const bool ignoreTrailingZeros = false;

        RuleFor(paymentOffer => paymentOffer.PaymentOfferId)
            .GreaterThan(0)
            .WithName("PaymentOfferId");
        RuleFor(paymentOffer => paymentOffer.Value)
            .InclusiveBetween(0.0M, maximumValue)
            .PrecisionScale(valueMaxSize, valuePrecision, ignoreTrailingZeros)
            .WithName("Value");
        RuleFor(paymentOffer => paymentOffer.PaymentFrequency)
            .NotNull()
            .SetValidator(new PaymentFrequencyValidator())
            .WithName("PaymentFrequency");
        RuleFor(paymentOffer => paymentOffer.Currency)
            .NotNull()
            .SetValidator(new CurrencyValidator())
            .WithName("Currency");
    }
}