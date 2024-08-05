using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public record PaymentFrequencyRecord(string PaymentFrequencyName, string Suffix);

public struct PaymentFrequencyVo
{
    public required string PaymentFrequencyName { get; init; }
    public required string Suffix { get; set; }

    public PaymentFrequencyVo(string name, string suffix) {
        PaymentFrequencyName = name;
        Suffix = suffix;

        this.Validate();
    }
    
    public PaymentFrequencyVo(PaymentFrequencyRecord record) : this(record.PaymentFrequencyName, record.Suffix) {}
}

public static class PaymentFrequencyExtensions {
    private static PaymentFrequencyValidator Validator { get; } = new();

    public static void Validate(this PaymentFrequencyVo paymentFrequency) => 
        Validator.ValidateAndThrow(paymentFrequency);
}