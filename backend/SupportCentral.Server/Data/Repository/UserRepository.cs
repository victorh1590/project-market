namespace SupportCentral.Server.Data.Repository;

using System.Data;
using SupportCentral.Server.Data.Model;
using Dapper;

public class UserRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<User> GetAll()
    {
        string query = "SELECT * FROM Users";
        return _uow.Connection.Query<User>(query);
    }

    public User? GetById(int id)
    {
        string query = "SELECT * FROM Users WHERE Id = @Id";
        return _uow.Connection.QueryFirstOrDefault<User>(query, new { Id = id });
    }

    public void Insert(User user)
    {
        string query = "INSERT INTO Users (Username, Email) VALUES (@Username, @Email)";
        _uow.Connection.Execute(query, user);
    }

    public void Update(User user)
    {
        string query = "UPDATE Users SET Username = @Username, Email = @Email WHERE Id = @Id";
        _uow.Connection.Execute(query, user);
    }

    public void Delete(int id)
    {
        string query = "DELETE FROM Users WHERE Id = @Id";
        _uow.Connection.Execute(query, new { Id = id });
    }
}
