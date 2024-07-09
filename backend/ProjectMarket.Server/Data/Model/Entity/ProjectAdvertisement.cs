namespace ProjectMarket.Server.Data.Model;

public class ProjectAdvertisement {
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime OpenedOn { get; set; }
    public DateTime Deadline { get; set; }
    public PaymentOffer PaymentOffer { get; set; }
    public Costumer Costumer { get; set; }
    public AdvertisementStatusVO Status { get; set; }
    public List<KnowledgeAreaVO> Subjects { get; set; }
    public List<JobRequirementVO>? Requirements { get; set; }
}
