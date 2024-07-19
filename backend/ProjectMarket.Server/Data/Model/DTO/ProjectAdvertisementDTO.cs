using ProjectMarket.Server.Data.Model.VO;
using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Data.Model.DTO;

public class ProjectAdvertisementDTO {
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

    public ProjectAdvertisementDTO(
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
    }
}
