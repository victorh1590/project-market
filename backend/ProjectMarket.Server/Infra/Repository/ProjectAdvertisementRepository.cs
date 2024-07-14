using Dapper;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Infra.Repository;

public class ProjectAdvertisementRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<ProjectAdvertisement> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT * FROM ProjectAdvertisement";
        return _uow.Connection.Query<ProjectAdvertisement>(query);
    }

    public ProjectAdvertisement? GetByProjectAdvertisementId(int id)
    {
        string query = "SELECT * FROM ProjectAdvertisement WHERE ProjectAdvertisementId = @ProjectAdvertisementId";
        return _uow.Connection.QueryFirstOrDefault<ProjectAdvertisement>(query, new { ProjectAdvertisementId = id });
    }

    public void Insert(ProjectAdvertisement ProjectAdvertisement)
    {
        string query = 
            "INSERT INTO ProjectAdvertisement " +
            "(Title, Description, OpenedOn, Deadline, PaymentOfferId, CostumerId, StatusId, SubjectsId) " +
            "VALUES " +
            "(@Title, @Description, @OpenedOn, @Deadline, @PaymentOfferId, @CostumerId, @StatusId, @SubjectsId)";

        foreach(int knowledgeAreaId in ProjectAdvertisement.Subjects.Select(s => s.KnowledgeAreaId).ToArray()) {
        _uow.Connection.Execute(query, 
            new {
                Title = ProjectAdvertisement.Title,
                Description = ProjectAdvertisement.Description,
                OpenedOn = ProjectAdvertisement.OpenedOn,
                Deadline = ProjectAdvertisement.Deadline,
                PaymentOfferId = ProjectAdvertisement.PaymentOffer.PaymentOfferId,
                CostumerId = ProjectAdvertisement.Costumer.CostumerId,
                StatusId = ProjectAdvertisement.Status.AdvertisementStatusId,
                SubjectId = knowledgeAreaId
            });
        }
    }

    public void Update(ProjectAdvertisement ProjectAdvertisement)
    {
        string query = 
            "UPDATE ProjectAdvertisement " + 
            "SET Title = @Title, Description = @Description, OpenedOn = @OpenedOn, Deadline = @Deadline, " +
            "PaymentOfferId = @PaymentOfferId, CostumerId = @CostumerId, StatusId = @StatusId, SubjectId = @SubjectId " + 
            "WHERE ProjectAdvertisementId = @ProjectAdvertisementId";

        foreach(int knowledgeAreaId in ProjectAdvertisement.Subjects.Select(s => s.KnowledgeAreaId).ToArray()) {
            _uow.Connection.Execute(query, 
            new {
                Title = ProjectAdvertisement.Title,
                Description = ProjectAdvertisement.Description,
                OpenedOn = ProjectAdvertisement.OpenedOn,
                Deadline = ProjectAdvertisement.Deadline,
                PaymentOfferId = ProjectAdvertisement.PaymentOffer.PaymentOfferId,
                CostumerId = ProjectAdvertisement.Costumer.CostumerId,
                StatusId = ProjectAdvertisement.Status.AdvertisementStatusId,
                SubjectId = knowledgeAreaId
            });
        }
    }

    public void Delete(ProjectAdvertisement ProjectAdvertisement)
    {
        string query = "DELETE CASCADE FROM ProjectAdvertisement WHERE ProjectAdvertisementId = @ProjectAdvertisementId";
        _uow.Connection.Execute(query, ProjectAdvertisement);
    }
}
