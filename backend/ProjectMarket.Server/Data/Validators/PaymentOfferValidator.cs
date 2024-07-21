using FluentValidation;
using ProjectMarket.Server.Data.Model.Dto;
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

public class PaymentOfferDtoValidator : AbstractValidator<PaymentOfferDto> 
{   
    private static decimal MaximumValue => 1_000_000_000_000_000.00M;
    private static int ValueMaxSize => 17;
    private static int ValuePrecision => 2;
    private static bool IgnoreTrailingZeros => false;
    public PaymentOfferDtoValidator()
    {
        RuleFor(paymentOffer => paymentOffer.PaymentOfferId ?? 0)
            .SetValidator(new PaymentOfferIdValidator())
            .WithName("PaymentOfferId")
            .Unless(paymentOffer => paymentOffer.PaymentOfferId == null);
        RuleFor(paymentOffer => paymentOffer.Value)
            .InclusiveBetween(0.0M, MaximumValue)
            .PrecisionScale(ValueMaxSize, ValuePrecision, IgnoreTrailingZeros)
            .WithName("Value");
        RuleFor(paymentOffer => paymentOffer.PaymentFrequencyName)
            .NotNull()
            .SetValidator(new PaymentFrequencyNameValidator())
            .WithName("PaymentFrequency");
        RuleFor(paymentOffer => paymentOffer.CurrencyName)
            .NotNull()
            .SetValidator(new CurrencyNameValidator())
            .WithName("Currency");
    }
}

public class PaymentOfferIdValidator : AbstractValidator<int> 
{   
    public PaymentOfferIdValidator()
    {
        RuleFor(paymentOfferId => paymentOfferId)
            .GreaterThan(0)
            .WithName("PaymentOfferId");
    }
}