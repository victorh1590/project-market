namespace SupportCentral.Server.Data.Model;

public class PaymentOffer {
    public int Id { get; set; }
    public decimal Value { get; set; }
    public Currency Currency { get; set; }
}
