using Dapper;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Infra.Repository;

public class JobRequirementRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<JobRequirement> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT * FROM JobRequirement";
        return _uow.Connection.Query<JobRequirement>(query);
    }

    public JobRequirement? GetByJobRequirementName(string name)
    {
        string query = "SELECT * FROM JobRequirement WHERE JobRequirementName = @JobRequirementName";
        return _uow.Connection.QueryFirstOrDefault<JobRequirement>(query, new { JobRequirementName = name });
    }

    public void Insert(JobRequirement JobRequirement)
    {
        string query = "INSERT INTO JobRequirement (JobRequirementName) VALUES (@JobRequirementName)";
        _uow.Connection.Execute(query, JobRequirement);
    }

    public void Update(JobRequirement JobRequirement)
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
