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
        const string sql = """
                           SELECT "PaymentOfferId", "Value", 
                                  "PaymentFrequencyName", "Suffix", 
                                  "CurrencyName", "Prefix" 
                           FROM "PaymentOffer" 
                           JOIN "PaymentFrequency" USING ("PaymentFrequencyName") 
                           JOIN "Currency" USING ("CurrencyName")
                           """;
        var results = (UnitOfWork.Connection.Query<dynamic>(sql)).ToList();
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
        const string sql = """
                           SELECT "PaymentOfferId", "Value", 
                                  "PaymentFrequencyName", "Suffix", 
                                  "CurrencyName", "Prefix" 
                           FROM "PaymentOffer" 
                           JOIN "PaymentFrequency" USING ("PaymentFrequencyName") 
                           JOIN "Currency" USING ("CurrencyName") 
                           WHERE "PaymentOfferId" = @PaymentOfferId
                           """;
        try
        {
            var result = UnitOfWork.Connection.QuerySingle<dynamic>(sql, new { PaymentOfferId = id });
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
        const string sql = """
                           INSERT INTO "PaymentOffer" ("Value", "PaymentFrequencyName", "CurrencyName") 
                           VALUES (@Value, @PaymentFrequencyName, @CurrencyName) 
                           RETURNING "PaymentOfferId"
                           """;
        
        var id = UnitOfWork.Connection.QuerySingle<int>(sql, new
        {
            paymentOffer.Value,
            paymentOffer.PaymentFrequency.PaymentFrequencyName,
            paymentOffer.Currency.CurrencyName
        });
        
        return GetPaymentOfferById(id);
    }

    public bool Update(PaymentOffer paymentOffer)
    {
        const string sql = """
                           UPDATE "PaymentOffer" 
                           SET "Value" = @Value, "PaymentFrequencyName" = @PaymentFrequencyName, "CurrencyName" = @CurrencyName 
                           WHERE "PaymentOfferId" = @PaymentOfferId 
                           RETURNING true
                           """;

        return UnitOfWork.Connection.QuerySingle<bool>(sql,  new {
            paymentOffer.PaymentOfferId,
            paymentOffer.Value,
            paymentOffer.PaymentFrequency.PaymentFrequencyName,
            paymentOffer.Currency.CurrencyName
        });
    }

    public bool Delete(int id)
    {
        const string sql = """
                           DELETE FROM "PaymentOffer" CASCADE 
                           WHERE "PaymentOfferId" = @PaymentOfferId 
                           RETURNING true
                           """;
        return UnitOfWork.Connection.QuerySingle<bool>(sql, new { PaymentOfferId = id });
    }
}
