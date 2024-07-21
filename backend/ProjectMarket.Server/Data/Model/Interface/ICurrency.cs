namespace ProjectMarket.Server.Data.Model.ValueObjects;

public interface ICurrency
{
    string CurrencyName { get; }
    string Prefix { get; }
}