using Dapper;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Infra.Repository;

public class CurrencyRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<CurrencyVo> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT CurrencyName, Prefix FROM Currency";
        return _uow.Connection.Query<CurrencyVo>(query);
    }

    public CurrencyVo GetByCurrencyName(string name)
    {
        string query = "SELECT CurrencyName, Prefix FROM Currency WHERE CurrencyName = @CurrencyName";
        return _uow.Connection.QuerySingleOrDefault<CurrencyVo?>(query, new { CurrencyName = name })
                ?? throw new ArgumentException($"{nameof(CurrencyVo.CurrencyName)} not found");
    }

    public void Insert(CurrencyVo Currency)
    {
        string query = "INSERT INTO Currency (CurrencyName, Prefix) VALUES (@CurrencyName, @Prefix)";
        _uow.Connection.Execute(query, Currency);
    }

    public void Update(CurrencyVo Currency)
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
