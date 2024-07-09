namespace ProjectMarket.Server.Data.Model;

public class PaymentOffer {
    public int Id { get; set; }
    public decimal Value { get; set; }
    public CurrencyVO Currency { get; set; }
}
