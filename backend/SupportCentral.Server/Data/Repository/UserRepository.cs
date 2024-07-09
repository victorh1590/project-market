namespace SupportCentral.Server.Data.Repository;

using SupportCentral.Server.Data.Model;
using Dapper;

public class UserRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<User> GetAll()
    {
        string query = "SELECT * FROM User";
        return _uow.Connection.Query<User>(query);
    }

    public User? GetById(int id)
    {
        string query = "SELECT * FROM User WHERE Id = @Id";
        return _uow.Connection.QueryFirstOrDefault<User>(query, new { Id = id });
    }

    public void Insert(User user)
    {
        string query = 
            "INSERT INTO User (Name, Email, Password, Sector) " +
            "VALUES (@Username, @Email, @Password, @Sector)";
        _uow.Connection.Execute(query, user);
    }

    public void Update(User user)
    {
        string query = 
            "UPDATE User " + 
            "SET Name = @Name, Email = @Email, Password = @Password, Sector = @Sector " + 
            "WHERE Id = @Id";
        _uow.Connection.Execute(query, user);
    }

    public void Delete(int id)
    {
        string query = "DELETE FROM User WHERE Id = @Id";
        _uow.Connection.Execute(query, new { Id = id });
    }
}
