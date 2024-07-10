using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.VO;

public struct CurrencyVO {
    public int Id { get; set; }
    public decimal Name { get; set; }
    public string Prefix { get; set; }
    
    public List<PaymentOffer>? PaymentOffers { get; set; } 
}
