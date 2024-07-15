using Dapper;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Infra.Repository;

public class CurrencyRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<CurrencyVO> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT CurrencyName, Prefix FROM Currency";
        return _uow.Connection.Query<CurrencyVO>(query);
    }

    public CurrencyVO? GetByCurrencyName(string name)
    {
        string query = "SELECT CurrencyName, Prefix FROM Currency WHERE CurrencyName = @CurrencyName";
        return _uow.Connection.QueryFirstOrDefault<CurrencyVO>(query, new { CurrencyName = name });
    }

    public void Insert(CurrencyVO Currency)
    {
        string query = "INSERT INTO Currency (CurrencyName, Prefix) VALUES (@CurrencyName, @Prefix)";
        _uow.Connection.Execute(query, Currency);
    }

    public void Update(CurrencyVO Currency)
    {
        string query = "UPDATE Currency SET CurrencyName = @CurrencyName, Prefix = @Prefix WHERE CurrencyName = @CurrencyName";
        _uow.Connection.Execute(query, Currency);
    }

    public void Delete(string name)
    {
        string query = "DELETE CASCADE FROM Currency WHERE CurrencyName = @CurrencyName";
        _uow.Connection.Execute(query, new { CurrencyName = name });
    }
}
