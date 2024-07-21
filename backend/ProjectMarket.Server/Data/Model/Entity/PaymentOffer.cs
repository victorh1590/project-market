using FluentValidation;
using ProjectMarket.Server.Data.Model.Dto.EntityDto;
using ProjectMarket.Server.Data.Model.Dto.ValueObjectDto;
using ProjectMarket.Server.Data.Model.Interface;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.Entity;

public class PaymentOffer : IPaymentOffer
{
    public int? PaymentOfferId { get; init; }
    public decimal Value { get; set; }
    public IPaymentFrequency PaymentFrequency { get; set; }
    public ICurrency Currency { get; set; }

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
    
    public static PaymentOffer CreatePaymentOffer(PaymentOfferDto dto)
    {
        return new PaymentOffer(
            dto.PaymentOfferId, 
            dto.Value, 
            new PaymentFrequencyVo((PaymentFrequencyDto) dto.PaymentFrequency), 
            new CurrencyVo((CurrencyDto) dto.Currency) 
        );
    }
}

public static class PaymentOfferExtensions {
    private static PaymentOfferValidator Validator { get; } = new();

    public static void Validate(this PaymentOffer paymentOffer) => 
        Validator.ValidateAndThrow(paymentOffer);
}
