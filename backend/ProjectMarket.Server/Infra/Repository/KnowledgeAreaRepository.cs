using Dapper;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Db;

namespace ProjectMarket.Server.Infra.Repository;

public class KnowledgeAreaRepository(IUnitOfWork uow)
{
    public IEnumerable<KnowledgeAreaVo> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT KnowledgeAreaName FROM KnowledgeArea";
        return uow.Connection.Query<KnowledgeAreaVo>(query);
    }

    public KnowledgeAreaVo GetKnowledgeAreaByName(string name)
    {
        string query = "SELECT KnowledgeAreaName FROM KnowledgeArea WHERE KnowledgeAreaName = @KnowledgeAreaName";
        return uow.Connection.QuerySingleOrDefault(query, new { KnowledgeAreaName = name })
            ?? throw new ArgumentException($"{nameof(KnowledgeAreaVo.KnowledgeAreaName)} not found");
    }

    public void Insert(KnowledgeAreaVo KnowledgeArea)
    {
        string query = "INSERT INTO KnowledgeArea (KnowledgeAreaName) VALUES (@KnowledgeAreaName)";
        uow.Connection.Execute(query, KnowledgeArea);
    }

    public void Update(KnowledgeAreaVo KnowledgeArea)
    {
        string query = "UPDATE KnowledgeArea SET KnowledgeAreaName = @KnowledgeAreaName WHERE KnowledgeAreaName = @KnowledgeAreaName";
        uow.Connection.Execute(query, KnowledgeArea);
    }

    public void Delete(string name)
    {
        string query = "DELETE CASCADE FROM KnowledgeArea WHERE KnowledgeAreaName = @KnowledgeAreaName";
        uow.Connection.Execute(query, new { KnowledgeAreaName = name });
    }
}