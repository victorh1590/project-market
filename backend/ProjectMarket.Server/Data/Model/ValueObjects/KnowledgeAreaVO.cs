using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public struct KnowledgeAreaVo {
    public required string KnowledgeAreaName { get; set; }

    public KnowledgeAreaVo(string name) {
        KnowledgeAreaName = name;

        this.Validate();
    }
}

public static class KnowledgeAreaExtensions {
    private static KnowledgeAreaValidator Validator { get; } = new();

    public static void Validate(this KnowledgeAreaVo knowledgeArea) => 
        Validator.ValidateAndThrow(knowledgeArea);
}