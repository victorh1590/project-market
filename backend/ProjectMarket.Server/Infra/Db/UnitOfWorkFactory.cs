using System.Data.Common;
using Npgsql;
using ProjectMarket.Server.Infra.DependencyInjection;

namespace ProjectMarket.Server.Infra.Db;

public class UnitOfWorkFactory(IConfiguration configuration, DbmsName dbmsName)
{
    private DbmsName DbmsName { get; } = dbmsName;
    private IConfiguration Configuration { get; } = configuration;
    public UnitOfWork CreateUnitOfWork()
    {
        DbConnection connection = DbmsName switch
        {
            DbmsName.POSTGRESQL => new NpgsqlConnection(Configuration[DbmsName.GetConnectionStringName()]),
            _ => throw new InvalidDataException($"Failed to create connection, driver not found for database called {DbmsName}.")
        };

        return new UnitOfWork(connection);
    }
}