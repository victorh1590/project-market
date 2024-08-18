using Dapper;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Db;
using SqlKata.Compilers;

namespace ProjectMarket.Server.Infra.Repository;

public class PaymentOfferRepository(IUnitOfWork unitOfWork, Compiler compiler)
{
    public readonly IUnitOfWork UnitOfWork = unitOfWork;
    
    public IEnumerable<PaymentOffer> GetAll()
    {
        // TODO Use pagination instead.
        const string query = "SELECT \"PaymentOfferId\", \"Value\", \"PaymentFrequencyName\", \"CurrencyName\" " +
                             "FROM \"PaymentOffer\"";
        return UnitOfWork.Connection.Query<PaymentOffer>(query);
    }

    public PaymentOffer GetPaymentOfferById(int id)
    {
        const string query = "SELECT " +
                             "\"PaymentOfferId\", \"Value\", " +
                             "\"PaymentFrequencyName\", \"Suffix\", " + // from PaymentFrequencyVo
                             "\"CurrencyName\", \"Prefix\", " + // from CurrencyVo
                             "FROM \"PaymentOffer\" " + 
                             "JOIN \"PaymentFrequency\" USING (\"PaymentFrequencyName\") " +
                             "JOIN \"Currency\" USING (\"CurrencyName\") " +
                             "WHERE \"PaymentOfferId\" = @PaymentOfferId";
        try
        {
            var result = UnitOfWork.Connection.QuerySingle<dynamic>(query, new { PaymentOfferId = id });
            PaymentFrequencyVo paymentFrequency = new(result.PaymentFrequencyName, result.Suffix);
            CurrencyVo currency = new(result.CurrencyName, result.Prefix);
            return new(result.PaymentOfferId, result.Value, paymentFrequency, currency);
        }
        catch (Exception)
        {
            throw new ArgumentException($"{nameof(PaymentOffer.PaymentOfferId)} \'{id}\' not found");
        }
    }

    public PaymentOffer Insert(PaymentOffer paymentOffer)
    {
        const string query = "INSERT INTO \"PaymentOffer\" (\"Value\", \"PaymentFrequencyName\", \"CurrencyName\") " + 
                             "VALUES (@Value, @PaymentFrequencyName, @CurrencyName) " +
                             "RETURNING \"PaymentOfferId\", \"Value\", \"PaymentFrequencyName\", \"CurrencyName\"";

        return UnitOfWork.Connection.QuerySingle<PaymentOffer>(query, paymentOffer);
    }

    public bool Update(PaymentOffer paymentOffer)
    {
        const string query = "UPDATE \"PaymentOffer\" " +
                             "SET \"Value\" = @Value, \"PaymentFrequencyName\" = @PaymentFrequencyName, \"CurrencyName\" = @CurrencyName " +
                             "WHERE \"PaymentOfferId\" = @PaymentOfferId " +
                             "RETURNING true";

        return UnitOfWork.Connection.QuerySingle<bool>(query,  new {
            paymentOffer.PaymentOfferId,
            paymentOffer.Value,
            paymentOffer.PaymentFrequency.PaymentFrequencyName,
            paymentOffer.Currency.CurrencyName
        });
    }

    public bool Delete(int id)
    {
        const string query = "DELETE FROM \"PaymentOffer\" CASCADE " +
                             "WHERE \"PaymentOfferId\" = @PaymentOfferId " +
                             "RETURNING true";
        return UnitOfWork.Connection.QuerySingle<bool>(query, new { PaymentOfferId = id });
    }
}