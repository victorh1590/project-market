using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.VO;

public struct AdvertisementStatusVO {
    required public string AdvertisementStatusName { get; set; }

    public AdvertisementStatusVO(string name) {
        AdvertisementStatusName = name;

        var validator = new AdvertisementStatusValidator();
        validator.ValidateAndThrow(this);
    }
}
