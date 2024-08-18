using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public record CurrencyRecord(string CurrencyName, string Prefix);
public struct CurrencyVo : IEquatable<CurrencyVo>
{
    public string CurrencyName { get; init; }
    public string Prefix { get; set; }

    public CurrencyVo(string name, string prefix)
    {
        CurrencyName = name;
        Prefix = prefix;

        this.Validate();
    }
    public CurrencyVo(CurrencyRecord record) : this(record.CurrencyName, record.Prefix) {}

    public bool Equals(CurrencyVo other) => CurrencyName == other.CurrencyName && Prefix == other.Prefix;
    public override bool Equals(object? obj) => obj is CurrencyVo other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(CurrencyName, Prefix);
    public static bool operator ==(CurrencyVo left, CurrencyVo right) => left.Equals(right);
    public static bool operator !=(CurrencyVo left, CurrencyVo right) => !(left == right);
}

public static class CurrencyExtensions {
    private static CurrencyValidator Validator { get; } = new();

    public static void Validate(this CurrencyVo currency) =>
        Validator.ValidateAndThrow(currency);
}