using Dapper;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Db;
using SqlKata.Compilers;

namespace ProjectMarket.Server.Infra.Repository;

public class JobRequirementRepository(IUnitOfWork unitOfWork, Compiler compiler)
{
    public readonly IUnitOfWork UnitOfWork = unitOfWork;
    
    public IEnumerable<JobRequirementVo> GetAll()
    {
        // TODO Use pagination instead.
        const string sql = """
                           SELECT "JobRequirementName" 
                           FROM "JobRequirement"
                           """;
        return UnitOfWork.Connection.Query<JobRequirementVo>(sql);
    }

    public JobRequirementVo GetJobRequirementByName(string name)
    {
        const string sql = """
                           SELECT "JobRequirementName" 
                           FROM "JobRequirement" 
                           WHERE "JobRequirementName" = @JobRequirementName
                           """;
        try
        {
            var record = UnitOfWork.Connection.QuerySingle<JobRequirementRecord>(sql, new { JobRequirementName = name });
            JobRequirementVo result = new(record);
            return result;
        }
        catch (Exception)
        {
            throw new ArgumentException($"{nameof(JobRequirementVo.JobRequirementName)} \'{name}\' not found");
        }
    }

    public JobRequirementVo Insert(JobRequirementVo jobRequirement)
    {
        const string sql = """
                           INSERT INTO "JobRequirement" ("JobRequirementName") 
                           VALUES (@JobRequirementName) 
                           RETURNING "JobRequirementName"
                           """;
       return UnitOfWork.Connection.QuerySingle<JobRequirementVo>(sql, jobRequirement);
    }

    public bool Update(string name, JobRequirementVo jobRequirement)
    {
        const string sql = """
                           UPDATE "JobRequirement" 
                           SET "JobRequirementName" = @JobRequirementName 
                           WHERE "JobRequirementName" = @JobRequirementNameToUpdate 
                           RETURNING true
                           """;
        return UnitOfWork.Connection.QuerySingle<bool>(sql, new
        {
            JobRequirementNameToUpdate = name,
            JobRequirementName = jobRequirement.JobRequirementName
        });
    }

    public bool Delete(string name)
    {
        const string sql = """
                           DELETE FROM "JobRequirement" CASCADE 
                           WHERE "JobRequirementName" = @JobRequirementName 
                           RETURNING true
                           """;
        return UnitOfWork.Connection.QuerySingle<bool>(sql, new { JobRequirementName = name });
    }
}