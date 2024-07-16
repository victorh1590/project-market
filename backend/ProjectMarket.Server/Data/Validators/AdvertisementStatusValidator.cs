using FluentValidation;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Validators;

public class AdvertisementStatusValidator : AbstractValidator<AdvertisementStatusVO>
{
    public AdvertisementStatusValidator()
    {
        const int nameMaximumLength = 64;

        RuleFor(advertisementStatus => advertisementStatus.AdvertisementStatusName)
            .NotEmpty()
            .MaximumLength(nameMaximumLength)
            .WithName("AdvertisementStatusName");
    }
}