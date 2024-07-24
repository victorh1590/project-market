using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ProjectMarket.Server.Infra.Db;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;

namespace ProjectMarket.Test.Integration;

public class CurrencyRepositoryTest : IAsyncLifetime
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();
    
    public CurrencyRepositoryTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    public Task InitializeAsync()
    {
        return _postgreSqlContainer.StartAsync();
    }
    
    public Task DisposeAsync()
    {
        return _postgreSqlContainer.DisposeAsync().AsTask();
    }

    [Fact]
    public void ExecuteCommand()
    {
        ConfigurationBuilder builder = new ConfigurationBuilder();
        builder.Properties.Add("CONNECTIONSTRING__POSTGRESQ", _postgreSqlContainer.GetConnectionString());
        IConfiguration configuration = builder.Build();
        UnitOfWork unitOfWork = new UnitOfWork(configuration);
        
        _testOutputHelper.WriteLine(configuration["CONNECTIONSTRING__POSTGRESQ"]);
        
        // using (DbConnection connection = new NpgsqlConnection(_postgreSqlContainer.GetConnectionString()))
        // {
        //     using (DbCommand command = new NpgsqlCommand())
        //     {
        //         connection.Open();
        //         command.Connection = connection;
        //         command.CommandText = "SELECT 1";
        //     }
        // }
    }
}