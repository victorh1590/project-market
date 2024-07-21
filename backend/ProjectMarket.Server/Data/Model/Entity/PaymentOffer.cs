using FluentValidation;
using ProjectMarket.Server.Data.Model.Interface;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.Entity;

public class PaymentOffer : IPaymentOffer<PaymentFrequencyVo, CurrencyVo>
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

    // Let's use a factory instead.
    // public static PaymentOffer CreatePaymentOffer(PaymentOfferDto dto)
    // {
    //     return new PaymentOffer(
    //         dto.PaymentOfferId, 
    //         dto.Value, 
    //         new PaymentFrequencyVo(dto.PaymentFrequency), 
    //         new CurrencyVo(CurrencyFactory) 
    //     );
    // }
}

public static class PaymentOfferExtensions {
    private static PaymentOfferValidator Validator { get; } = new();

    public static void Validate(this PaymentOffer paymentOffer) => 
        Validator.ValidateAndThrow(paymentOffer);
}
