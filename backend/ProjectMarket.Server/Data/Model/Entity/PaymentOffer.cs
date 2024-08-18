using FluentValidation;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.Entity;

public class PaymentOffer
{
    public int? PaymentOfferId { get; init; }
    public decimal Value { get; set; }
    public PaymentFrequencyVo PaymentFrequency { get; set; }
    public CurrencyVo Currency { get; set; }

    public PaymentOffer(
        int? id,
        decimal value,
        PaymentFrequencyVo paymentFrequency,
        CurrencyVo currency)
    {
        PaymentOfferId = id;
        Value = value;
        PaymentFrequency = paymentFrequency;
        Currency = currency;

        this.Validate();
    }
}

public static class PaymentOfferExtensions {
    private static PaymentOfferValidator Validator { get; } = new();

    public static void Validate(this PaymentOffer paymentOffer) => 
        Validator.ValidateAndThrow(paymentOffer);
}
