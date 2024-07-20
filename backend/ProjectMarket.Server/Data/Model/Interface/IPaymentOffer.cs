using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Model.Interface;

public interface IPaymentOffer
{
    int? PaymentOfferId { get; }
    decimal Value { get; }
    PaymentFrequencyVO PaymentFrequency { get; }
    CurrencyVO Currency { get; }
}