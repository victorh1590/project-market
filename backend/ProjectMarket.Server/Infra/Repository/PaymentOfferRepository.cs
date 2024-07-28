using Dapper;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Infra.Db;
using SqlKata.Compilers;

namespace ProjectMarket.Server.Infra.Repository;

public class PaymentOfferRepository(IUnitOfWork unitOfWork, CurrencyRepository currencyRepository, PaymentFrequencyRepository paymentFrequencyRepository)
{
    public readonly IUnitOfWork UnitOfWork = unitOfWork;
    
    public IEnumerable<PaymentOffer> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT * FROM PaymentOffer";
        return UnitOfWork.Connection.Query<PaymentOffer>(query);
    }

    public PaymentOffer GetPaymentOfferById(int id)
    {
        string query = "SELECT * FROM PaymentOffer WHERE PaymentOfferId = @PaymentOfferId";
        return UnitOfWork.Connection.QueryFirstOrDefault<PaymentOffer>(query, new { PaymentOfferId = id }) 
                              ?? throw new ArgumentException($"{nameof(PaymentOffer.PaymentOfferId)} not found");
        // PaymentFrequencyVo paymentFrequency = paymentFrequencyRepository.GetByPaymentFrequencyName(dto.PaymentFrequency);
        // CurrencyVo currency = currencyRepository.GetByCurrencyName(dto.Currency);
    }

    public PaymentOffer Insert(PaymentOffer paymentOffer)
    {
        string query = 
            "INSERT INTO PaymentOffer (Value, PaymentFrequencyName, CurrencyName) " + 
            "VALUES (@Value, @PaymentFrequencyName, @CurrencyName) " +
            "RETURNING PaymentOfferId, Value, PaymentFrequencyName, CurrencyName";

        return UnitOfWork.Connection.QuerySingle<PaymentOffer>(query, paymentOffer);
        // UnitOfWork.Connection.Execute(query, new {
        //     paymentOffer.Value,
        //     paymentOffer.PaymentFrequency.PaymentFrequencyName,
        //     paymentOffer.Currency.CurrencyName
        // });
    }

    public PaymentOffer Update(PaymentOffer paymentOffer)
    {
        string query = 
            "UPDATE PaymentOffer " +
            "SET Value = @Value, PaymentFrequencyName = @PaymentFrequencyName, @CurrencyName = CurrencyName " +
            "WHERE PaymentOfferId = @PaymentOfferId " +
            "RETURNING PaymentOfferId, Value, PaymentFrequencyName, CurrencyName";

        return UnitOfWork.Connection.QuerySingle<PaymentOffer>(query, paymentOffer);
        // UnitOfWork.Connection.Execute(query, new {
        //     paymentOffer.Value,
        //     paymentOffer.PaymentFrequency.PaymentFrequencyName,
        //     paymentOffer.Currency.CurrencyName
        // });
    }

    public void Delete(int id)
    {
        string query = "DELETE FROM PaymentOffer CASCADE WHERE PaymentOfferId = @PaymentOfferId";
        UnitOfWork.Connection.Execute(query, new { PaymentOfferId = id });
    }
}