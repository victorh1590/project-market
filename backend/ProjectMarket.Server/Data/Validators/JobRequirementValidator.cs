using FluentValidation;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Validators;

public class JobRequirementValidator : AbstractValidator<JobRequirementVO>
{
    public JobRequirementValidator()
    {
        const int nameMaximumLength = 64;

        RuleFor(jobRequirement => jobRequirement.JobRequirementName)
            .NotEmpty()
            .MaximumLength(nameMaximumLength)
            .WithName("JobRequirementName");
    }
}