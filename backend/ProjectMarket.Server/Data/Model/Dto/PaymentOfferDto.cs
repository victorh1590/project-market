using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.Dto;

public class PaymentOfferDto
{
    public int? PaymentOfferId { get; init; }
    public decimal Value { get; set; }
    public string PaymentFrequencyName { get; set; }
    public string CurrencyName { get; set; }

    public PaymentOfferDto(
        int? id, 
        decimal value, 
        string paymentFrequencyNameName, 
        string currencyNameName)
    {
        PaymentOfferId = id;
        Value = value;
        PaymentFrequencyName = paymentFrequencyNameName;
        CurrencyName = currencyNameName;
    }
}

public static class PaymentOfferDtoExtensions {
    private static PaymentOfferDtoValidator Validator { get; } = new();

    public static void Validate(this PaymentOfferDto paymentOffer) => 
        Validator.ValidateAndThrow(paymentOffer);
}
