using ProjectMarket.Server.Data.Model.Dto;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Repository;

namespace ProjectMarket.Server.Data.Model.Factory;

public class ProjectAdvertisementFactory(
    PaymentOfferRepository paymentOfferRepository,
    CustomerRepository customerRepository,
    AdvertisementStatusRepository advertisementStatusRepository,
    KnowledgeAreaRepository knowledgeAreaRepository,
    JobRequirementRepository jobRequirementRepository)
{
    public ProjectAdvertisement CreateProjectAdvertisement(ProjectAdvertisementDto dto)
    {
        PaymentOffer paymentOffer = paymentOfferRepository.GetPaymentOfferById(dto.PaymentOfferId);
        Customer customer = customerRepository.GetCustomerById(dto.CustomerId);
        AdvertisementStatusVo advertisementStatus = 
            advertisementStatusRepository.GetAdvertisementStatusByName(dto.StatusName);
        List<KnowledgeAreaVo> knowledgeAreaList = [];
        dto.SubjectNames.ForEach(subject =>
        {
            knowledgeAreaList.Add(knowledgeAreaRepository.GetKnowledgeAreaByName(subject));
        });
        List<JobRequirementVo>? jobRequirementsList = null;
        if (dto.RequirementNames != null)
        {
            jobRequirementsList = [];
            dto.RequirementNames.ForEach(requirement =>
            {
                jobRequirementsList.Add(jobRequirementRepository.GetJobRequirementByName(requirement));
            });
        }

        return new ProjectAdvertisement(
            dto.ProjectAdvertisementId, 
            dto.Title,
            dto.Description,
            dto.OpenedOn,
            dto.Deadline,
            paymentOffer,
            customer,
            advertisementStatus,
            knowledgeAreaList,
            jobRequirementsList);
    }
}