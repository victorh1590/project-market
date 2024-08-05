using System.ComponentModel.DataAnnotations;
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
        // var query = compiler.Compile(new Query("Currency").Select("CurrencyName", "Prefix"))
        //             ?? throw new InvalidDataException($"{nameof(CurrencyRepository)} Failed to compile query");
        // TODO Use pagination instead.
        string query = "SELECT \"CurrencyName\", \"Prefix\" FROM \"Currency\"";
        return UnitOfWork.Connection.Query<CurrencyVo>(query);
    }

    public CurrencyVo GetCurrencyByName(string name)
    {
        string query = "SELECT \"CurrencyName\", \"Prefix\" " +
                       "FROM \"Currency\" WHERE \"CurrencyName\" = @CurrencyName";
        try
        {
            var record = UnitOfWork.Connection.QuerySingle<CurrencyRecord>(query, new { CurrencyName = name });
            CurrencyVo result = new(record);
            return result;
        }
        catch (Exception)
        {
            throw new ArgumentException($"{nameof(CurrencyVo.CurrencyName)} \'{name}\' not found");
        }
    }

    public CurrencyVo Insert(CurrencyVo currency)
    {
        string query = "INSERT INTO \"Currency\" (\"CurrencyName\", \"Prefix\") " +
                       "VALUES (@CurrencyName, @Prefix) " +
                       "RETURNING \"CurrencyName\", \"Prefix\"";
        
        return UnitOfWork.Connection.QuerySingle<CurrencyVo>(query, currency);
    }

    public bool Update(string name, CurrencyVo Currency)
    {
        string query = "UPDATE \"Currency\" SET \"CurrencyName\" = @CurrencyName, \"Prefix\" = @Prefix WHERE \"CurrencyName\" = @CurrencyNameToUpdate RETURNING true";
        return UnitOfWork.Connection.QuerySingle<bool>(query, new
        {
            CurrencyNameToUpdate = name,
            CurrencyName = Currency.CurrencyName,
            Prefix = Currency.Prefix
        });
    }

    public bool Delete(string name)
    {
        string query = "DELETE FROM \"Currency\" CASCADE WHERE \"CurrencyName\" = @CurrencyName RETURNING true";
        return UnitOfWork.Connection.QuerySingle<bool>(query, new { CurrencyName = name });
    }
}
