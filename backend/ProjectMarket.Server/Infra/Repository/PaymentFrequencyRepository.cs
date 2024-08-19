using Dapper;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Db;
using SqlKata.Compilers;

namespace ProjectMarket.Server.Infra.Repository;

public class PaymentFrequencyRepository(IUnitOfWork unitOfWork, Compiler compiler)
{
    public readonly IUnitOfWork UnitOfWork = unitOfWork;
    
    public IEnumerable<PaymentFrequencyVo> GetAll()
    {
        // TODO Use pagination instead.
        const string sql = """
                           SELECT "PaymentFrequencyName", "Suffix" 
                           FROM "PaymentFrequency"
                           """;
        return UnitOfWork.Connection.Query<PaymentFrequencyVo>(sql);
    }

    public PaymentFrequencyVo GetPaymentFrequencyByName(string name)
    {
        const string sql = """
                           SELECT "PaymentFrequencyName", "Suffix" 
                           FROM "PaymentFrequency" 
                           WHERE "PaymentFrequencyName" = @PaymentFrequencyName
                           """;
        try
        {
            var record = UnitOfWork.Connection.QuerySingle<PaymentFrequencyRecord>(sql, new { PaymentFrequencyName = name });
            PaymentFrequencyVo result = new(record);
            return result;
        }
        catch (Exception)
        {
            throw new ArgumentException($"{nameof(PaymentFrequencyVo.PaymentFrequencyName)} \'{name}\' not found");
        }
    }

    public PaymentFrequencyVo Insert(PaymentFrequencyVo paymentFrequency)
    {
        const string sql = """
                           INSERT INTO "PaymentFrequency" ("PaymentFrequencyName", "Suffix") 
                           VALUES (@PaymentFrequencyName, @Suffix) 
                           RETURNING "PaymentFrequencyName", "Suffix"
                           """;
        return UnitOfWork.Connection.QuerySingle<PaymentFrequencyVo>(sql, paymentFrequency);
    }

    public bool Update(string name, PaymentFrequencyVo paymentFrequency)
    {
        const string sql = """
                           UPDATE "PaymentFrequency" 
                           SET "PaymentFrequencyName" = @PaymentFrequencyName, "Suffix" = @Suffix 
                           WHERE "PaymentFrequencyName" = @PaymentFrequencyNameToUpdate 
                           RETURNING true
                           """;
        return UnitOfWork.Connection.QuerySingle<bool>(sql, new
        {
            PaymentFrequencyNameToUpdate = name,
            PaymentFrequencyName = paymentFrequency.PaymentFrequencyName,
            Suffix = paymentFrequency.Suffix
        });
    }

    public bool Delete(string name)
    {
        const string sql = """
                           DELETE FROM "PaymentFrequency" CASCADE 
                           WHERE "PaymentFrequencyName" = @PaymentFrequencyName 
                           RETURNING true
                           """;
        return UnitOfWork.Connection.QuerySingle<bool>(sql, new { PaymentFrequencyName = name });
    }
}