using Dapper;
using ProjectMarket.Server.Data.Model.VO;

namespace ProjectMarket.Server.Infra.Repository;

public class AdvertisementStatusRepository(IUnitOfWork uow)
{
    private readonly IUnitOfWork _uow = uow;

    public IEnumerable<AdvertisementStatusVO> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT * FROM AdvertisementStatus";
        return _uow.Connection.Query<AdvertisementStatusVO>(query);
    }

    public AdvertisementStatusVO? GetByAdvertisementStatusName(string name)
    {
        string query = "SELECT * FROM AdvertisementStatus WHERE AdvertisementStatusName = @AdvertisementStatusName";
        return _uow.Connection.QueryFirstOrDefault<AdvertisementStatusVO>(query, new { AdvertisementStatusName = name });
    }

    public void Insert(AdvertisementStatusVO AdvertisementStatus)
    {
        string query = 
            "INSERT INTO AdvertisementStatus (AdvertisementStatusName) VALUES (@AdvertisementStatusName)";

        _uow.Connection.Execute(query, AdvertisementStatus);
    }

    public void Update(AdvertisementStatusVO AdvertisementStatus)
    {
        string query = 
            "UPDATE AdvertisementStatus " + 
            "SET AdvertisementStatusName = @AdvertisementStatusName" +
            "WHERE AdvertisementStatusName = @AdvertisementStatusName";
        _uow.Connection.Execute(query, AdvertisementStatus);
    }

    public void Delete(AdvertisementStatusVO AdvertisementStatus)
    {
        string query = "DELETE CASCADE FROM AdvertisementStatus WHERE AdvertisementStatusName = @AdvertisementStatusName";
        _uow.Connection.Execute(query, AdvertisementStatus);
    }
}
