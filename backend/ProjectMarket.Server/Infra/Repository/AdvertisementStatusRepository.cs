using Dapper;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Db;
using SqlKata.Compilers;

namespace ProjectMarket.Server.Infra.Repository;

public class AdvertisementStatusRepository(IUnitOfWork unitOfWork, Compiler compiler)
{
    public readonly IUnitOfWork UnitOfWork = unitOfWork;

    public IEnumerable<AdvertisementStatusVo> GetAll()
    {
        // TODO Use pagination instead.
        string query = "SELECT \"AdvertisementStatusName\" " +
                       "FROM \"AdvertisementStatus\"";
        return UnitOfWork.Connection.Query<AdvertisementStatusVo>(query);
    }

    public AdvertisementStatusVo GetAdvertisementStatusByName(string name)
    {
        string query = "SELECT \"AdvertisementStatusName\" " +
                       "FROM \"AdvertisementStatus\" " +
                       "WHERE \"AdvertisementStatusName\" = @AdvertisementStatusName";
        try
        {
            var record = UnitOfWork.Connection.QuerySingle<AdvertisementStatusRecord>(query, new { AdvertisementStatusName = name });
            AdvertisementStatusVo result = new(record);
            return result;
        }
        catch (Exception)
        {
            throw new ArgumentException($"{nameof(AdvertisementStatusVo.AdvertisementStatusName)} \'{name}\' not found");
        }
    }

    public AdvertisementStatusVo Insert(AdvertisementStatusVo advertisementStatus)
    {
        string query = 
            "INSERT INTO \"AdvertisementStatus\" (\"AdvertisementStatusName\") " +
            "VALUES (@AdvertisementStatusName) " +
            "RETURNING \"AdvertisementStatus\"";

        return UnitOfWork.Connection.QuerySingle<AdvertisementStatusVo>(query, advertisementStatus);
    }

    public bool Update(string name, AdvertisementStatusVo advertisementStatus)
    {
        string query = 
            "UPDATE \"AdvertisementStatus\" " + 
            "SET \"AdvertisementStatusName\" = @AdvertisementStatusName" +
            "WHERE \"AdvertisementStatusName\" = @AdvertisementStatusNameToUpdate " +
            "RETURNING true";
        return UnitOfWork.Connection.QuerySingle<bool>(query, new
        {
            AdvertisementStatusNameToUpdate = name,
            AdvertisementStatusName = advertisementStatus.AdvertisementStatusName
        });
    }

    public bool Delete(AdvertisementStatusVo advertisementStatus)
    {
        string query = "DELETE FROM AdvertisementStatus CASCADE " +
                       "WHERE \"AdvertisementStatusName\" = @AdvertisementStatusName " +
                       "RETURNING true";
        return UnitOfWork.Connection.QuerySingle<bool>(query, advertisementStatus);
    }
}
