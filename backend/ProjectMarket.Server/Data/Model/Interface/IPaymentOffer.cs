using ProjectMarket.Server.Data.Model.ValueObjects;

namespace ProjectMarket.Server.Data.Model.Interface;

public interface IPaymentOffer
{
    int? PaymentOfferId { get; }
    decimal Value { get; }
    IPaymentFrequency PaymentFrequency { get; }
    ICurrency Currency { get; }
}