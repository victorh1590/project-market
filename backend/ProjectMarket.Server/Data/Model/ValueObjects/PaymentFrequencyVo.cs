using FluentValidation;
using ProjectMarket.Server.Data.Model.Dto.ValueObjectDto;
using ProjectMarket.Server.Data.Model.Interface;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public struct PaymentFrequencyVo : IPaymentFrequency
{
    public string PaymentFrequencyName { get; init; }
    public string Suffix { get; set; }

    public PaymentFrequencyVo(string name, string suffix) {
        PaymentFrequencyName = name;
        Suffix = suffix;

        this.Validate();
    }
    
    public PaymentFrequencyVo(PaymentFrequencyDto dto) : 
        this(dto.PaymentFrequencyName, dto.Suffix)
    {
    }
}

public static class PaymentFrequencyExtensions {
    private static PaymentFrequencyValidator Validator { get; } = new();

    public static void Validate(this IPaymentFrequency paymentFrequency) => 
        Validator.ValidateAndThrow(paymentFrequency);
}