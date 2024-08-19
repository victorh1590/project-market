using Dapper;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Db;
using SqlKata.Compilers;

namespace ProjectMarket.Server.Infra.Repository;

public class CurrencyRepository(IUnitOfWork unitOfWork, Compiler compiler)
{
    public readonly IUnitOfWork UnitOfWork = unitOfWork;

    public IEnumerable<CurrencyVo> GetAll()
    {
        // TODO Use pagination instead.
        const string sql = """
                           SELECT "CurrencyName", "Prefix" 
                           FROM "Currency"
                           """;
        return UnitOfWork.Connection.Query<CurrencyVo>(sql);
    }

    public CurrencyVo GetCurrencyByName(string name)
    {
        const string sql = """
                           SELECT "CurrencyName", "Prefix" 
                           FROM "Currency" 
                           WHERE "CurrencyName" = @CurrencyName
                           """;
        try
        {
            var record = UnitOfWork.Connection.QuerySingle<CurrencyRecord>(sql, new { CurrencyName = name });
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
        const string sql = """
                           INSERT INTO "Currency" ("CurrencyName", "Prefix") 
                           VALUES (@CurrencyName, @Prefix) 
                           RETURNING "CurrencyName", "Prefix"
                           """;
        
        return UnitOfWork.Connection.QuerySingle<CurrencyVo>(sql, currency);
    }

    public bool Update(string name, CurrencyVo currency)
    {
        const string sql = """
                           UPDATE "Currency" 
                           SET "CurrencyName" = @CurrencyName, "Prefix" = @Prefix 
                           WHERE "CurrencyName" = @CurrencyNameToUpdate 
                           RETURNING true
                           """;
        return UnitOfWork.Connection.QuerySingle<bool>(sql, new
        {
            CurrencyNameToUpdate = name,
            CurrencyName = currency.CurrencyName,
            Prefix = currency.Prefix
        });
    }

    public bool Delete(string name)
    {
        const string sql = """
                           DELETE FROM "Currency" CASCADE 
                           WHERE "CurrencyName" = @CurrencyName 
                           RETURNING true
                           """;
        return UnitOfWork.Connection.QuerySingle<bool>(sql, new { CurrencyName = name });
    }
}