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

    public KnowledgeAreaVO? GetById(int id)
    {
        string query = "SELECT * FROM KnowledgeArea WHERE Id = @Id";
        return _uow.Connection.QueryFirstOrDefault<KnowledgeAreaVO>(query, new { Id = id });
    }

    public void Insert(KnowledgeAreaVO KnowledgeArea)
    {
        string query = "INSERT INTO KnowledgeArea (Name) VALUES (@Name)";
        _uow.Connection.Execute(query, KnowledgeArea);
    }

    public void Update(KnowledgeAreaVO KnowledgeArea)
    {
        string query = "UPDATE KnowledgeArea SET Name = @Name WHERE Id = @Id";
        _uow.Connection.Execute(query, KnowledgeArea);
    }

    public void Delete(int id)
    {
        // should return 0 if it has any advertisement linked to it.
        // will require integration tests to make sure works as intended.
        string query = "DELETE FROM KnowledgeArea WHERE Id = @Id";
        _uow.Connection.Execute(query, new { Id = id });
    }
}