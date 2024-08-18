using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public record PaymentFrequencyRecord(string PaymentFrequencyName, string Suffix);
public struct PaymentFrequencyVo : IEquatable<PaymentFrequencyVo>
{
    public string PaymentFrequencyName { get; init; }
    public string Suffix { get; set; }

    public PaymentFrequencyVo(string name, string suffix) {
        PaymentFrequencyName = name;
        Suffix = suffix;

        this.Validate();
    }
    public PaymentFrequencyVo(PaymentFrequencyRecord record) : this(record.PaymentFrequencyName, record.Suffix) {}

    public bool Equals(PaymentFrequencyVo other) => PaymentFrequencyName == other.PaymentFrequencyName && Suffix == other.Suffix;
    public override bool Equals(object? obj) => obj is PaymentFrequencyVo other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(PaymentFrequencyName, Suffix);
    public static bool operator ==(PaymentFrequencyVo left, PaymentFrequencyVo right) => left.Equals(right);
    public static bool operator !=(PaymentFrequencyVo left, PaymentFrequencyVo right) => !left.Equals(right);
}

public static class PaymentFrequencyExtensions {
    private static PaymentFrequencyValidator Validator { get; } = new();

    public static void Validate(this PaymentFrequencyVo paymentFrequency) => 
        Validator.ValidateAndThrow(paymentFrequency);
}