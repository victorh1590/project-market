using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Model.Entity;

public class PaymentOfferDTO {
    required public int? PaymentOfferId { get; set; }
    required public decimal Value { get; set; }
    required public PaymentFrequencyVO PaymentFrequency { get; set; }
    required public CurrencyVO Currency { get; set; }

    public PaymentOfferDTO(
        int? id,
        decimal value,
        PaymentFrequencyVO paymentFrequency,
        CurrencyVO currency)
    {
        PaymentOfferId = id;
        Value = value;
        PaymentFrequency = paymentFrequency;
        Currency = currency;
    }
}
