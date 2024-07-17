using Dapper;
using Microsoft.Data.SqlClient;
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
        string insert = 
            "INSERT INTO ProjectAdvertisement " +
            "(Title, Description, OpenedOn, Deadline, PaymentOfferId, CustomerId, StatusId) " +
            "VALUES " +
            "(@Title, @Description, @OpenedOn, @Deadline, @PaymentOfferId, @CustomerId, @StatusId)" +
            "RETURNING ProjectAdvertisementId::INT";

        int id = _uow.Connection.QuerySingle<int>(insert, 
        new {
            Title = ProjectAdvertisement.Title,
            Description = ProjectAdvertisement.Description,
            OpenedOn = ProjectAdvertisement.OpenedOn,
            Deadline = ProjectAdvertisement.Deadline,
            PaymentOfferId = ProjectAdvertisement.PaymentOffer.PaymentOfferId,
            CustomerId = ProjectAdvertisement.Customer.CustomerId,
            StatusId = ProjectAdvertisement.Status.AdvertisementStatusName,
        });

        foreach(string knowledgeAreaName in ProjectAdvertisement.Subjects.Select(adv => adv.KnowledgeAreaName).ToArray()) {
            string insertRelation = 
                "INSERT INTO ProjectAdvertisementJobRequirement " +
                "(ProjectAdvertismentId, JobRequirementName) " +
                "VALUES " +
                "(@ProjectAdvertisementId, @JobRequirementName)";
            int linesAffected = _uow.Connection.Execute(insertRelation, 
            new {
                ProjectAdvertisementId = id,
                JobRequirementName = knowledgeAreaName
            });
            if(linesAffected == 0) throw new InvalidOperationException("Insert failed: Number of lines inserted was zero");
        }
    }

    public void Update(ProjectAdvertisement ProjectAdvertisement)
    {
        string update = 
            "UPDATE ProjectAdvertisement " + 
            "SET Title = @Title, Description = @Description, OpenedOn = @OpenedOn, Deadline = @Deadline, " +
            "PaymentOfferId = @PaymentOfferId, CustomerId = @CustomerId, StatusId = @StatusId" + 
            "WHERE ProjectAdvertisementId = @ProjectAdvertisementId";

            _uow.Connection.Execute(update, 
            new {
                Title = ProjectAdvertisement.Title,
                Description = ProjectAdvertisement.Description,
                OpenedOn = ProjectAdvertisement.OpenedOn,
                Deadline = ProjectAdvertisement.Deadline,
                PaymentOfferId = ProjectAdvertisement.PaymentOffer.PaymentOfferId,
                CustomerId = ProjectAdvertisement.Customer.CustomerId,
                StatusId = ProjectAdvertisement.Status.AdvertisementStatusName,
            });

        string[] knowledgeAreaNames =  ProjectAdvertisement.Subjects
            .Select(s => s.KnowledgeAreaName)
            .ToArray();

        string deleteQuery = 
            "DELETE FROM ProjectAdvertisementJobRequirement " +
            "WHERE ProjectAdvertisementId = @ProjectAdvertisementId AND JobRequirementName NOT IN @KnowledgeAreaNames";
        _uow.Connection.Execute(deleteQuery, new {
            ProjectAdvertisementId = ProjectAdvertisement.ProjectAdvertisementId,
            KnowledgeAreaNames = knowledgeAreaNames
        });

        foreach (string knowledgeAreaName in knowledgeAreaNames)
        {
            string insertRelation = 
                "INSERT INTO ProjectAdvertisementJobRequirement " +
                "(ProjectAdvertisementId, JobRequirementName) " +
                "VALUES " +
                "(@ProjectAdvertisementId, @JobRequirementName)";
            _uow.Connection.Execute(insertRelation, new {
                ProjectAdvertisementId = ProjectAdvertisement.ProjectAdvertisementId,
                JobRequirementName = knowledgeAreaName
            });
        }
    }

    public void Delete(ProjectAdvertisement ProjectAdvertisement)
    {
        string query = "DELETE CASCADE FROM ProjectAdvertisement WHERE ProjectAdvertisementId = @ProjectAdvertisementId";
        _uow.Connection.Execute(query, ProjectAdvertisement);
    }
}
