using ProjectMarket.Server.Data.Model.Dto;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Repository;

namespace ProjectMarket.Server.Data.Model.Factory;

public class PaymentOfferFactory(IUnitOfWork uow)
{
    private readonly PaymentFrequencyRepository _paymentFrequencyRepository = new(uow);
    private readonly CurrencyRepository _currencyRepository = new(uow);

    public PaymentOffer CreatePaymentOffer(PaymentOfferDto dto)
    {
        PaymentFrequencyVo paymentFrequencyVo =
            _paymentFrequencyRepository.GetPaymentFrequencyByName(dto.PaymentFrequencyName);
        CurrencyVo currencyVo = _currencyRepository.GetCurrencyByName(dto.CurrencyName);
        PaymentOffer paymentOffer = new(dto.PaymentOfferId, dto.Value, paymentFrequencyVo, currencyVo);
        return paymentOffer;
    }
}