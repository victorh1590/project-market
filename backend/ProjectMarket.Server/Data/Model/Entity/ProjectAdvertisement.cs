using FluentValidation;
using ProjectMarket.Server.Data.Model.VO;
using ProjectMarket.Server.Data.Validators;
using ProjectMarket.Server.Data.Model.DTO;

namespace ProjectMarket.Server.Data.Model.Entity;

public class ProjectAdvertisement {
    required public int? ProjectAdvertisementId { get; set; }
    required public string Title { get; set; }
    public string? Description { get; set; }
    required public DateTime OpenedOn { get; set; }
    public DateTime? Deadline { get; set; }
    required public PaymentOffer PaymentOffer { get; set; }
    required public Customer Customer { get; set; }
    required public AdvertisementStatusVO Status { get; set; }
    required public List<KnowledgeAreaVO> Subjects { get; set; }
    required public List<JobRequirementVO>? Requirements { get; set; }

    public ProjectAdvertisement(
        int? id, 
        string title, 
        string? description, 
        DateTime? deadline,
        PaymentOffer paymentOffer, 
        Customer customer, 
        AdvertisementStatusVO status, 
        List<KnowledgeAreaVO> subjects,
        List<JobRequirementVO>? requirements) 
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

    public ProjectAdvertisement(ProjectAdvertisementDTO dto) 
    {
        ProjectAdvertisementId = dto.ProjectAdvertisementId;
        Title = dto.Title;
        Description = dto.Description;
        Deadline = dto.Deadline;
        PaymentOffer = dto.PaymentOffer;
        Customer = dto.Customer;
        Status = dto.Status;
        Subjects = dto.Subjects;
        Requirements = dto.Requirements;

        this.Validate();
    }
}

public static class ProjectAdvertisementExtensions {
    public static ProjectAdvertisementValidator Validator { get; private set; } = new();

    public static void Validate(this ProjectAdvertisement projectAdvertisement) => 
        Validator.ValidateAndThrow(projectAdvertisement);
}