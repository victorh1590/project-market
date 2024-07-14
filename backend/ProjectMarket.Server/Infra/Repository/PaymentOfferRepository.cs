using Dapper;
using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Infra.Repository;

public class PaymentOfferRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<PaymentOffer> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT * FROM PaymentOffer";
        return _uow.Connection.Query<PaymentOffer>(query);
    }

    public PaymentOffer? GetByPaymentOfferId(int id)
    {
        string query = "SELECT * FROM PaymentOffer WHERE PaymentOfferId = @PaymentOfferId";
        return _uow.Connection.QueryFirstOrDefault<PaymentOffer>(query, new { PaymentOfferId = id });
    }

    public void Insert(PaymentOffer PaymentOffer)
    {
        string query = 
            "INSERT INTO PaymentOffer (Value, PaymentFrequencyName, CurrencyName) " + 
            "VALUES (@Value, @PaymentFrequencyName, @CurrencyName)";
        _uow.Connection.Execute(query, new {
            PaymentOffer.Value,
            PaymentOffer.PaymentFrequency.PaymentFrequencyName,
            PaymentOffer.Currency.CurrencyName
        });
    }

    public void Update(PaymentOffer PaymentOffer)
    {
        string query = 
            "UPDATE PaymentOffer " +
            "SET Value = @Value, PaymentFrequencyName = @PaymentFrequencyName, @CurrencyName = CurrencyName " +
            "WHERE PaymentOfferId = @PaymentOfferId";
        _uow.Connection.Execute(query, new {
            PaymentOffer.Value,
            PaymentOffer.PaymentFrequency.PaymentFrequencyName,
            PaymentOffer.Currency.CurrencyName
        });
    }

    public void Delete(int id)
    {
        string query = "DELETE CASCADE FROM PaymentOffer WHERE PaymentOfferId = @PaymentOfferId";
        _uow.Connection.Execute(query, new { PaymentOfferId = id });
    }
}