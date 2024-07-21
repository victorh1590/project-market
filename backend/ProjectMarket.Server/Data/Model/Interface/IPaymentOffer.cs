using ProjectMarket.Server.Data.Model.ValueObjects;

namespace ProjectMarket.Server.Data.Model.Interface;

public interface IPaymentOffer<out T1, out T2>
{
    int? PaymentOfferId { get; }
    decimal Value { get; }
    T1 PaymentFrequency { get; }
    T2 Currency { get; }
}