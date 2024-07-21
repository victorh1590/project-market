using FluentValidation;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.ValueObjects;

namespace ProjectMarket.Server.Data.Validators;

public class KnowledgeAreaValidator : AbstractValidator<KnowledgeAreaVo>
{
    public KnowledgeAreaValidator()
    {
        RuleFor(knowledgeArea => knowledgeArea.KnowledgeAreaName)
            .SetValidator(new KnowledgeAreaNameValidator())
            .WithName("KnowledgeAreaName");
    }
}

public class KnowledgeAreaNameValidator : AbstractValidator<string>
{
    private static int NameMaximumLength => 64;

    public KnowledgeAreaNameValidator()
    {

        RuleFor(knowledgeAreaName => knowledgeAreaName)
            .NotEmpty()
            .MaximumLength(NameMaximumLength)
            .WithName("KnowledgeAreaName");
    }
}