using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public struct PaymentFrequencyVo
{
    public string PaymentFrequencyName { get; init; }
    public string Suffix { get; set; }

    public PaymentFrequencyVo(string name, string suffix) {
        PaymentFrequencyName = name;
        Suffix = suffix;

        this.Validate();
    }
}

public static class PaymentFrequencyExtensions {
    private static PaymentFrequencyValidator Validator { get; } = new();

    public static void Validate(this PaymentFrequencyVo paymentFrequency) => 
        Validator.ValidateAndThrow(paymentFrequency);
}