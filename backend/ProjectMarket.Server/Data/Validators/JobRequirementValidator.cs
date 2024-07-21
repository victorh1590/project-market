using FluentValidation;
using ProjectMarket.Server.Data.Model.ValueObjects;

namespace ProjectMarket.Server.Data.Validators;

public class JobRequirementValidator : AbstractValidator<JobRequirementVo>
{
    public JobRequirementValidator()
    {
        const int nameMaximumLength = 64;

        RuleFor(jobRequirement => jobRequirement.JobRequirementName)
            .SetValidator(new JobRequirementNameValidator())
            .WithName("JobRequirementName");
    }
}

public class JobRequirementNameValidator : AbstractValidator<string>
{
    private static int NameMaximumLength => 64;

    public JobRequirementNameValidator()
    {

        RuleFor(jobRequirementName => jobRequirementName)
            .NotEmpty()
            .MaximumLength(NameMaximumLength)
            .WithName("JobRequirementName");
    }
}