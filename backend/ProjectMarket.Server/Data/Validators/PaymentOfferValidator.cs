using FluentValidation;
using ProjectMarket.Server.Data.Model.Dto;
using ProjectMarket.Server.Data.Model.Interface;

namespace ProjectMarket.Server.Data.Validators;

public class PaymentOfferValidator : AbstractValidator<IPaymentOffer>
{
    public PaymentOfferValidator()
    {
        const decimal maximumValue = 1_000_000_000_000_000.00M;
        const int valueMaxSize = 17;
        const int valuePrecision = 2;
        const bool ignoreTrailingZeros = false;

        RuleFor(paymentOffer => paymentOffer.PaymentOfferId)
            .NotNull()
            .GreaterThan(0)
            .WithName("PaymentOfferId")
            .Unless(paymentOffer => paymentOffer is PaymentOfferDto);
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