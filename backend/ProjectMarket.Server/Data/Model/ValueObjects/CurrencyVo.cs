using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProjectMarket.Server.Data.Validators;
using ProjectMarket.Server.Infra.Repository;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public struct CurrencyVo
{
    public string CurrencyName { get; init; }
    public string Prefix { get; set; }

    public CurrencyVo(string name, string prefix) {
        CurrencyName = name;
        Prefix = prefix;

        this.Validate();
    }

    public static CurrencyVo? CreateCurrencyVo(string currencyName, [FromServices] IUnitOfWork uow)
    {
        CurrencyRepository currencyRepository = new(uow);
        return currencyRepository.GetByCurrencyName(currencyName);
    }
}

public static class CurrencyExtensions {
    private static CurrencyValidator Validator { get; } = new();

    public static void Validate(this CurrencyVo currency) => 
        Validator.ValidateAndThrow(currency);
}