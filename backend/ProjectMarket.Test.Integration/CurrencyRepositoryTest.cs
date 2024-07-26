// log: dotnet test --logger "console;verbosity=detailed"

using System.Reflection;
using System.Text;
using DbUp;
using FluentMigrator;
using Microsoft.Extensions.Configuration;
using ProjectMarket.Server.Infra.Db;
using ProjectMarket.Server.Infra.Repository;
using Testcontainers.PostgreSql;
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
        private IConfiguration? _configuration;

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

            var upgradeEngine = DeployChanges.To
                .PostgresqlDatabase(_configuration["CONNECTIONSTRING__POSTGRESQL"])
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), s => s.EndsWith(".sql"), Encoding.UTF8)
                .WithTransactionPerScript()
                .LogToConsole()
                .Build();

            var result = upgradeEngine.PerformUpgrade();

            if (!result.Successful)
            {
                Console.WriteLine(result.Error);
                throw new Exception("Database upgrade failed", result.Error);
            }
        }

        public Task DisposeAsync()
        {
            return _postgreSqlContainer.DisposeAsync().AsTask();
        }

        [Fact]
        public async Task ExecuteCommand()
        {
            await InitializeAsync(); // Ensure database setup is completed

            _testOutputHelper.WriteLine(_configuration?["CONNECTIONSTRING__POSTGRESQL"]);

            using (var unitOfWork = new UnitOfWork(_configuration))
            {
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