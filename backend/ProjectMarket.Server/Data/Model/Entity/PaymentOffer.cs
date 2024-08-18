using FluentValidation;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.Entity;

public class PaymentOffer : IEquatable<PaymentOffer>
{
    public int? PaymentOfferId { get; init; }
    public decimal Value { get; set; }
    public PaymentFrequencyVo PaymentFrequency { get; set; }
    public CurrencyVo Currency { get; set; }

    public PaymentOffer(
        int? paymentOfferId,
        decimal value,
        PaymentFrequencyVo paymentFrequency,
        CurrencyVo currency)
    {
        PaymentOfferId = paymentOfferId;
        Value = value;
        PaymentFrequency = paymentFrequency;
        Currency = currency;

        this.Validate();
    }

    public override bool Equals(object? obj)
        => ReferenceEquals(this, obj) ||
           (obj is PaymentOffer other &&
            PaymentOfferId == other.PaymentOfferId &&
            Value == other.Value &&
            PaymentFrequency.Equals(other.PaymentFrequency) &&
            Currency.Equals(other.Currency));

    public override int GetHashCode()
        => HashCode.Combine(PaymentOfferId, Value, PaymentFrequency, Currency);

    public bool Equals(PaymentOffer? obj)
        => ReferenceEquals(this, obj) ||
           (obj != null &&
            PaymentOfferId == obj.PaymentOfferId &&
            Value == obj.Value &&
            PaymentFrequency.Equals(obj.PaymentFrequency) &&
            Currency.Equals(obj.Currency));
}

public static class PaymentOfferExtensions {
    private static PaymentOfferValidator Validator { get; } = new();

    public static void Validate(this PaymentOffer paymentOffer) => 
        Validator.ValidateAndThrow(paymentOffer);
}
