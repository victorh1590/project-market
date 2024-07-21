using ProjectMarket.Server.Data.Model.VO;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.Interface;

namespace ProjectMarket.Server.Data.Model.Dto;

public class ProjectAdvertisementDto(
    int? id,
    string title,
    string? description,
    DateTime? deadline,
    PaymentOffer paymentOffer,
    Customer customer,
    AdvertisementStatusVO status,
    List<KnowledgeAreaVO> subjects,
    List<JobRequirementVO>? requirements,
    DateTime openedOn) : IProjectAdvertisement
{
    public int? ProjectAdvertisementId { get; init; } = id;
    public string Title { get;} = title;
    public string? Description { get;} = description;
    public DateTime OpenedOn { get;} = openedOn;
    public DateTime? Deadline { get;} = deadline;
    public PaymentOffer PaymentOffer { get;} = paymentOffer;
    public Customer Customer { get;} = customer;
    public AdvertisementStatusVO Status { get;} = status;
    public List<KnowledgeAreaVO> Subjects { get;} = subjects;
    public List<JobRequirementVO>? Requirements { get;} = requirements;
}
