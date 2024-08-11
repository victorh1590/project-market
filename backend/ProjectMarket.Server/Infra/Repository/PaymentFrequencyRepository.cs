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
        string query = "SELECT \"PaymentFrequencyName\", \"Suffix\" " +
                       "FROM \"PaymentFrequency\"";
        return UnitOfWork.Connection.Query<PaymentFrequencyVo>(query);
    }

    public PaymentFrequencyVo GetPaymentFrequencyByName(string name)
    {
        string query = "SELECT \"PaymentFrequencyName\", \"Suffix\" " +
                       "FROM \"PaymentFrequency\" " +
                       "WHERE \"PaymentFrequencyName\" = @PaymentFrequencyName";
        try
        {
            var record = UnitOfWork.Connection.QuerySingle<PaymentFrequencyRecord>(query, new { PaymentFrequencyName = name });
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
        string query = "INSERT INTO \"PaymentFrequency\" (\"PaymentFrequencyName\", \"Suffix\") " +
                       "VALUES (@PaymentFrequencyName, @Suffix) " +
                       "RETURNING \"PaymentFrequencyName\", \"Suffix\"";
        return UnitOfWork.Connection.QuerySingle<PaymentFrequencyVo>(query, paymentFrequency);
    }

    public bool Update(string name, PaymentFrequencyVo paymentFrequency)
    {
        string query = "UPDATE \"PaymentFrequency\" " +
                       "SET  \"PaymentFrequencyName\" = @PaymentFrequencyName, \"Suffix\" = @Suffix " +
                       "WHERE  \"PaymentFrequencyName\" = @PaymentFrequencyNameToUpdate " +
                       "RETURNING true";
        return UnitOfWork.Connection.QuerySingle<bool>(query, new
        {
            PaymentFrequencyNameToUpdate = name,
            PaymentFrequencyName = paymentFrequency.PaymentFrequencyName,
            Suffix = paymentFrequency.Suffix
        });
    }

    public bool Delete(string name)
    {
        string query = "DELETE FROM \"PaymentFrequency\" CASCADE " +
                       "WHERE  \"PaymentFrequencyName\" = @PaymentFrequencyName " +
                       "RETURNING true";
        return UnitOfWork.Connection.QuerySingle<bool>(query, new { PaymentFrequencyName = name });
    }
}