using FluentValidation;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.Entity;

public class ProjectAdvertisement
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
        int? id, 
        string title, 
        string? description, 
        DateTime? deadline,
        PaymentOffer paymentOffer, 
        Customer customer, 
        AdvertisementStatusVo status, 
        List<KnowledgeAreaVo> subjects,
        List<JobRequirementVo>? requirements) 
    {
        ProjectAdvertisementId = id;
        Title = title;
        Description = description;
        Deadline = deadline;
        PaymentOffer = paymentOffer;
        Customer = customer;
        Status = status;
        Subjects = subjects;
        Requirements = requirements;

        this.Validate();
    }
}

public static class ProjectAdvertisementExtensions {
    private static ProjectAdvertisementValidator Validator { get; } = new();

    public static void Validate(this ProjectAdvertisement projectAdvertisement) => 
        Validator.ValidateAndThrow(projectAdvertisement);
}