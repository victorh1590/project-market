using Dapper;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Db;
using SqlKata.Compilers;

namespace ProjectMarket.Server.Infra.Repository;

public class ProjectAdvertisementRepository(IUnitOfWork unitOfWork, Compiler compiler)
{
    public readonly IUnitOfWork UnitOfWork = unitOfWork;

    public IEnumerable<ProjectAdvertisement> GetAll()
    {
        // TODO Use pagination instead.
        const string projectAdvertisementSql 
            = """
              SELECT ProjectAdvertisementId, Title, Description, OpenedOn, Deadline, 
                   PaymentOfferId, Value, 
                   CustomerId, AdvertisementStatusName 
              FROM "ProjectAdvertisement"
              JOIN "PaymentOffer" USING(PaymentOfferId)
              JOIN "Customer" USING(CustomerId)
              JOIN "AdvertisementStatus" USING(AdvertisementStatusName)
              """;

        const string joinProjectAdvertisementXKnowledgeArea 
            = """
              SELECT KnowledgeAreaName
              FROM "ProjectAdvertisement"
              JOIN "ProjectAdvertisementKnowledgeArea" USING(ProjectAdvertisementId)
              JOIN "KnowledgeArea" USING(KnowledgeAreaName)
              """;
        
        const string joinProjectAdvertisementXJobRequirement 
            = """
              SELECT JobRequirementName
              FROM "ProjectAdvertisement"
              JOIN "ProjectAdvertisementJobRequirement" USING(ProjectAdvertisementId)
              JOIN "JobRequirement" USING(JobRequirementName)
              """;

        const string paymentOfferSql
            = """
              SELECT "PaymentOfferId", "Value", 
                     "PaymentFrequencyName", "Suffix", 
                     "CurrencyName", "Prefix" 
              FROM "PaymentOffer" 
              JOIN "PaymentFrequency" USING ("PaymentFrequencyName") 
              JOIN "Currency" USING ("CurrencyName") 
              WHERE "PaymentOfferId" = @PaymentOfferId
              """;
        
        const string customerSql 
            = """
              SELECT "CustomerId", "Name", "Email", "Password", "RegistrationDate" 
              FROM "Customer" 
              WHERE "CustomerId" = @CustomerId
              """;
        
        const string advertisementStatusSql 
            = """
              SELECT "AdvertisementStatusName" 
              FROM "AdvertisementStatus" 
              WHERE "AdvertisementStatusName" = @AdvertisementStatusName
              """;
        
        var results = UnitOfWork.Connection.Query<dynamic>(projectAdvertisementSql).ToList();
        var projectAdvertisements = new List<ProjectAdvertisement>();
        foreach (var result in results)
        {
            var paymentOfferDynamic = 
                UnitOfWork.Connection.QuerySingle<dynamic>(paymentOfferSql, new { result.PaymentOfferId });
            PaymentFrequencyVo paymentFrequency = new(result.PaymentFrequencyName, result.Suffix);
            CurrencyVo currency = new(result.CurrencyName, result.Prefix);
            PaymentOffer paymentOffer = new(result.PaymentOfferId, result.Value, paymentFrequency, currency);

            var customer = UnitOfWork.Connection.QuerySingle<Customer>(customerSql, new { result.CustomerId });
            
            var advertisementStatusRecord = 
                UnitOfWork.Connection.QuerySingle<AdvertisementStatusRecord>(advertisementStatusSql, new { result.AdvertisementStatusName });
            AdvertisementStatusVo advertisementStatusVo = new(advertisementStatusRecord);
            
            var knowledgeAreasRecords = 
                UnitOfWork.Connection.Query<KnowledgeAreaRecord>(joinProjectAdvertisementXKnowledgeArea).ToList();
            var knowledgeAreas = 
                knowledgeAreasRecords.Select(ka => new KnowledgeAreaVo(ka)).ToList();

            var jobRequirementRecords =
                UnitOfWork.Connection.Query<JobRequirementRecord>(joinProjectAdvertisementXJobRequirement).ToList();
            var jobRequirements = jobRequirementRecords.Count > 0
                ? jobRequirementRecords.Select(jr => new JobRequirementVo(jr)).ToList()
                : null;
            
            projectAdvertisements.Add(new(
                    result.ProjectAdvertisementId,
                    result.Title,
                    result.Description,
                    result.OpenedOn,
                    result.Deadline,
                    paymentOffer,
                    customer,
                    advertisementStatusVo,
                    knowledgeAreas,
                    jobRequirements
                    )
            );
        }

        return projectAdvertisements;
    }

    public ProjectAdvertisement GetProjectAdvertisementById(int id)
    {
        const string projectAdvertisementSql 
            = """
              SELECT ProjectAdvertisementId, Title, Description, OpenedOn, Deadline, 
                   PaymentOfferId, Value, 
                   CustomerId, AdvertisementStatusName 
              FROM "ProjectAdvertisement"
              JOIN "PaymentOffer" USING(PaymentOfferId)
              JOIN "Customer" USING(CustomerId)
              JOIN "AdvertisementStatus" USING(AdvertisementStatusName)
              WHERE "ProjectAdvertisementId" = @ProjectAdvertisementId
              """;

        const string joinProjectAdvertisementXKnowledgeArea 
            = """
              SELECT KnowledgeAreaName
              FROM "ProjectAdvertisement"
              JOIN "ProjectAdvertisementKnowledgeArea" USING(ProjectAdvertisementId)
              JOIN "KnowledgeArea" USING(KnowledgeAreaName)
              """;
        
        const string joinProjectAdvertisementXJobRequirement 
            = """
              SELECT JobRequirementName
              FROM "ProjectAdvertisement"
              JOIN "ProjectAdvertisementJobRequirement" USING(ProjectAdvertisementId)
              JOIN "JobRequirement" USING(JobRequirementName)
              """;

        const string paymentOfferSql
            = """
              SELECT "PaymentOfferId", "Value", 
                     "PaymentFrequencyName", "Suffix", 
                     "CurrencyName", "Prefix" 
              FROM "PaymentOffer" 
              JOIN "PaymentFrequency" USING ("PaymentFrequencyName") 
              JOIN "Currency" USING ("CurrencyName") 
              WHERE "PaymentOfferId" = @PaymentOfferId
              """;
        
        const string customerSql 
            = """
              SELECT "CustomerId", "Name", "Email", "Password", "RegistrationDate" 
              FROM "Customer" 
              WHERE "CustomerId" = @CustomerId
              """;
        
        const string advertisementStatusSql 
            = """
              SELECT "AdvertisementStatusName" 
              FROM "AdvertisementStatus" 
              WHERE "AdvertisementStatusName" = @AdvertisementStatusName
              """;
        try
        {
           var result = UnitOfWork.Connection.QuerySingle<dynamic>(projectAdvertisementSql);
            var paymentOfferDynamic = 
                UnitOfWork.Connection.QuerySingle<dynamic>(paymentOfferSql, new { result.PaymentOfferId });
            PaymentFrequencyVo paymentFrequency = new(result.PaymentFrequencyName, result.Suffix);
            CurrencyVo currency = new(result.CurrencyName, result.Prefix);
            PaymentOffer paymentOffer = new(result.PaymentOfferId, result.Value, paymentFrequency, currency);

            var customer = UnitOfWork.Connection.QuerySingle<Customer>(customerSql, new { result.CustomerId });
            
            var advertisementStatusRecord = 
                UnitOfWork.Connection.QuerySingle<AdvertisementStatusRecord>(advertisementStatusSql, new { result.AdvertisementStatusName });
            AdvertisementStatusVo advertisementStatusVo = new(advertisementStatusRecord);
            
            var knowledgeAreasRecords = 
                UnitOfWork.Connection.Query<KnowledgeAreaRecord>(joinProjectAdvertisementXKnowledgeArea).ToList();
            var knowledgeAreas = 
                knowledgeAreasRecords.Select(ka => new KnowledgeAreaVo(ka)).ToList();

            var jobRequirementRecords =
                UnitOfWork.Connection.Query<JobRequirementRecord>(joinProjectAdvertisementXJobRequirement).ToList();
            var jobRequirements = jobRequirementRecords.Count > 0
                ? jobRequirementRecords.Select(jr => new JobRequirementVo(jr)).ToList()
                : null;

            return new(
                result.ProjectAdvertisementId,
                result.Title,
                result.Description,
                result.OpenedOn,
                result.Deadline,
                paymentOffer,
                customer,
                advertisementStatusVo,
                knowledgeAreas,
                jobRequirements
            );
        }
        catch (Exception)
        {
            throw new ArgumentException($"{nameof(PaymentOffer.PaymentOfferId)} \'{id}\' not found");
        }
    }

    // TODO: All methods below need to be updated.
    public ProjectAdvertisement Insert(ProjectAdvertisement projectAdvertisement)
    {
        const string insert = "INSERT INTO ProjectAdvertisement " +
                              "(Title, Description, OpenedOn, Deadline, PaymentOfferId, CustomerId, AdvertisementStatusName) " +
                              "VALUES (@Title, @Description, @OpenedOn, @Deadline, @PaymentOfferId, @CustomerId, @AdvertisementStatusName)" +
                              "RETURNING ProjectAdvertisementId::INT";

        ProjectAdvertisement inserted = UnitOfWork.Connection.QuerySingle<ProjectAdvertisement>(insert, 
        new {
            Title = projectAdvertisement.Title,
            Description = projectAdvertisement.Description,
            OpenedOn = projectAdvertisement.OpenedOn,
            Deadline = projectAdvertisement.Deadline,
            PaymentOfferId = projectAdvertisement.PaymentOffer.PaymentOfferId,
            CustomerId = projectAdvertisement.Customer.CustomerId,
            AdvertisementStatusName = projectAdvertisement.Status.AdvertisementStatusName,
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
                              "PaymentOfferId = @PaymentOfferId, CustomerId = @CustomerId, AdvertisementStatusName = @AdvertisementStatusName " + 
                              "WHERE ProjectAdvertisementId = @ProjectAdvertisementId";

        ProjectAdvertisement updated = UnitOfWork.Connection.QuerySingle<ProjectAdvertisement>(update, 
        new {
            Title = projectAdvertisement.Title,
            Description = projectAdvertisement.Description,
            OpenedOn = projectAdvertisement.OpenedOn,
            Deadline = projectAdvertisement.Deadline,
            PaymentOfferId = projectAdvertisement.PaymentOffer.PaymentOfferId,
            CustomerId = projectAdvertisement.Customer.CustomerId,
            AdvertisementStatusName = projectAdvertisement.Status.AdvertisementStatusName,
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
        const string sql = "DELETE FROM ProjectAdvertisement CASCADE WHERE ProjectAdvertisementId = @ProjectAdvertisementId " +
                             "RETURNING ProjectAdvertisementId";
        return UnitOfWork.Connection.Execute(sql, projectAdvertisement) == 1;
    }
}
