using Microsoft.Extensions.Configuration;
using ProjectMarket.Server.Infra.Db;
using ProjectMarket.Server.Infra.DependencyInjection;
using ProjectMarket.Test.Integration.Database.Interfaces;

namespace ProjectMarket.Test.Integration.Database;

public class PostgresService
{
    public IDbResource Database { get; }
    public IMigration Migration { get; }
    public IConfiguration Configuration { get; }
    public DbmsName DbmsName { get; init; }
    public String ConnectionString { get; init; }
    internal PostgresService(IDbResource resource, IMigration migration, IConfiguration configuration, DbmsName dbmsName)
    {
        Database = resource;
        Migration = migration;
        Configuration = configuration;
        DbmsName = dbmsName;
        ConnectionString = Configuration[DbmsName.GetConnectionStringName()] 
            ?? throw new KeyNotFoundException($"{nameof(ConnectionString)} not found in configuration for {DbmsName}");
    }
}