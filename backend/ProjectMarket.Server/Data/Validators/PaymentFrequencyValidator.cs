using FluentValidation;
using ProjectMarket.Server.Data.Model.ValueObjects;

namespace ProjectMarket.Server.Data.Validators;

public class PaymentFrequencyValidator : AbstractValidator<PaymentFrequencyVo>
{
    public PaymentFrequencyValidator()
    {
        RuleFor(paymentFrequency => paymentFrequency.PaymentFrequencyName)
            .SetValidator(new PaymentFrequencyNameValidator())
            .WithName("PaymentFrequencyName");
        RuleFor(paymentFrequency => paymentFrequency.Suffix)
            .SetValidator(new PaymentFrequencySuffixValidator())
            .WithName("Suffix");
    }
}

public class PaymentFrequencyNameValidator : AbstractValidator<string>
{
    private static int NameMaximumLength => 64;
    
    public PaymentFrequencyNameValidator()
    {

        RuleFor(paymentFrequencyName => paymentFrequencyName)
            .NotEmpty()
            .MaximumLength(NameMaximumLength)
            .WithName("PaymentFrequencyName");
    }
}

public class PaymentFrequencySuffixValidator : AbstractValidator<string>
{
    private static int SuffixMaximumLength => 8;

    public PaymentFrequencySuffixValidator()
    {
        
        RuleFor(suffix => suffix)
            .NotEmpty()
            .MaximumLength(SuffixMaximumLength)
            .WithName("Suffix");
    }
}