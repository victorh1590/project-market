using Dapper;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Infra.Repository;

public class AdvertisementStatusRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<AdvertisementStatus> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT * FROM AdvertisementStatus";
        return _uow.Connection.Query<AdvertisementStatus>(query);
    }

    public AdvertisementStatus? GetByAdvertisementStatusName(string name)
    {
        string query = "SELECT * FROM AdvertisementStatus WHERE AdvertisementStatusName = @AdvertisementStatusName";
        return _uow.Connection.QueryFirstOrDefault<AdvertisementStatus>(query, new { AdvertisementStatusName = name });
    }

    public void Insert(AdvertisementStatus AdvertisementStatus)
    {
        string query = 
            "INSERT INTO AdvertisementStatus (AdvertisementStatusName) VALUES (@AdvertisementStatusName)";

        _uow.Connection.Execute(query, AdvertisementStatus);
    }

    public void Update(AdvertisementStatus AdvertisementStatus)
    {
        string query = 
            "UPDATE AdvertisementStatus " + 
            "SET AdvertisementStatusName = @AdvertisementStatusName" +
            "WHERE AdvertisementStatusName = @AdvertisementStatusName";
        _uow.Connection.Execute(query, AdvertisementStatus);
    }

    public void Delete(AdvertisementStatus AdvertisementStatus)
    {
        string query = "DELETE CASCADE FROM AdvertisementStatus WHERE AdvertisementStatusName = @AdvertisementStatusName";
        _uow.Connection.Execute(query, AdvertisementStatus);
    }
}
