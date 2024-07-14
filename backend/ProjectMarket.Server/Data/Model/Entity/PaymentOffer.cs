using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Model.Entity;

public class PaymentOffer {
    required public int PaymentOfferId { get; set; }
    required public decimal Value { get; set; }
    required public PaymentFrequency PaymentFrequency { get; set; }
    required public Currency Currency { get; set; }
}
