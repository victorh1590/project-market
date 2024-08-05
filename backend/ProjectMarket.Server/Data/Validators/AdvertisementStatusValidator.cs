using FluentValidation;
using ProjectMarket.Server.Data.Model.ValueObjects;

namespace ProjectMarket.Server.Data.Validators;

public class AdvertisementStatusValidator : AbstractValidator<AdvertisementStatusVo>
{
    public AdvertisementStatusValidator()
    {
        RuleFor(advertisementStatus => advertisementStatus.AdvertisementStatusName)
            .SetValidator(new AdvertisementStatusNameValidator())
            .WithName("AdvertisementStatusName");
    }
}

public class AdvertisementStatusNameValidator : AbstractValidator<string>
{
    private static int NameMaximumLength => 64;

    public AdvertisementStatusNameValidator()
    {

        RuleFor(advertisementStatusName => advertisementStatusName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(NameMaximumLength)
            .WithName("AdvertisementStatusName");
    }
}