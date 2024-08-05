using FluentValidation;
using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Validators;

public class PaymentOfferValidator : AbstractValidator<PaymentOffer> 
{
    private static decimal MaximumValue => 1_000_000_000_000_000.00M;
    private static int ValueMaxSize => 17;
    private static int ValuePrecision => 2;
    private static bool IgnoreTrailingZeros => false;
    public PaymentOfferValidator()
    {
        RuleFor(paymentOffer => paymentOffer.PaymentOfferId)
            .GreaterThan(0)
            .WithName("PaymentOfferId")
            .Unless(paymentOffer => paymentOffer.PaymentOfferId == null);
        RuleFor(paymentOffer => paymentOffer.Value)
            .NotNull()
            .InclusiveBetween(0.0M, MaximumValue)
            .PrecisionScale(ValueMaxSize, ValuePrecision, IgnoreTrailingZeros)
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