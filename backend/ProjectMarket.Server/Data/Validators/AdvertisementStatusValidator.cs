using FluentValidation;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Validators;

public class AdvertisementStatusValidator : AbstractValidator<AdvertisementStatusVO>
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
            .NotEmpty()
            .MaximumLength(NameMaximumLength)
            .WithName("AdvertisementStatusName");
    }
}