using FluentValidation;
using ProjectMarket.Server.Data.Model.Dto.ValueObjectDto;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public struct CurrencyVo : ICurrency
{
    public string CurrencyName { get; init; }
    public string Prefix { get; set; }

    public CurrencyVo(string name, string prefix) {
        CurrencyName = name;
        Prefix = prefix;

        this.Validate();
    }

    public CurrencyVo(CurrencyDto dto) : this(dto.CurrencyName, dto.Prefix)
    {
    }
}

public static class CurrencyExtensions {
    private static CurrencyValidator Validator { get; } = new();

    public static void Validate(this ICurrency currency) => 
        Validator.ValidateAndThrow(currency);
}