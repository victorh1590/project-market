using FluentValidation;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Validators;

public class KnowledgeAreaValidator : AbstractValidator<KnowledgeAreaVO>
{
    public KnowledgeAreaValidator()
    {
        const int nameMaximumLength = 64;

        RuleFor(knowledgeArea => knowledgeArea.KnowledgeAreaName)
            .NotEmpty()
            .MaximumLength(nameMaximumLength)
            .WithName("KnowledgeAreaName");
    }
}