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

    public AdvertisementStatus? GetByAdvertisementStatusId(int id)
    {
        string query = "SELECT * FROM AdvertisementStatus WHERE AdvertisementStatusId = @AdvertisementStatusId";
        return _uow.Connection.QueryFirstOrDefault<AdvertisementStatus>(query, new { AdvertisementStatusId = id });
    }

    public void Insert(AdvertisementStatus AdvertisementStatus)
    {
        string query = 
            "INSERT INTO AdvertisementStatus (Status) VALUES (@Status)";

        _uow.Connection.Execute(query,AdvertisementStatus);
    }

    public void Update(AdvertisementStatus AdvertisementStatus)
    {
        string query = 
            "UPDATE AdvertisementStatus " + 
            "SET Status = @Status" +
            "WHERE AdvertisementStatusId = @AdvertisementStatusId";
        _uow.Connection.Execute(query, AdvertisementStatus);
    }

    public void Delete(AdvertisementStatus AdvertisementStatus)
    {
        string query = "DELETE CASCADE FROM AdvertisementStatus WHERE AdvertisementStatusId = @AdvertisementStatusId";
        _uow.Connection.Execute(query, AdvertisementStatus);
    }
}
