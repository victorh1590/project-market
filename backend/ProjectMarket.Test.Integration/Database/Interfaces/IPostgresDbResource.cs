using DotNet.Testcontainers.Containers;
using Testcontainers.PostgreSql;

namespace ProjectMarket.Test.Integration.Database.Interfaces;

public interface IPostgresDbResource: IDbResource
{
    PostgreSqlContainer PostgreSqlContainer { get; }
    IDatabaseContainer IDbResource.DatabaseContainer => PostgreSqlContainer;
}