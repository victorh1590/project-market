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
        const string sql = """
                           SELECT "AdvertisementStatusName" 
                           FROM "AdvertisementStatus"
                           """;
        return UnitOfWork.Connection.Query<AdvertisementStatusVo>(sql);
    }

    public AdvertisementStatusVo GetAdvertisementStatusByName(string name)
    {
        const string sql = """
                           SELECT "AdvertisementStatusName" 
                           FROM "AdvertisementStatus" 
                           WHERE "AdvertisementStatusName" = @AdvertisementStatusName
                           """;
        try
        {
            var record = UnitOfWork.Connection.QuerySingle<AdvertisementStatusRecord>(sql, new { AdvertisementStatusName = name });
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
        const string sql = """
                           INSERT INTO "AdvertisementStatus" ("AdvertisementStatusName") 
                           VALUES (@AdvertisementStatusName) 
                           RETURNING "AdvertisementStatusName"
                           """;

        return UnitOfWork.Connection.QuerySingle<AdvertisementStatusVo>(sql, advertisementStatus);
    }

    public bool Update(string name, AdvertisementStatusVo advertisementStatus)
    {
        const string sql = """
                           UPDATE "AdvertisementStatus" 
                           SET "AdvertisementStatusName" = @AdvertisementStatusName 
                           WHERE "AdvertisementStatusName" = @AdvertisementStatusNameToUpdate 
                           RETURNING true
                           """;
        return UnitOfWork.Connection.QuerySingle<bool>(sql, new
        {
            AdvertisementStatusNameToUpdate = name,
            AdvertisementStatusName = advertisementStatus.AdvertisementStatusName
        });
    }

    public bool Delete(string name)
    {
        const string sql = """
                           DELETE FROM "AdvertisementStatus" CASCADE 
                           WHERE "AdvertisementStatusName" = @AdvertisementStatusName 
                           RETURNING true
                           """;
        return UnitOfWork.Connection.QuerySingle<bool>(sql, new { AdvertisementStatusName = name });
    }
}