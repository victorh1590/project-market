using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectMarket.Server.Data.Validators;
using ProjectMarket.Server.Infra.Repository;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public struct CurrencyVo
{
    public required string CurrencyName { get; init; }
    public required string Prefix { get; set; }

    public CurrencyVo(string name, string prefix)
    {
        CurrencyName = name;
        Prefix = prefix;

        this.Validate();
    }
}

public static class CurrencyExtensions {
    private static CurrencyValidator Validator { get; } = new();

    public static void Validate(this CurrencyVo currency) => 
        Validator.ValidateAndThrow(currency);
}