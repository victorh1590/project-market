using Dapper;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Infra.Repository;

public class CurrencyRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<Currency> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT * FROM Currency";
        return _uow.Connection.Query<Currency>(query);
    }

    public Currency? GetByCurrencyId(int id)
    {
        string query = "SELECT * FROM Currency WHERE CurrencyId = @CurrencyId";
        return _uow.Connection.QueryFirstOrDefault<Currency>(query, new { CurrencyId = id });
    }

    public void Insert(Currency Currency)
    {
        string query = "INSERT INTO Currency (Name, Prefix) VALUES (@Name, @Prefix)";
        _uow.Connection.Execute(query, Currency);
    }

    public void Update(Currency Currency)
    {
        string query = "UPDATE Currency SET Name = @Name, Prefix = @Prefix WHERE CurrencyId = @CurrencyId";
        _uow.Connection.Execute(query, Currency);
    }

    public void Delete(int id)
    {
        string query = "DELETE CASCADE FROM Currency WHERE CurrencyId = @CurrencyId";
        _uow.Connection.Execute(query, new { CurrencyId = id });
    }
}
