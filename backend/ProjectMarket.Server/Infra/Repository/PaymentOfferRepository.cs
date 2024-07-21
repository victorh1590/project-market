using Dapper;
using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Infra.Repository;

public class PaymentOfferRepository(IUnitOfWork uow)
{
    private readonly CurrencyRepository _currencyRepository = new(uow);
    private readonly PaymentFrequencyRepository _paymentFrequencyRepository = new(uow);
    
    public IEnumerable<PaymentOffer> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT * FROM PaymentOffer";
        return uow.Connection.Query<PaymentOffer>(query);
    }

    public PaymentOffer GetByPaymentOfferById(int id)
    {
        string query = "SELECT * FROM PaymentOffer WHERE PaymentOfferId = @PaymentOfferId";
        return uow.Connection.QueryFirstOrDefault<PaymentOffer>(query, new { PaymentOfferId = id }) 
                              ?? throw new ArgumentException($"{nameof(PaymentOffer.PaymentOfferId)} not found");
        // PaymentFrequencyVo paymentFrequency = _paymentFrequencyRepository.GetByPaymentFrequencyName(dto.PaymentFrequency);
        // CurrencyVo currency = _currencyRepository.GetByCurrencyName(dto.Currency);
    }

    public void Insert(PaymentOffer PaymentOffer)
    {
        string query = 
            "INSERT INTO PaymentOffer (Value, PaymentFrequencyName, CurrencyName) " + 
            "VALUES (@Value, @PaymentFrequencyName, @CurrencyName)";
        uow.Connection.Execute(query, new {
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
        uow.Connection.Execute(query, new {
            PaymentOffer.Value,
            PaymentOffer.PaymentFrequency.PaymentFrequencyName,
            PaymentOffer.Currency.CurrencyName
        });
    }

    public void Delete(int id)
    {
        string query = "DELETE CASCADE FROM PaymentOffer WHERE PaymentOfferId = @PaymentOfferId";
        uow.Connection.Execute(query, new { PaymentOfferId = id });
    }
}