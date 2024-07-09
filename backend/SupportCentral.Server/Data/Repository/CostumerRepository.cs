namespace SupportCentral.Server.Data.Repository;

using SupportCentral.Server.Data.Model;
using Dapper;

public class CostumerRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<Costumer> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT * FROM Costumer";
        return _uow.Connection.Query<Costumer>(query);
    }

    public Costumer? GetById(int id)
    {
        string query = "SELECT * FROM Costumer WHERE Id = @Id";
        return _uow.Connection.QueryFirstOrDefault<Costumer>(query, new { Id = id });
    }

    public void Insert(Costumer Costumer)
    {
        string insertCostumer = 
            "INSERT INTO Costumer (Name, Email, Password, RegistrationDate) " +
            "VALUES (@Name, @Email, @Password, @RegistrationDate)";

        _uow.Connection.Execute(insertCostumer,Costumer);

        // if(Costumer.Advertisements?.Count > 0) {
        //   // delegate to Advertisements Repo and pass _uow as dependency.
        // }
    }

    public void Update(Costumer Costumer)
    {
        string query = 
            "UPDATE Costumer " + 
            "SET Name = @Name, Email = @Email, Password = @Password, RegistrationDate = @RegistrationDate " + 
            "WHERE Id = @Id";
        _uow.Connection.Execute(query, Costumer);
    }

    public void Delete(Costumer Costumer)
    {
        // delegate to Advertisements repo first, do result == Costumer.Advertisements... and proceed if ok.
        // cascade?
        string query = "DELETE FROM Costumer WHERE Id = @Id";
        _uow.Connection.Execute(query, Costumer);
    }
}
