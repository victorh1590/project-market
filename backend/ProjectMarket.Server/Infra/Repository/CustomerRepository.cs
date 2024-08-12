using Dapper;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Infra.Db;
using SqlKata.Compilers;

namespace ProjectMarket.Server.Infra.Repository;

public class CustomerRepository(IUnitOfWork unitOfWork, Compiler compiler)
{
    public readonly IUnitOfWork UnitOfWork = unitOfWork;

    public IEnumerable<Customer> GetAll()
    {
        // TODO Use pagination instead.
        const string query = "SELECT \"CustomerId\", \"Name\", \"Email\", \"Password\", \"RegistrationDate\" " +
                             "FROM \"Customer\"";
        return UnitOfWork.Connection.Query<Customer>(query);
    }

    public Customer GetCustomerById(int id)
    {
        const string query = "SELECT \"CustomerId\", \"Name\", \"Email\", \"Password\", \"RegistrationDate\" " +
                             "FROM \"Customer\" " +
                             "WHERE \"CustomerId\" = @CustomerId";
        try
        {
            return UnitOfWork.Connection.QuerySingle<Customer>(query, new { CustomerId = id });
        }
        catch (Exception)
        {
            throw new ArgumentException($"{nameof(Customer.Name)} \'{id}\' not found");
        }
    }

    public Customer Insert(Customer customer)
    {
        const string query = "INSERT INTO \"Customer\" (\"Name\", \"Email\", \"Password\", \"RegistrationDate\") " +
                             "VALUES (@Name, @Email, @Password, @RegistrationDate) " + 
                             "RETURNING \"CustomerId\", \"Name\", \"Email\", \"Password\", \"RegistrationDate\"";

        return UnitOfWork.Connection.QuerySingle<Customer>(query, customer);
    }
    
    public bool Update(Customer customer)
    {
        const string query = "UPDATE \"Customer\" " + 
                             "SET \"Name\" = @Name, \"Email\" = @Email, \"Password\" = @Password, \"RegistrationDate\" = @RegistrationDate " + 
                             "WHERE \"CustomerId\" = @CustomerId " +
                             "RETURNING true";
        return UnitOfWork.Connection.QuerySingle<bool>(query, new
        {
            CustomerId = customer.CustomerId,
            Name = customer.Name,
            Email = customer.Email,
            Password = customer.Password,
            RegistrationDate = customer.RegistrationDate
        });
    }

    public bool Delete(int id)
    {
        const string query = "DELETE FROM \"Customer\" CASCADE " +
                             "WHERE \"CustomerId\" = @CustomerId " +
                             "RETURNING true";
        return UnitOfWork.Connection.QuerySingle<bool>(query, new { CustomerId = id });
    }
}
