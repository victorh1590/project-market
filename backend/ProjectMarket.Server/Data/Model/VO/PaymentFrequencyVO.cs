using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.VO;

public struct PaymentFrequencyVO {
    required public int Id { get; set; }
    required public string Description { get; set; }
    required public string Suffix { get; set; }
    //
    public List<PaymentOffer>? PaymentOffers { get; set; }
}