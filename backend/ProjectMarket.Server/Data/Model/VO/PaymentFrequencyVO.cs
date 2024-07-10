using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.VO;

public struct PaymentFrequencyVO {
    public int Id { get; set; }
    public string Description { get; set; }
    public string Suffix { get; set; }

    public List<PaymentOffer>? PaymentOffers { get; set; }
}