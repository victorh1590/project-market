using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public record KnowledgeAreaRecord(string KnowledgeAreaName);
public struct KnowledgeAreaVo : IEquatable<KnowledgeAreaVo>
{
    public string KnowledgeAreaName { get; set; }

    public KnowledgeAreaVo(string name) {
        KnowledgeAreaName = name;

        this.Validate();
    }
    public KnowledgeAreaVo(KnowledgeAreaRecord record) : this(record.KnowledgeAreaName) {}

    public bool Equals(KnowledgeAreaVo other) => KnowledgeAreaName == other.KnowledgeAreaName;
    public override bool Equals(object? obj) => obj is KnowledgeAreaVo other && Equals(other);
    public override int GetHashCode() => KnowledgeAreaName.GetHashCode();
    public static bool operator ==(KnowledgeAreaVo left, KnowledgeAreaVo right) => left.Equals(right);
    public static bool operator !=(KnowledgeAreaVo left, KnowledgeAreaVo right) => !left.Equals(right);
}

public static class KnowledgeAreaExtensions {
    private static KnowledgeAreaValidator Validator { get; } = new();

    public static void Validate(this KnowledgeAreaVo knowledgeArea) => 
        Validator.ValidateAndThrow(knowledgeArea);
}