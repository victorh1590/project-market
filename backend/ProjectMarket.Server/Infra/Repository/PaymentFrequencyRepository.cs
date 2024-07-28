using Dapper;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Db;

namespace ProjectMarket.Server.Infra.Repository;

public class PaymentFrequencyRepository(IUnitOfWork uow)
{
    public IEnumerable<PaymentFrequencyVo> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT PaymentFrequencyName, Suffix FROM PaymentFrequency";
        return uow.Connection.Query<PaymentFrequencyVo>(query);
    }

    public PaymentFrequencyVo GetPaymentFrequencyByName(string name)
    {
        string query = "SELECT PaymentFrequencyName, Suffix FROM PaymentFrequency WHERE PaymentFrequencyName = @PaymentFrequencyName";
        return uow.Connection.QuerySingleOrDefault<PaymentFrequencyVo?>(query, new { PaymentFrequencyName = name })
                    ?? throw new ArgumentException($"{nameof(PaymentFrequencyVo.PaymentFrequencyName)} not found");
    }

    public void Insert(PaymentFrequencyVo PaymentFrequency)
    {
        string query = "INSERT INTO PaymentFrequency (Description, Suffix) VALUES (@Description, @Suffix)";
        uow.Connection.Execute(query, PaymentFrequency);
    }

    public void Update(PaymentFrequencyVo PaymentFrequency)
    {
        string query = 
            "UPDATE PaymentFrequency " +
            "SET Description = @Description, Suffix = @Suffix " +
            "WHERE PaymentFrequencyName = @PaymentFrequencyName";
        uow.Connection.Execute(query, PaymentFrequency);
    }

    public void Delete(string name)
    {
        string query = "DELETE CASCADE FROM PaymentFrequency WHERE PaymentFrequencyName = @PaymentFrequencyName";
        uow.Connection.Execute(query, new { PaymentFrequencyNamePaymentFrequencyName = name });
    }
}