namespace SupportCentral.Server.Data.Model;

public class ProjectAdvertisement {
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime OpenedOn { get; set; }
    public DateTime Deadline { get; set; }
    public AdvertisementStatus Status { get; set; }
    public PaymentOffer PaymentOffer { get; set; }
    public Costumer Costumer { get; set; }
    public List<KnowledgeArea> Subjects { get; set; }
    public List<JobRequirement>? Requirements { get; set; }
}
