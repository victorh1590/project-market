using ProjectMarket.Server.Data.Model.ValueObjects;

namespace ProjectMarket.Server.Data.Model.Dto.ValueObjectDto;

public struct CurrencyDto(string currencyName, string prefix = "") : ICurrency
{
    public string CurrencyName { get; init;  } = currencyName;
    public string Prefix { get; set;  } = prefix;
}