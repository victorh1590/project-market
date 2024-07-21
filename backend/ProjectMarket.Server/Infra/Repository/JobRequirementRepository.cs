using Dapper;
using ProjectMarket.Server.Data.Model.ValueObjects;

namespace ProjectMarket.Server.Infra.Repository;

public class JobRequirementRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<JobRequirementVo> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT JobRequirementName FROM JobRequirement";
        return _uow.Connection.Query<JobRequirementVo>(query);
    }

    public JobRequirementVo? GetByJobRequirementName(string name)
    {
        string query = "SELECT JobRequirementName FROM JobRequirement WHERE JobRequirementName = @JobRequirementName";
        return _uow.Connection.QueryFirstOrDefault<JobRequirementVo>(query, new { JobRequirementName = name });
    }

    public void Insert(JobRequirementVo JobRequirement)
    {
        string query = "INSERT INTO JobRequirement (JobRequirementName) VALUES (@JobRequirementName)";
        _uow.Connection.Execute(query, JobRequirement);
    }

    public void Update(JobRequirementVo JobRequirement)
    {
        string query = "UPDATE JobRequirement SET JobRequirementName = @JobRequirementName WHERE JobRequirementName = @JobRequirementName";
        _uow.Connection.Execute(query, JobRequirement);
    }

    public void Delete(string name)
    {
        string query = "DELETE CASCADE FROM JobRequirement WHERE JobRequirementName = @JobRequirementName";
        _uow.Connection.Execute(query, new { JobRequirementName = name });
    }
}
