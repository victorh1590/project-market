using Microsoft.Extensions.Configuration;
using ProjectMarket.Server.Infra.Db;
using ProjectMarket.Server.Infra.Repository;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ProjectMarket.Test.Integration
{
    public class CurrencyRepositoryTest : IAsyncLifetime
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private PostgresService? _postgresService;
        
        public CurrencyRepositoryTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public async Task InitializeAsync()
        {
            _postgresService = await PostgresServiceFactory.CreateServiceAsync();
            _postgresService?.Migration.ExecuteAllMigrations();
        }

        public async Task DisposeAsync()
        {
            if (_postgresService != null)
            {
                await _postgresService.Database.DisposeAsync();
            }
        }

        // public Task DisposeAsync()
        // {
        //     // return _postgreSqlContainer.DisposeAsync().AsTask();
        // }

       

        [Fact]
        public void ExecuteCommand()
        {

            
            if (_postgresService == null) return;
            ConfigurationBuilder builder = new ConfigurationBuilder();
            var configuration = 
                builder.AddInMemoryCollection(
                    new Dictionary<string, string?> 
                    {
                        ["CONNECTIONSTRING__POSTGRESQL"] = _postgresService.Migration.ConnectionString
                    }
                )
                .Build();
            
            _testOutputHelper.WriteLine(_postgresService.ConnectionString);
            _testOutputHelper.WriteLine(nameof(_postgresService.ConnectionString));
            using var unitOfWork = new UnitOfWork(configuration);
            var currencyRepository = new CurrencyRepository(unitOfWork);
            currencyRepository.UnitOfWork.Begin();

            var command = currencyRepository.UnitOfWork.Connection.CreateCommand();
            command.CommandText = "SELECT * FROM public.\"Items\"";
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