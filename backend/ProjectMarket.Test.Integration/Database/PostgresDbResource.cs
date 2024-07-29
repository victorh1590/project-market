using ProjectMarket.Test.Integration.Database.Interfaces;
using Testcontainers.PostgreSql;

namespace ProjectMarket.Test.Integration.Database;

public class PostgresDbResource : IPostgresDbResource
{
    public PostgreSqlContainer PostgreSqlContainer { get; }

    internal PostgresDbResource() => PostgreSqlContainer = new PostgreSqlBuilder().Build();

    public async ValueTask DisposeAsync() => await PostgreSqlContainer.DisposeAsync();

    public async ValueTask StartAsync() => await PostgreSqlContainer.StartAsync();
}