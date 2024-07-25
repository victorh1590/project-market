using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FluentMigrator;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectMarket.Server.Infra.Db;
using ProjectMarket.Server.Infra.Repository;
using Testcontainers.PostgreSql;
using Xunit;
using Xunit.Abstractions;

namespace ProjectMarket.Test.Integration
{
    [Migration(1)]
    public class CreateInitialTable : Migration
    {
        public override void Up()
        {
            Create.Table("Items")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString(255).NotNullable();

            Insert.IntoTable("Items").Row(new { Id = 1, Name = "Something" });
        }

        public override void Down()
        {
            Delete.Table("Items");
        }
    }

    public class CurrencyRepositoryTest : IAsyncLifetime
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly PostgreSqlContainer _postgreSqlContainer;
        private IConfiguration _configuration;

        public CurrencyRepositoryTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _postgreSqlContainer = new PostgreSqlBuilder().Build();
        }

        public async Task InitializeAsync()
        {
            await _postgreSqlContainer.StartAsync();
            
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["CONNECTIONSTRING__POSTGRESQL"] = _postgreSqlContainer.GetConnectionString()
            });
            _configuration = builder.Build();
            
            // Migrate the database
            var serviceProvider = CreateServices(_configuration);
            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.ListMigrations(); // List all migrations to see if they are recognized
                runner.MigrateUp(1); // Run the migration
            }
        }

        public Task DisposeAsync()
        {
            return _postgreSqlContainer.DisposeAsync().AsTask();
        }

        private static ServiceProvider CreateServices(IConfiguration configuration)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(configuration["CONNECTIONSTRING__POSTGRESQL"])
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        [Fact]
        public void ExecuteCommand()
        {
            _testOutputHelper.WriteLine(_configuration["CONNECTIONSTRING__POSTGRESQL"]);
            using (var unitOfWork = new UnitOfWork(_configuration)) {
                var currencyRepository = new CurrencyRepository(unitOfWork);
                currencyRepository.UnitOfWork.Begin();

                var command = currencyRepository.UnitOfWork.Connection.CreateCommand();
                command.CommandText = "SELECT * FROM Items";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _testOutputHelper.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
                    }
                }
            }
        }
    }
}