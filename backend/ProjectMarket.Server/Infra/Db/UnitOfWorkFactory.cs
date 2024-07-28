using System.Data.Common;
using Npgsql;

namespace ProjectMarket.Server.Infra.Db;

public class UnitOfWorkFactory
{
    private DbmsName DbmsName { get; }
    private IConfiguration Configuration { get; }
    
    public UnitOfWorkFactory(IConfiguration configuration, DbmsName dbmsName)
    {
        DbmsName = dbmsName;
        Configuration = configuration;
    }

    public UnitOfWork CreateUnitOfWork()
    {
        DbConnection connection = DbmsName switch
        {
            DbmsName.POSTGRESQL => 
                new NpgsqlConnection(Configuration[$"CONNECTIONSTRING__{DbmsName.ToString().ToUpperInvariant()}"]),
            _ =>
                throw new InvalidDataException($"Failed to create connection, driver not found for database called {DbmsName}.")
        };

        return new UnitOfWork(Configuration, connection);
    }
}