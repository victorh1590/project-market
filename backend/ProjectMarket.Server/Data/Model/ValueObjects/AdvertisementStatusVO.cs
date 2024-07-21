using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public struct AdvertisementStatusVo {
    public required string AdvertisementStatusName { get; set; }

    public AdvertisementStatusVo(string name) {
        AdvertisementStatusName = name;

        this.Validate();
    }
}

public static class AdvertisementStatusExtensions {
    private static AdvertisementStatusValidator Validator { get; } = new();

    public static void Validate(this AdvertisementStatusVo advertisementStatus) => 
        Validator.ValidateAndThrow(advertisementStatus);
}