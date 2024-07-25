using ProjectMarket.Server.Data.Model.Dto;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Db;
using ProjectMarket.Server.Infra.Repository;

namespace ProjectMarket.Server.Data.Model.Factory;

public class ProjectAdvertisementFactory(IUnitOfWork uow)
{
    private readonly PaymentOfferRepository _paymentOfferRepository = new(uow);
    private readonly CustomerRepository _customerRepository = new(uow);
    private readonly AdvertisementStatusRepository _advertisementStatusRepository = new(uow);
    private readonly KnowledgeAreaRepository _knowledgeAreaRepository = new(uow);
    private readonly JobRequirementRepository _jobRequirementRepository = new(uow);
    
    public ProjectAdvertisement CreateProjectAdvertisement(ProjectAdvertisementDto dto)
    {
        PaymentOffer paymentOffer = _paymentOfferRepository.GetPaymentOfferById(dto.PaymentOfferId);
        Customer customer = _customerRepository.GetCustomerById(dto.CustomerId);
        AdvertisementStatusVo advertisementStatus = 
            _advertisementStatusRepository.GetAdvertisementStatusByName(dto.StatusName);
        List<KnowledgeAreaVo> knowledgeAreaList = [];
        dto.SubjectNames.ForEach(subject =>
        {
            knowledgeAreaList.Add(_knowledgeAreaRepository.GetKnowledgeAreaByName(subject));
        });
        List<JobRequirementVo>? jobRequirementsList = null;
        if (dto.RequirementNames != null)
        {
            jobRequirementsList = [];
            dto.RequirementNames.ForEach(requirement =>
            {
                jobRequirementsList.Add(_jobRequirementRepository.GetJobRequirementByName(requirement));
            });
        }

        return new ProjectAdvertisement(
            dto.ProjectAdvertisementId, 
            dto.Title,
            dto.Description,
            dto.Deadline,
            paymentOffer,
            customer,
            advertisementStatus,
            knowledgeAreaList,
            jobRequirementsList);
    }
}