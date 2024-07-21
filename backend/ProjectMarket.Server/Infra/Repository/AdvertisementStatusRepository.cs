using Dapper;
using ProjectMarket.Server.Data.Model.ValueObjects;

namespace ProjectMarket.Server.Infra.Repository;

public class AdvertisementStatusRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<AdvertisementStatusVo> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT AdvertisementStatusName FROM AdvertisementStatus";
        return _uow.Connection.Query<AdvertisementStatusVo>(query);
    }

    public AdvertisementStatusVo? GetByAdvertisementStatusName(string name)
    {
        string query = "SELECT AdvertisementStatusName FROM AdvertisementStatus WHERE AdvertisementStatusName = @AdvertisementStatusName";
        return _uow.Connection.QueryFirstOrDefault<AdvertisementStatusVo>(query, new { AdvertisementStatusName = name });
    }

    public void Insert(AdvertisementStatusVo AdvertisementStatus)
    {
        string query = 
            "INSERT INTO AdvertisementStatus (AdvertisementStatusName) VALUES (@AdvertisementStatusName)";

        _uow.Connection.Execute(query, AdvertisementStatus);
    }

    public void Update(AdvertisementStatusVo AdvertisementStatus)
    {
        string query = 
            "UPDATE AdvertisementStatus " + 
            "SET AdvertisementStatusName = @AdvertisementStatusName" +
            "WHERE AdvertisementStatusName = @AdvertisementStatusName";
        _uow.Connection.Execute(query, AdvertisementStatus);
    }

    public void Delete(AdvertisementStatusVo AdvertisementStatus)
    {
        string query = "DELETE CASCADE FROM AdvertisementStatus WHERE AdvertisementStatusName = @AdvertisementStatusName";
        _uow.Connection.Execute(query, AdvertisementStatus);
    }
}
