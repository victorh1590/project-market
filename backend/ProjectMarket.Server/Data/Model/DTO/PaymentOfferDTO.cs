using ProjectMarket.Server.Data.Model.Interface;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Model.Dto;

public class PaymentOfferDto(
    int? id, 
    decimal value, 
    PaymentFrequencyVO paymentFrequency, 
    CurrencyVO currency) : IPaymentOffer
{
    public int? PaymentOfferId { get; init; } = id;
    public decimal Value { get; set; } = value;
    public PaymentFrequencyVO PaymentFrequency { get; set; } = paymentFrequency;
    public CurrencyVO Currency { get; set; } = currency;
}
