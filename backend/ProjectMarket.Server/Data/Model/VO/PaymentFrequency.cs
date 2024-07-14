using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.VO;

public class PaymentFrequency {
    required public int PaymentFrequencyId { get; set; }
    required public string Description { get; set; }
    required public string Suffix { get; set; }
    //
    public List<PaymentOffer>? PaymentOffers { get; set; }
}