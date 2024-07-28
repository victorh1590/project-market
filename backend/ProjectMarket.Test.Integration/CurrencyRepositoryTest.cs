using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using ProjectMarket.Server.Infra.Db;
using ProjectMarket.Server.Infra.Repository;
using ProjectMarket.Test.Integration.Database;
using SqlKata;
using SqlKata.Compilers;

namespace ProjectMarket.Test.Integration;

[TestFixture]
public class CurrencyRepositoryTest
{
    private PostgresService _postgresService;
    private UnitOfWorkFactory _unitOfWorkFactory;
    
    [OneTimeSetUp]
    public async Task InitializeAsync()
    {
        _postgresService = await PostgresServiceFactory.CreateServiceAsync() ?? throw new InvalidOperationException();
        _postgresService.Migration.ExecuteAllMigrations();

        const DbmsName databaseName = DbmsName.POSTGRESQL;
        var builder = new ConfigurationBuilder();
        var configuration = 
            builder.AddInMemoryCollection(new Dictionary<string, string?> 
                {
                    [$"CONNECTIONSTRING__{databaseName.ToString().ToUpperInvariant()}"] = _postgresService.Migration.ConnectionString
                }).Build();

        _unitOfWorkFactory = new UnitOfWorkFactory(configuration, databaseName);
    }

    [OneTimeTearDown]
    public async Task DisposeAsync()
    {
        await _postgresService.Database.DisposeAsync();
    }

    [Test]
    public void ExecuteCommand()
    {
        TestContext.WriteLine(_postgresService.ConnectionString);
        using var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork();
        var currencyRepository = new CurrencyRepository(unitOfWork);
        currencyRepository.UnitOfWork.Begin();
        var compiler = new PostgresCompiler();
        var query = compiler.Compile(new Query("Items").Select("*"));
        TestContext.WriteLine(query.ToString());
        var command = currencyRepository.UnitOfWork.Connection.CreateCommand();
        command.CommandText = query.Sql;
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            TestContext.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
        }
    }
}
