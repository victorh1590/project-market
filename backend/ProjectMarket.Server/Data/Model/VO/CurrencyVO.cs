using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.VO;

public struct CurrencyVO {
    required public string CurrencyName { get; set; }
    required public string Prefix { get; set; }

    public CurrencyVO(string name, string prefix) {
        CurrencyName = name;
        Prefix = prefix;

        var validator = new CurrencyValidator();
        validator.ValidateAndThrow(this);
    }
}
