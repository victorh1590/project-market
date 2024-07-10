using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Model.Entity;

public class PaymentOffer {
    public int Id { get; set; }
    public decimal Value { get; set; }
    public PaymentFrequencyVO PaymentFrequency { get; set; }
    public CurrencyVO Currency { get; set; }
}
