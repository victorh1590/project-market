using Dapper;
using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Infra.Repository;

public class CostumerRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<Costumer> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT * FROM Costumer";
        return _uow.Connection.Query<Costumer>(query);
    }

    public Costumer? GetByCostumerId(int id)
    {
        string query = "SELECT * FROM Costumer WHERE CostumerId = @CostumerId";
        return _uow.Connection.QueryFirstOrDefault<Costumer>(query, new { CostumerId = id });
    }

    public void Insert(Costumer Costumer)
    {
        string query = 
            "INSERT INTO Costumer (Name, Email, Password, RegistrationDate) " +
            "VALUES (@Name, @Email, @Password, @RegistrationDate)";

        _uow.Connection.Execute(query,Costumer);
    }

    public void Update(Costumer Costumer)
    {
        string query = 
            "UPDATE Costumer " + 
            "SET Name = @Name, Email = @Email, Password = @Password, RegistrationDate = @RegistrationDate " + 
            "WHERE CostumerId = @CostumerId";
        _uow.Connection.Execute(query, Costumer);
    }

    public void Delete(Costumer Costumer)
    {
        string query = "DELETE CASCADE FROM Costumer WHERE CostumerId = @CostumerId";
        _uow.Connection.Execute(query, Costumer);
    }
}
