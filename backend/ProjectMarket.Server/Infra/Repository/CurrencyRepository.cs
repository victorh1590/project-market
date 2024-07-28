using Dapper;
using Microsoft.Data.SqlClient;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Db;
using SqlKata;
using SqlKata.Compilers;

namespace ProjectMarket.Server.Infra.Repository;

public class CurrencyRepository(IUnitOfWork unitOfWork, Compiler compiler)
{
    public readonly IUnitOfWork UnitOfWork = unitOfWork;

    public IEnumerable<CurrencyVo> GetAll()
    {
        var query = compiler.Compile(new Query("Currency").Select("CurrencyName", "Prefix"))
                    ?? throw new InvalidDataException($"{nameof(CurrencyRepository)} Failed to compile query");
        // TODO Use pagination instead.
        // string query = "SELECT CurrencyName, Prefix FROM Currency";
        return UnitOfWork.Connection.Query<CurrencyVo>(query.Sql);
    }

    public CurrencyVo GetCurrencyByName(string name)
    {
        string query = "SELECT CurrencyName, Prefix FROM Currency WHERE CurrencyName = @CurrencyName";
        return UnitOfWork.Connection.QuerySingleOrDefault<CurrencyVo?>(query, new { CurrencyName = name })
                ?? throw new ArgumentException($"{nameof(CurrencyVo.CurrencyName)} not found");
    }

    public void Insert(CurrencyVo Currency)
    {
        string query = "INSERT INTO Currency (CurrencyName, Prefix) VALUES (@CurrencyName, @Prefix)";
        UnitOfWork.Connection.Execute(query, Currency);
    }

    public void Update(CurrencyVo Currency)
    {
        string query = "UPDATE Currency SET CurrencyName = @CurrencyName, Prefix = @Prefix WHERE CurrencyName = @CurrencyName";
        UnitOfWork.Connection.Execute(query, Currency);
    }

    public void Delete(string name)
    {
        string query = "DELETE CASCADE FROM Currency WHERE CurrencyName = @CurrencyName";
        UnitOfWork.Connection.Execute(query, new { CurrencyName = name });
    }
}
