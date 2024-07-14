using Dapper;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Infra.Repository;

public class PaymentFrequencyRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<PaymentFrequencyVO> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT * FROM PaymentFrequency";
        return _uow.Connection.Query<PaymentFrequencyVO>(query);
    }

    public PaymentFrequencyVO? GetByPaymentFrequencyId(int id)
    {
        string query = "SELECT * FROM PaymentFrequency WHERE PaymentFrequencyId = @PaymentFrequencyId";
        return _uow.Connection.QueryFirstOrDefault<PaymentFrequencyVO>(query, new { PaymentFrequencyId = id });
    }

    public void Insert(PaymentFrequencyVO PaymentFrequency)
    {
        string query = "INSERT INTO PaymentFrequency (Description, Suffix) VALUES (@Description, @Suffix)";
        _uow.Connection.Execute(query, PaymentFrequency);
    }

    public void Update(PaymentFrequencyVO PaymentFrequency)
    {
        string query = 
            "UPDATE PaymentFrequency " +
            "SET Description = @Description, Suffix = @Suffix " +
            "WHERE PaymentFrequencyId = @PaymentFrequencyId";
        _uow.Connection.Execute(query, PaymentFrequency);
    }

    public void Delete(int id)
    {
        string query = "DELETE CASCADE FROM PaymentFrequency WHERE PaymentFrequencyId = @PaymentFrequencyId";
        _uow.Connection.Execute(query, new { PaymentFrequencyId = id });
    }
}