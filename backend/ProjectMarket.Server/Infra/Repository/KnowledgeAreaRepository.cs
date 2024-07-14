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

    public KnowledgeAreaVO? GetByKnowledgeAreaId(int id)
    {
        string query = "SELECT * FROM KnowledgeArea WHERE KnowledgeAreaId = @KnowledgeAreaId";
        return _uow.Connection.QueryFirstOrDefault<KnowledgeAreaVO>(query, new { KnowledgeAreaId = id });
    }

    public void Insert(KnowledgeAreaVO KnowledgeArea)
    {
        string query = "INSERT INTO KnowledgeArea (Name) VALUES (@Name)";
        _uow.Connection.Execute(query, KnowledgeArea);
    }

    public void Update(KnowledgeAreaVO KnowledgeArea)
    {
        string query = "UPDATE KnowledgeArea SET Name = @Name WHERE KnowledgeAreaId = @KnowledgeAreaId";
        _uow.Connection.Execute(query, KnowledgeArea);
    }

    public void Delete(int id)
    {
        string query = "DELETE CASCADE FROM KnowledgeArea WHERE KnowledgeAreaId = @KnowledgeAreaId";
        _uow.Connection.Execute(query, new { KnowledgeAreaId = id });
    }
}