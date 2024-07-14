using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.VO;

public class Currency {
    required public int CurrencyId { get; set; }
    required public decimal Name { get; set; }
    required public string Prefix { get; set; }
    //
    public List<PaymentOffer>? PaymentOffers { get; set; } 
}
