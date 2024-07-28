using ProjectMarket.Server.Data.Model.Dto;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Repository;

namespace ProjectMarket.Server.Data.Model.Factory;

public class PaymentOfferFactory(PaymentFrequencyRepository paymentFrequencyRepository, CurrencyRepository currencyRepository)
{
    public PaymentOffer CreatePaymentOffer(PaymentOfferDto dto)
    {
        PaymentFrequencyVo paymentFrequencyVo =
            paymentFrequencyRepository.GetPaymentFrequencyByName(dto.PaymentFrequencyName);
        CurrencyVo currencyVo = currencyRepository.GetCurrencyByName(dto.CurrencyName);
        PaymentOffer paymentOffer = new(dto.PaymentOfferId, dto.Value, paymentFrequencyVo, currencyVo);
        return paymentOffer;
    }
}