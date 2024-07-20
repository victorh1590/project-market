using FluentValidation;
using ProjectMarket.Server.Data.Model.Dto;
using ProjectMarket.Server.Data.Model.Interface;
using ProjectMarket.Server.Data.Validators;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Model.Entity;

public class PaymentOffer : IPaymentOffer
{
    public int? PaymentOfferId { get; init; }
    public decimal Value { get; set; }
    public PaymentFrequencyVO PaymentFrequency { get; set; }
    public CurrencyVO Currency { get; set; }

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
    
    public static PaymentOffer CreatePaymentOffer(PaymentOfferDto dto)
    {
        return new PaymentOffer(
            dto.PaymentOfferId, 
            dto.Value, 
            dto.PaymentFrequency, 
            dto.Currency 
        );
    }
}

public static class PaymentOfferExtensions {
    private static PaymentOfferValidator Validator { get; } = new();

    public static void Validate(this PaymentOffer paymentOffer) => 
        Validator.ValidateAndThrow(paymentOffer);
}
