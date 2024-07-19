using Dapper;
using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Infra.Repository;

public class CustomerRepository(IUnitOfWork unityOfWork)
{
    public readonly IUnitOfWork uow = unityOfWork;

    public IEnumerable<Customer> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT * FROM Customer";
        return uow.Connection.Query<Customer>(query);
    }

    public Customer? GetByCustomerId(int id)
    {
        string query = "SELECT * FROM Customer WHERE CustomerId = @CustomerId";
        return uow.Connection.QueryFirstOrDefault<Customer>(query, new { CustomerId = id });
    }

    public void Insert(Customer Customer)
    {
        string query = 
            "INSERT INTO Customer (Name, Email, Password, RegistrationDate) " +
            "VALUES (@Name, @Email, @Password, @RegistrationDate)";

        uow.Connection.Execute(query,Customer);
    }

    public void Update(Customer Customer)
    {
        string query = 
            "UPDATE Customer " + 
            "SET Name = @Name, Email = @Email, Password = @Password, RegistrationDate = @RegistrationDate " + 
            "WHERE CustomerId = @CustomerId";
        uow.Connection.Execute(query, Customer);
    }

    public void Delete(Customer Customer)
    {
        string query = "DELETE CASCADE FROM Customer WHERE CustomerId = @CustomerId";
        uow.Connection.Execute(query, Customer);
    }
}
