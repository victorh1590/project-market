using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.ValueObjects;

public record AdvertisementStatusRecord(string AdvertisementStatusName);
public struct AdvertisementStatusVo : IEquatable<AdvertisementStatusVo> 
{
    public string AdvertisementStatusName { get; set; }
    
    public AdvertisementStatusVo(string name) {
        AdvertisementStatusName = name;
        this.Validate();
    }
    public AdvertisementStatusVo(AdvertisementStatusRecord record) : this(record.AdvertisementStatusName) {}
   
    public bool Equals(AdvertisementStatusVo other) => AdvertisementStatusName == other.AdvertisementStatusName;
    public override bool Equals(object? obj) => obj is AdvertisementStatusVo other && Equals(other);
    public override int GetHashCode() => AdvertisementStatusName.GetHashCode();
    public static bool operator ==(AdvertisementStatusVo left, AdvertisementStatusVo right) => left.Equals(right);
    public static bool operator !=(AdvertisementStatusVo left, AdvertisementStatusVo right) => !(left == right);
}

public static class AdvertisementStatusExtensions {
    private static AdvertisementStatusValidator Validator { get; } = new();

    public static void Validate(this AdvertisementStatusVo advertisementStatus) => 
        Validator.ValidateAndThrow(advertisementStatus);
}