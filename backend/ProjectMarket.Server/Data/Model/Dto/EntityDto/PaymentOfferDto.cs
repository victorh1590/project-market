using ProjectMarket.Server.Data.Model.Dto.ValueObjectDto;
using ProjectMarket.Server.Data.Model.Interface;
using ProjectMarket.Server.Data.Model.ValueObjects;

namespace ProjectMarket.Server.Data.Model.Dto.EntityDto;

public class PaymentOfferDto(
    int? id, 
    decimal value, 
    string paymentFrequencyName, 
    string currencyName) : IPaymentOffer
{
    public int? PaymentOfferId { get; init; } = id;
    public decimal Value { get; set; } = value;
    public IPaymentFrequency PaymentFrequency { get; set; } = new PaymentFrequencyDto(paymentFrequencyName);
    public ICurrency Currency { get; set; } = new CurrencyDto(currencyName);
}
