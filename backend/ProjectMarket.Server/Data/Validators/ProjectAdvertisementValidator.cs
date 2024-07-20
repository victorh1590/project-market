using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using ProjectMarket.Server.Data.Model.Dto;
using ProjectMarket.Server.Data.Model.Interface;

namespace ProjectMarket.Server.Data.Validators;

public class ProjectAdvertisementValidator : AbstractValidator<IProjectAdvertisement>
{
    public ProjectAdvertisementValidator()
    {
        DateTime firstValidDate = new(2020, 01, 01, 00, 00, 00);
        DateTime lastValidDate = new(2099, 01, 01, 00, 00, 00);

        RuleFor(projectAdvertisement => projectAdvertisement.ProjectAdvertisementId)
            .GreaterThan(0)
            .WithName("ProjectAdvertisementId")
            .Unless(projectAdvertisement => projectAdvertisement is ProjectAdvertisementDto);
        RuleFor(projectAdvertisement => projectAdvertisement.Title)
            .NotEmpty()
            .MaximumLength(128)
            .WithName("Title");
        RuleFor(projectAdvertisement => projectAdvertisement.Description)
            .MaximumLength(512)
            .WithName("Description")
            .Unless(projectAdvertisement => string.IsNullOrEmpty(projectAdvertisement.Description));
        RuleFor(projectAdvertisement => projectAdvertisement.OpenedOn)
            .InclusiveBetween(firstValidDate, DateTime.Now)
            .LessThan(projectAdvertisement => projectAdvertisement.Deadline)
            .WithName("OpenedOn");
        RuleFor(projectAdvertisement => projectAdvertisement.Deadline)
            .InclusiveBetween(DateTime.Now, lastValidDate)
            .GreaterThan(projectAdvertisement => projectAdvertisement.OpenedOn)
            .WithName("Deadline")
            .Unless(projectAdvertisement => projectAdvertisement.Deadline == null);
        RuleFor(projectAdvertisement => projectAdvertisement.PaymentOffer)
            .NotNull()
            .SetValidator(new PaymentOfferValidator())
            .WithName("PaymentOffer");
        RuleFor(projectAdvertisement => projectAdvertisement.Customer)
            .NotNull()
            .SetValidator(new CustomerValidator())
            .WithName("Customer");
        RuleFor(projectAdvertisement => projectAdvertisement.Subjects)
            .NotNull()
            .NotEmpty()
            .WithName("Subjects");
        RuleForEach(projectAdvertisement => projectAdvertisement.Subjects)
            .SetValidator(new KnowledgeAreaValidator())
            .WithName("Subject#{CollectionIndex}")
            .Unless(projectAdvertisement => projectAdvertisement.Subjects.IsNullOrEmpty());
        RuleFor(projectAdvertisement => projectAdvertisement.Requirements)
            .NotEmpty()
            .WithName("Requirements")
            .Unless(projectAdvertisement => projectAdvertisement.Requirements == null);
        RuleForEach(projectAdvertisement => projectAdvertisement.Requirements)
            .SetValidator(new JobRequirementValidator())
            .WithName("Requirements#{CollectionIndex}")
            .Unless(projectAdvertisement => projectAdvertisement.Requirements.IsNullOrEmpty());
    }
}