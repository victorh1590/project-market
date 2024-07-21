using FluentValidation;
using ProjectMarket.Server.Data.Model.Dto;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Data.Model.VO;
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
    public AdvertisementStatusVO Status { get; set; }
    public List<KnowledgeAreaVo> Subjects { get; set; }
    public List<JobRequirementVo>? Requirements { get; set; }

    public ProjectAdvertisement(
        int? id, 
        string title, 
        string? description, 
        DateTime? deadline,
        PaymentOffer paymentOffer, 
        Customer customer, 
        AdvertisementStatusVO status, 
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

        var validator = new ProjectAdvertisementValidator();
        validator.ValidateAndThrow(this);
    }

    // public ProjectAdvertisement(ProjectAdvertisementDto dto) 
    // {
    //     ProjectAdvertisementId = dto.ProjectAdvertisementId;
    //     Title = dto.Title;
    //     Description = dto.Description;
    //     Deadline = dto.Deadline;
    //     PaymentOffer = dto.PaymentOffer;
    //     Customer = dto.Customer;
    //     Status = dto.Status;
    //     Subjects = dto.Subjects;
    //     Requirements = dto.Requirements;
    //
    //     this.Validate();
    // }
}

public static class ProjectAdvertisementExtensions {
    private static ProjectAdvertisementValidator Validator { get; } = new();

    public static void Validate(this ProjectAdvertisement projectAdvertisement) => 
        Validator.ValidateAndThrow(projectAdvertisement);
}