using Dapper;
using ProjectMarket.Server.Data.Model.ValueObjects;

namespace ProjectMarket.Server.Infra.Repository;

public class KnowledgeAreaRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<KnowledgeAreaVo> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT KnowledgeAreaName FROM KnowledgeArea";
        return _uow.Connection.Query<KnowledgeAreaVo>(query);
    }

    public KnowledgeAreaVo GetByKnowledgeAreaByName(string name)
    {
        string query = "SELECT KnowledgeAreaName FROM KnowledgeArea WHERE KnowledgeAreaName = @KnowledgeAreaName";
        return _uow.Connection.QuerySingleOrDefault(query, new { KnowledgeAreaName = name })
            ?? throw new ArgumentException($"{nameof(KnowledgeAreaVo.KnowledgeAreaName)} not found");
    }

    public void Insert(KnowledgeAreaVo KnowledgeArea)
    {
        string query = "INSERT INTO KnowledgeArea (KnowledgeAreaName) VALUES (@KnowledgeAreaName)";
        _uow.Connection.Execute(query, KnowledgeArea);
    }

    public void Update(KnowledgeAreaVo KnowledgeArea)
    {
        string query = "UPDATE KnowledgeArea SET KnowledgeAreaName = @KnowledgeAreaName WHERE KnowledgeAreaName = @KnowledgeAreaName";
        _uow.Connection.Execute(query, KnowledgeArea);
    }

    public void Delete(string name)
    {
        string query = "DELETE CASCADE FROM KnowledgeArea WHERE KnowledgeAreaName = @KnowledgeAreaName";
        _uow.Connection.Execute(query, new { KnowledgeAreaName = name });
    }
}