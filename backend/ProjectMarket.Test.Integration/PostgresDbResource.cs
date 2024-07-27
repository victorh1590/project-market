using Testcontainers.PostgreSql;

namespace ProjectMarket.Test.Integration;

public class PostgresDbResource : IPostgresDbResource
{
    public PostgreSqlContainer PostgreSqlContainer { get; }

    internal PostgresDbResource()
    {
        PostgreSqlContainer = new PostgreSqlBuilder().Build();
    }

    public async Task InitializeAsync()
    {
        // Ensure the PostgreSQL container is started
        await PostgreSqlContainer.StartAsync();
    }
    
    public async Task DisposeAsync()
    {
        await PostgreSqlContainer.DisposeAsync();
    }
}