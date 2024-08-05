using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public record CurrencyRecord(string CurrencyName, string Prefix);

public struct CurrencyVo
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
}

public static class CurrencyExtensions {
    private static CurrencyValidator Validator { get; } = new();

    public static void Validate(this CurrencyVo currency) =>
        Validator.ValidateAndThrow(currency);
}