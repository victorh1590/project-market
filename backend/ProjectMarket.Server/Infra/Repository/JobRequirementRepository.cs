using Dapper;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Infra.Repository;

public class JobRequirementRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<JobRequirementVO> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT * FROM JobRequirement";
        return _uow.Connection.Query<JobRequirementVO>(query);
    }

    public JobRequirementVO? GetByJobRequirementId(int id)
    {
        string query = "SELECT * FROM JobRequirement WHERE JobRequirementId = @JobRequirementId";
        return _uow.Connection.QueryFirstOrDefault<JobRequirementVO>(query, new { JobRequirementId = id });
    }

    public void Insert(JobRequirementVO JobRequirement)
    {
        string query = "INSERT INTO JobRequirement (Requirement) VALUES (@Requirement)";
        _uow.Connection.Execute(query, JobRequirement);
    }

    public void Update(JobRequirementVO JobRequirement)
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
