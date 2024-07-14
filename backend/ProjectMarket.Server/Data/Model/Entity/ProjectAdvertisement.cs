using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Model.Entity;

public class ProjectAdvertisement {
    required public int ProjectAdvertisementId { get; set; }
    required public string Title { get; set; }
    public string? Description { get; set; }
    required public DateTime OpenedOn { get; set; }
    public DateTime? Deadline { get; set; }
    required public PaymentOffer PaymentOffer { get; set; }
    required public Costumer Costumer { get; set; }
    required public AdvertisementStatus Status { get; set; }
    required public List<KnowledgeArea> Subjects { get; set; }
    public List<JobRequirement>? Requirements { get; set; }
}
