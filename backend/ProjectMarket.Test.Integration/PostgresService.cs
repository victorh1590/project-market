using Microsoft.Extensions.Configuration;

namespace ProjectMarket.Test.Integration;

public class PostgresService
{
// "CONNECTIONSTRING__POSTGRESQL";
    public IDbResource Database { get; init; }
    public IMigration Migration { get; init; }
    public IConfiguration Configuration { get; init; }
    public String ConnectionString => Configuration[nameof(ConnectionString)] ?? string.Empty;
    internal PostgresService(IDbResource resource, IMigration migration)
    {
        Database = resource;
        Migration = migration;
        var builder = new ConfigurationBuilder();
        Configuration = builder
            .AddInMemoryCollection(
                new Dictionary<string, string?> 
                {
                    [nameof(ConnectionString)] = Migration.ConnectionString
                }
            )
            .Build();
    }
}