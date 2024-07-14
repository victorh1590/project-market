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

    public PaymentOffer? GetByPaymentOfferName(string name)
    {
        string query = "SELECT * FROM PaymentOffer WHERE PaymentOfferName = @PaymentOfferName";
        return _uow.Connection.QueryFirstOrDefault<PaymentOffer>(query, new { PaymentOfferName = name });
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

    public void Delete(string name)
    {
        string query = "DELETE CASCADE FROM PaymentOffer WHERE PaymentOfferName = @PaymentOfferName";
        _uow.Connection.Execute(query, new { PaymentOfferName = name });
    }
}