namespace ProjectMarket.Server.Data.Model.Dto;

public class PaymentOfferDto(
    int? id,
    decimal value,
    string paymentFrequencyNameName,
    string currencyNameName)
{
    public int? PaymentOfferId { get; init; } = id;
    public decimal Value { get; set; } = value;
    public string PaymentFrequencyName { get; set; } = paymentFrequencyNameName;
    public string CurrencyName { get; set; } = currencyNameName;
}