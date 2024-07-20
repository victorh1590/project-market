using FluentValidation;
using ProjectMarket.Server.Data.Validators;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Model.Entity;

public class PaymentOffer {
    required public int? PaymentOfferId { get; set; }
    required public decimal Value { get; set; }
    required public PaymentFrequencyVO PaymentFrequency { get; set; }
    required public CurrencyVO Currency { get; set; }

    public PaymentOffer(
        int? id,
        decimal value,
        PaymentFrequencyVO paymentFrequency,
        CurrencyVO currency)
    {
        PaymentOfferId = id;
        Value = value;
        PaymentFrequency = paymentFrequency;
        Currency = currency;

        this.Validate();
    }
}

public static class PaymentOfferExtensions {
    public static PaymentOfferValidator Validator { get; private set; } = new();

    public static void Validate(this PaymentOffer paymentOffer) => 
        Validator.ValidateAndThrow(paymentOffer);
}
