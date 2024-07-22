using Dapper;
using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Infra.Repository;

public class ProjectAdvertisementRepository(IUnitOfWork unitOfWork)
{
    public readonly IUnitOfWork UnitOfWork = unitOfWork;

    public IEnumerable<ProjectAdvertisement> GetAll()
    {
        // TODO Use pagination instead.
        const string query = "SELECT * FROM ProjectAdvertisement";
        return UnitOfWork.Connection.Query<ProjectAdvertisement>(query);
    }

    public ProjectAdvertisement? GetProjectAdvertisementById(int id)
    {
        const string query = "SELECT * FROM ProjectAdvertisement WHERE ProjectAdvertisementId = @ProjectAdvertisementId";
        return UnitOfWork.Connection.QueryFirstOrDefault<ProjectAdvertisement>(query, new { ProjectAdvertisementId = id })
               ?? throw new ArgumentException($"{nameof(ProjectAdvertisement.ProjectAdvertisementId)} not found");
    }

    public ProjectAdvertisement Insert(ProjectAdvertisement projectAdvertisement)
    {
        const string insert = "INSERT INTO ProjectAdvertisement " +
                              "(Title, Description, OpenedOn, Deadline, PaymentOfferId, CustomerId, StatusId) " +
                              "VALUES (@Title, @Description, @OpenedOn, @Deadline, @PaymentOfferId, @CustomerId, @StatusId)" +
                              "RETURNING ProjectAdvertisementId::INT";

        ProjectAdvertisement inserted = UnitOfWork.Connection.QuerySingle<ProjectAdvertisement>(insert, 
        new {
            Title = projectAdvertisement.Title,
            Description = projectAdvertisement.Description,
            OpenedOn = projectAdvertisement.OpenedOn,
            Deadline = projectAdvertisement.Deadline,
            PaymentOfferId = projectAdvertisement.PaymentOffer.PaymentOfferId,
            CustomerId = projectAdvertisement.Customer.CustomerId,
            StatusId = projectAdvertisement.Status.AdvertisementStatusName,
        });

        foreach(string knowledgeAreaName in projectAdvertisement.Subjects.Select(adv => adv.KnowledgeAreaName).ToArray()) {
            string insertRelation = 
                "INSERT INTO ProjectAdvertisementJobRequirement (ProjectAdvertismentId, JobRequirementName) " +
                "VALUES (@ProjectAdvertisementId, @JobRequirementName)";
            int linesAffected = UnitOfWork.Connection.Execute(insertRelation, 
            new {
                ProjectAdvertisementId = inserted.ProjectAdvertisementId,
                JobRequirementName = knowledgeAreaName
            });
            if(linesAffected == 0) throw new InvalidOperationException("Insert failed: Number of lines inserted was zero");
        }

        return inserted;
    }

    public ProjectAdvertisement Update(ProjectAdvertisement projectAdvertisement)
    {
        const string update = "UPDATE ProjectAdvertisement " + 
                              "SET Title = @Title, Description = @Description, OpenedOn = @OpenedOn, Deadline = @Deadline, " +
                              "PaymentOfferId = @PaymentOfferId, CustomerId = @CustomerId, StatusId = @StatusId " + 
                              "WHERE ProjectAdvertisementId = @ProjectAdvertisementId";

        ProjectAdvertisement updated = UnitOfWork.Connection.QuerySingle<ProjectAdvertisement>(update, 
        new {
            Title = projectAdvertisement.Title,
            Description = projectAdvertisement.Description,
            OpenedOn = projectAdvertisement.OpenedOn,
            Deadline = projectAdvertisement.Deadline,
            PaymentOfferId = projectAdvertisement.PaymentOffer.PaymentOfferId,
            CustomerId = projectAdvertisement.Customer.CustomerId,
            StatusId = projectAdvertisement.Status.AdvertisementStatusName,
        });

        string[] knowledgeAreaNames =  projectAdvertisement.Subjects
            .Select(s => s.KnowledgeAreaName)
            .ToArray();

        const string deleteQuery = "DELETE FROM ProjectAdvertisementJobRequirement " +
                                   "WHERE ProjectAdvertisementId = @ProjectAdvertisementId AND JobRequirementName NOT IN @KnowledgeAreaNames";
        UnitOfWork.Connection.Execute(deleteQuery, new {
            ProjectAdvertisementId = projectAdvertisement.ProjectAdvertisementId,
            KnowledgeAreaNames = knowledgeAreaNames
        });

        foreach (string knowledgeAreaName in knowledgeAreaNames)
        {
            const string insertRelation = "INSERT INTO ProjectAdvertisementJobRequirement (ProjectAdvertisementId, JobRequirementName) " +
                                          "VALUES (@ProjectAdvertisementId, @JobRequirementName)";
            UnitOfWork.Connection.Execute(insertRelation, new {
                ProjectAdvertisementId = projectAdvertisement.ProjectAdvertisementId,
                JobRequirementName = knowledgeAreaName
            });
        }

        return updated;
    }

    public bool Delete(ProjectAdvertisement projectAdvertisement)
    {
        const string query = "DELETE FROM ProjectAdvertisement CASCADE WHERE ProjectAdvertisementId = @ProjectAdvertisementId " +
                             "RETURNING ProjectAdvertisementId";
        return UnitOfWork.Connection.Execute(query, projectAdvertisement) == 1;
    }
}
