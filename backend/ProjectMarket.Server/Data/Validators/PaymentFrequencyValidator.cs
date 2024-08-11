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
    private static int NameMaximumLength => 32;
    
    public PaymentFrequencyNameValidator()
    {

        RuleFor(paymentFrequencyName => paymentFrequencyName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(NameMaximumLength)
            .WithName("PaymentFrequencyName");
    }
}

public class PaymentFrequencySuffixValidator : AbstractValidator<string>
{
    private static int SuffixMaximumLength => 32;

    public PaymentFrequencySuffixValidator()
    {
        
        RuleFor(suffix => suffix)
            .NotNull()
            .NotEmpty()
            .MaximumLength(SuffixMaximumLength)
            .WithName("Suffix");
    }
}