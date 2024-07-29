using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Extensions;
using NUnit.Framework;
using ProjectMarket.Server.Infra.Db;
using ProjectMarket.Server.Infra.DependencyInjection;
using ProjectMarket.Test.Integration.Database.Interfaces;

namespace ProjectMarket.Test.Integration.Database;

public class PostgresServiceFactory
{
    private const DbmsName DbmsName = Server.Infra.Db.DbmsName.POSTGRESQL;
    
    public static async Task<PostgresService?> CreateServiceAsync()
    {
        IPostgresDbResource postrgresDbResource = new PostgresDbResource();
        await postrgresDbResource.StartAsync();
        String connectionString = postrgresDbResource.PostgreSqlContainer.GetConnectionString();
        TestContext.WriteLine(connectionString);
        IMigration postgresMigration = new PostgresMigration(connectionString);
        IConfiguration configuration = GenerateTestConfiguration(DbmsName, connectionString);
        return new PostgresService(postrgresDbResource, postgresMigration, configuration, DbmsName);
    }
    
    public static PostgresService CreateService()
    {
        IPostgresDbResource postrgresDbResource = new PostgresDbResource();
        postrgresDbResource.StartAsync().AsTask().Wait();
        String connectionString = postrgresDbResource.PostgreSqlContainer.GetConnectionString();
        IMigration postgresMigration = new PostgresMigration(connectionString);
        const DbmsName dbmsName = DbmsName.POSTGRESQL;
        IConfiguration configuration = GenerateTestConfiguration(dbmsName, connectionString);
        return new PostgresService(postrgresDbResource, postgresMigration, configuration, dbmsName);
    }

    private static IConfiguration GenerateTestConfiguration(DbmsName dbmsName, String connectionString)
    {
        var builder = new ConfigurationBuilder();
        return builder.AddInMemoryCollection(new Dictionary<string, string?> 
            {
                [dbmsName.GetConnectionStringName()] = connectionString
            }).Build();
    }
}