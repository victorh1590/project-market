using Dapper;
using Microsoft.Data.SqlClient;
using Npgsql;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.Interface;

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

    public Customer GetByCustomerId(int id)
    {
        string query = "SELECT * FROM Customer WHERE CustomerId = @CustomerId";
        return uow.Connection.QueryFirstOrDefault<Customer>(query, new { CustomerId = id }) 
               ?? throw new ArgumentException($"{nameof(ICustomer.CustomerId)} not found");
    }

    public Customer Insert(ICustomer Customer)
    {
        string query = 
            "INSERT INTO Customer (Name, Email, Password, RegistrationDate) " +
            "VALUES (@Name, @Email, @Password, @RegistrationDate) " + 
            "RETURNING CustomerId, Name, Email, Password, RegistrationDate";

       return uow.Connection.QuerySingle<Customer>(query, Customer);
    }

    public Customer Update(ICustomer Customer)
    {
        string query = 
            "UPDATE Customer " + 
            "SET Name = @Name, Email = @Email, Password = @Password, RegistrationDate = @RegistrationDate " + 
            "WHERE CustomerId = @CustomerId " +
            "RETURNING CustomerId, Name, Email, Password, RegistrationDate";
        return uow.Connection.QuerySingle<Customer>(query, Customer);
    }

    public bool Delete(ICustomer Customer)
    {
        string query = 
            "DELETE CASCADE FROM Customer WHERE CustomerId = @CustomerId " +
            "RETURNING CustomerId";
        return uow.Connection.Execute(query, Customer) == 1;
    }
}
