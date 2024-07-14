using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.VO;

public struct Currency {
    required public int CurrencyId { get; set; }
    required public decimal Name { get; set; }
    required public string Prefix { get; set; }
}
