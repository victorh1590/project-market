using ProjectMarket.Server.Data.Model.Interface;

namespace ProjectMarket.Server.Data.Model.Dto;

public class PaymentOfferDto(
    int? id, 
    decimal value, 
    string paymentFrequencyName, 
    string currencyName) : IPaymentOffer<string, string>
{
    public int? PaymentOfferId { get; init; } = id;
    public decimal Value { get; set; } = value;
    public string PaymentFrequency { get; set; } = paymentFrequencyName;
    public string Currency { get; set; } = currencyName;
}
