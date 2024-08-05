using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public record KnowledgeAreaRecord(string KnowledgeAreaName);

public struct KnowledgeAreaVo {
    public required string KnowledgeAreaName { get; set; }

    public KnowledgeAreaVo(string name) {
        KnowledgeAreaName = name;

        this.Validate();
    }

    public KnowledgeAreaVo(KnowledgeAreaRecord record) : this(record.KnowledgeAreaName) {}
}

public static class KnowledgeAreaExtensions {
    private static KnowledgeAreaValidator Validator { get; } = new();

    public static void Validate(this KnowledgeAreaVo knowledgeArea) => 
        Validator.ValidateAndThrow(knowledgeArea);
}