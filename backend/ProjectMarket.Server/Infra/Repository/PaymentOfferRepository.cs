using Dapper;
using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Infra.Repository;

public class PaymentOfferRepository
{
    public readonly IUnitOfWork UnitOfWork;

    private readonly CurrencyRepository _currencyRepository;
    private readonly PaymentFrequencyRepository _paymentFrequencyRepository;

    public PaymentOfferRepository(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
        _currencyRepository = new CurrencyRepository(UnitOfWork);
        _paymentFrequencyRepository = new PaymentFrequencyRepository(UnitOfWork);
    }
    
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
        // PaymentFrequencyVo paymentFrequency = _paymentFrequencyRepository.GetByPaymentFrequencyName(dto.PaymentFrequency);
        // CurrencyVo currency = _currencyRepository.GetByCurrencyName(dto.Currency);
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