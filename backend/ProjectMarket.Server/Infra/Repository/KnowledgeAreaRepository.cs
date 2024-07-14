using Dapper;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Infra.Repository;

public class KnowledgeAreaRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<KnowledgeAreaVO> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT * FROM KnowledgeArea";
        return _uow.Connection.Query<KnowledgeAreaVO>(query);
    }

    public KnowledgeAreaVO? GetByKnowledgeAreaName(string name)
    {
        string query = "SELECT * FROM KnowledgeArea WHERE KnowledgeAreaName = @KnowledgeAreaName";
        return _uow.Connection.QueryFirstOrDefault<KnowledgeAreaVO>(query, new { KnowledgeAreaName = name });
    }

    public void Insert(KnowledgeAreaVO KnowledgeArea)
    {
        string query = "INSERT INTO KnowledgeArea (KnowledgeAreaName) VALUES (@KnowledgeAreaName)";
        _uow.Connection.Execute(query, KnowledgeArea);
    }

    public void Update(KnowledgeAreaVO KnowledgeArea)
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