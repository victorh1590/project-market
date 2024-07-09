namespace SupportCentral.Server.Data.Repository;

using SupportCentral.Server.Data.Model;
using Dapper;

public class SectorRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<Sector> GetAll()
    {
        string query = "SELECT * FROM Sector";
        return _uow.Connection.Query<Sector>(query);
    }

    public Sector? GetById(int id)
    {
        string query = "SELECT * FROM Sector WHERE Id = @Id";
        return _uow.Connection.QueryFirstOrDefault<Sector>(query, new { Id = id });
    }

    public void Insert(Sector sector)
    {
        string query = "INSERT INTO Sector (Name) VALUES (@Name)";
        _uow.Connection.Execute(query, sector);
    }

    public void Update(Sector sector)
    {
        string query = "UPDATE Sector SET Name = @Name WHERE Id = @Id";
        _uow.Connection.Execute(query, sector);
    }

    public void Delete(int id)
    {
        string query = "DELETE FROM Sector WHERE Id = @Id";
        _uow.Connection.Execute(query, new { Id = id });
    }
}
