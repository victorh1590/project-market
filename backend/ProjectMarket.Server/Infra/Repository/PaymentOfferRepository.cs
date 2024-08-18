using System.Collections;
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
        const string query = "SELECT " +
                             "\"PaymentOfferId\", \"Value\", " +
                             "\"PaymentFrequencyName\", \"Suffix\", " + // from PaymentFrequencyVo
                             "\"CurrencyName\", \"Prefix\" " + // from CurrencyVo
                             "FROM \"PaymentOffer\" " +
                             "JOIN \"PaymentFrequency\" USING (\"PaymentFrequencyName\") " +
                             "JOIN \"Currency\" USING (\"CurrencyName\")";
        var results = (UnitOfWork.Connection.Query<dynamic>(query)).ToList();
        List<PaymentOffer> paymentOffers = [];
        foreach (var obj in results)
        {
            PaymentFrequencyVo paymentFrequency = new(obj.PaymentFrequencyName, obj.Suffix);
            CurrencyVo currency = new(obj.CurrencyName, obj.Prefix);
            paymentOffers.Add(new(obj.PaymentOfferId, obj.Value, paymentFrequency, currency));
        }

        return paymentOffers;
    }

    public PaymentOffer GetPaymentOfferById(int id)
    {
        const string query = "SELECT \"PaymentOfferId\", \"Value\", " +
                             "\"PaymentFrequencyName\", \"Suffix\", " +
                             "\"CurrencyName\", \"Prefix\" " +
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

    // TODO: Fix RETURNING.
    public PaymentOffer Insert(PaymentOffer paymentOffer)
    {
        const string query = "INSERT INTO \"PaymentOffer\" (\"Value\", \"PaymentFrequencyName\", \"CurrencyName\") " + 
                             "VALUES (@Value, @PaymentFrequencyName, @CurrencyName) " +
                             "RETURNING \"PaymentOfferId\", \"Value\", \"PaymentFrequencyName\", \"CurrencyName\"";

        return UnitOfWork.Connection.QuerySingle<PaymentOffer>(query, new
        {
            paymentOffer.Value,
            paymentOffer.PaymentFrequency.PaymentFrequencyName,
            paymentOffer.Currency.CurrencyName
        });
    }

    public bool Update(PaymentOffer paymentOffer)
    {
        const string query = "UPDATE \"PaymentOffer\" " +
                             "SET \"Value\" = @Value, \"PaymentFrequencyName\" = @PaymentFrequencyName, \"CurrencyName\" = @CurrencyName " +
                             "WHERE \"PaymentOfferId\" = @PaymentOfferId " +
                             "RETURNING true";

        return UnitOfWork.Connection.QuerySingle<bool>(query,  new {
            PaymentOfferId = paymentOffer.PaymentOfferId,
            Value = paymentOffer.Value,
            PaymentFrequencyName = paymentOffer.PaymentFrequency.PaymentFrequencyName,
            CurrencyName = paymentOffer.Currency.CurrencyName
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