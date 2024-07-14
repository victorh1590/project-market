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

    public JobRequirement? GetByJobRequirementId(int id)
    {
        string query = "SELECT * FROM JobRequirement WHERE JobRequirementId = @JobRequirementId";
        return _uow.Connection.QueryFirstOrDefault<JobRequirement>(query, new { JobRequirementId = id });
    }

    public void Insert(JobRequirement JobRequirement)
    {
        string query = "INSERT INTO JobRequirement (Requirement) VALUES (@Requirement)";
        _uow.Connection.Execute(query, JobRequirement);
    }

    public void Update(JobRequirement JobRequirement)
    {
        string query = "UPDATE JobRequirement SET Requirement = @Requirement WHERE JobRequirementId = @JobRequirementId";
        _uow.Connection.Execute(query, JobRequirement);
    }

    public void Delete(int id)
    {
        string query = "DELETE CASCADE FROM JobRequirement WHERE JobRequirementId = @JobRequirementId";
        _uow.Connection.Execute(query, new { JobRequirementId = id });
    }
}
