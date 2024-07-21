using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public struct JobRequirementVo {
    public required string JobRequirementName { get; init; }

    public JobRequirementVo(string name) {
        JobRequirementName = name;

        this.Validate();
    }
}

public static class JobRequirementExtensions {
    private static JobRequirementValidator Validator { get; } = new();

    public static void Validate(this JobRequirementVo jobRequirement) => 
        Validator.ValidateAndThrow(jobRequirement);
}