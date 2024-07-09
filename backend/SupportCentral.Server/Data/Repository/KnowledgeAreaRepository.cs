namespace SupportCentral.Server.Data.Repository;

using SupportCentral.Server.Data.Model;
using Dapper;

public class KnowledgeAreaRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<KnowledgeArea> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT * FROM KnowledgeArea";
        return _uow.Connection.Query<KnowledgeArea>(query);
    }

    public KnowledgeArea? GetById(int id)
    {
        string query = "SELECT * FROM KnowledgeArea WHERE Id = @Id";
        return _uow.Connection.QueryFirstOrDefault<KnowledgeArea>(query, new { Id = id });
    }

    public void Insert(KnowledgeArea KnowledgeArea)
    {
        string query = "INSERT INTO KnowledgeArea (Name) VALUES (@Name)";
        _uow.Connection.Execute(query, KnowledgeArea);
    }

    public void Update(KnowledgeArea KnowledgeArea)
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
