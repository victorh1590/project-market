using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.VO;

public struct JobRequirementVO {
    required public string JobRequirementName { get; set; }

    public JobRequirementVO(string name) {
        JobRequirementName = name;

        var validator = new JobRequirementValidator();
        validator.ValidateAndThrow(this);
    }
}
