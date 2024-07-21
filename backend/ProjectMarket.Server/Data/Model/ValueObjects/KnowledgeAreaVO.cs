using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public struct KnowledgeAreaVo {
    public required string KnowledgeAreaName { get; set; }

    public KnowledgeAreaVo(string name) {
        KnowledgeAreaName = name;

        var validator = new KnowledgeAreaValidator();
        validator.ValidateAndThrow(this);
    }
}