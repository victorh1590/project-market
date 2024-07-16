using FluentValidation;
using ProjectMarket.Server.Data.Validators;
namespace ProjectMarket.Server.Data.Model.VO;

public struct PaymentFrequencyVO {
    required public string PaymentFrequencyName { get; set; }
    required public string Suffix { get; set; }

    public PaymentFrequencyVO(string name, string suffix) {
        PaymentFrequencyName = name;
        Suffix = suffix;

        var validator = new PaymentFrequencyValidator();
        validator.ValidateAndThrow(this);
    }
}