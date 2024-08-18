using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public record JobRequirementRecord(string JobRequirementName);
public struct JobRequirementVo : IEquatable<JobRequirementVo> 
{
    public string JobRequirementName { get; init; }

    public JobRequirementVo(string name) {
        JobRequirementName = name;
        this.Validate();
    }
    public JobRequirementVo(JobRequirementRecord record) : this(record.JobRequirementName) {}

    public bool Equals(JobRequirementVo other) => JobRequirementName == other.JobRequirementName;
    public override bool Equals(object? obj) => obj is JobRequirementVo other && Equals(other);
    public override int GetHashCode() => JobRequirementName.GetHashCode();
    public static bool operator ==(JobRequirementVo left, JobRequirementVo right) => left.Equals(right);
    public static bool operator !=(JobRequirementVo left, JobRequirementVo right) => !left.Equals(right);
}

public static class JobRequirementExtensions {
    private static JobRequirementValidator Validator { get; } = new();

    public static void Validate(this JobRequirementVo jobRequirement) => 
        Validator.ValidateAndThrow(jobRequirement);
}