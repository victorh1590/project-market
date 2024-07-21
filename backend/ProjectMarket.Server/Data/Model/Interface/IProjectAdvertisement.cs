using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Data.Model.Interface;

public interface IProjectAdvertisement
{
    int? ProjectAdvertisementId { get; init; }
    string Title { get;}
    string? Description { get;}
    DateTime OpenedOn { get;}
    DateTime? Deadline { get;}
    PaymentOffer PaymentOffer { get;}
    Customer Customer { get;}
    AdvertisementStatusVO Status { get;}
    List<KnowledgeAreaVo> Subjects { get;}
    List<JobRequirementVo>? Requirements { get;}
}