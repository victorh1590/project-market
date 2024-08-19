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
        const string sql = """
                           SELECT "CustomerId", "Name", "Email", "Password", "RegistrationDate" 
                           FROM "Customer"
                           """;
        return UnitOfWork.Connection.Query<Customer>(sql);
    }

    public Customer GetCustomerById(int id)
    {
        const string sql = """
                           SELECT "CustomerId", "Name", "Email", "Password", "RegistrationDate" 
                           FROM "Customer" 
                           WHERE "CustomerId" = @CustomerId
                           """;
        try
        {
            return UnitOfWork.Connection.QuerySingle<Customer>(sql, new { CustomerId = id });
        }
        catch (Exception)
        {
            throw new ArgumentException($"{nameof(Customer.CustomerId)} \'{id}\' not found");
        }
    }

    public Customer Insert(Customer customer)
    {
        const string sql = """
                           INSERT INTO "Customer" ("Name", "Email", "Password", "RegistrationDate") 
                           VALUES (@Name, @Email, @Password, @RegistrationDate) 
                           RETURNING "CustomerId", "Name", "Email", "Password", "RegistrationDate"
                           """;

        return UnitOfWork.Connection.QuerySingle<Customer>(sql, customer);
    }
    
    public bool Update(Customer customer)
    {
        const string sql = """
                           UPDATE "Customer" 
                           SET "Name" = @Name, "Email" = @Email, "Password" = @Password, "RegistrationDate" = @RegistrationDate 
                           WHERE "CustomerId" = @CustomerId 
                           RETURNING true
                           """;
        return UnitOfWork.Connection.QuerySingle<bool>(sql, new
        {
            customer.CustomerId,
            customer.Name,
            customer.Email,
            customer.Password,
            customer.RegistrationDate
        });
    }

    public bool Delete(int id)
    {
        const string sql = """
                           DELETE FROM "Customer" CASCADE 
                           WHERE "CustomerId" = @CustomerId 
                           RETURNING true
                           """;
        return UnitOfWork.Connection.QuerySingle<bool>(sql, new { CustomerId = id });
    }
}