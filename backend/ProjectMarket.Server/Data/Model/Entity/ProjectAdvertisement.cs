using FluentValidation;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.Entity;

public class ProjectAdvertisement : IEquatable<ProjectAdvertisement>
{
    public int? ProjectAdvertisementId { get; init; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime OpenedOn { get; set; }
    public DateTime? Deadline { get; set; }
    public PaymentOffer PaymentOffer { get; set; }
    public Customer Customer { get; set; }
    public AdvertisementStatusVo Status { get; set; }
    public List<KnowledgeAreaVo> Subjects { get; set; }
    public List<JobRequirementVo>? Requirements { get; set; }

    public ProjectAdvertisement(
        int? projectAdvertisementId, 
        string title, 
        string? description, 
        DateTime? openedOn,
        DateTime? deadline,
        PaymentOffer paymentOffer, 
        Customer customer, 
        AdvertisementStatusVo status, 
        List<KnowledgeAreaVo> subjects,
        List<JobRequirementVo>? requirements) 
    {
        ProjectAdvertisementId = projectAdvertisementId;
        Title = title;
        Description = description;
        OpenedOn = openedOn ?? DateTime.Now;
        Deadline = deadline;
        PaymentOffer = paymentOffer;
        Customer = customer;
        Status = status;
        Subjects = subjects;
        Requirements = requirements;

        this.Validate();
    }
    
    public override bool Equals(object? obj)
        => ReferenceEquals(this, obj) || (
            obj is ProjectAdvertisement other &&
            ProjectAdvertisementId == other.ProjectAdvertisementId && 
            Title == other.Title && 
            Description == other.Description && 
            OpenedOn == other.OpenedOn && 
            Nullable.Equals(Deadline, other.Deadline) && 
            PaymentOffer.Equals(other.PaymentOffer) && 
            Customer.Equals(other.Customer) && 
            Status == other.Status && 
            Subjects.Equals(other.Subjects) && 
            (Requirements?.Equals(other.Requirements) ?? other.Requirements == null));
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(ProjectAdvertisementId);
        hashCode.Add(Title);
        hashCode.Add(Description);
        hashCode.Add(OpenedOn);
        hashCode.Add(Deadline);
        hashCode.Add(PaymentOffer);
        hashCode.Add(Customer);
        hashCode.Add(Status);
        hashCode.Add(Subjects);
        hashCode.Add(Requirements);
        return hashCode.ToHashCode();
    }
    public bool Equals(ProjectAdvertisement? other)
        => ReferenceEquals(this, other) || (
            other != null &&
            ProjectAdvertisementId == other.ProjectAdvertisementId && 
            Title == other.Title && 
            Description == other.Description && 
            OpenedOn == other.OpenedOn && 
            Nullable.Equals(Deadline, other.Deadline) && 
            PaymentOffer.Equals(other.PaymentOffer) && 
            Customer.Equals(other.Customer) && 
            Status == other.Status && 
            Subjects.Equals(other.Subjects) && 
            (Requirements?.Equals(other.Requirements) ?? other.Requirements == null));
}

public static class ProjectAdvertisementExtensions {
    private static ProjectAdvertisementValidator Validator { get; } = new();

    public static void Validate(this ProjectAdvertisement projectAdvertisement) => 
        Validator.ValidateAndThrow(projectAdvertisement);
}