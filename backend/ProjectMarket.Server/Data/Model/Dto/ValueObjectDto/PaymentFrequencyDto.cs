using ProjectMarket.Server.Data.Model.Interface;

namespace ProjectMarket.Server.Data.Model.Dto.ValueObjectDto;

public struct PaymentFrequencyDto(
    string paymentFrequencyName, 
    string suffix = "") : IPaymentFrequency
{
    public string PaymentFrequencyName { get; init; } = paymentFrequencyName;
    public string Suffix { get; set; } = suffix;
}