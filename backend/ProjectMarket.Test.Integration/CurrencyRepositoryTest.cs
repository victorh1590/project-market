using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using ProjectMarket.Server.Infra.Db;
using ProjectMarket.Server.Infra.Repository;
using ProjectMarket.Test.Integration.Database;
using SqlKata;
using SqlKata.Compilers;
using ProjectMarket.Server.Infra.Migrations;

namespace ProjectMarket.Test.Integration;

[TestFixture]
public class CurrencyRepositoryTest
{
    private PostgresService _postgresService;
    private UnitOfWorkFactory _unitOfWorkFactory;
    private PostgresCompiler _compiler = new();
    private CurrencyRepository _repository;

    [OneTimeSetUp]
    public async Task OneTimeSetUpAsync()
    {
        _postgresService = await PostgresServiceFactory.CreateServiceAsync() ?? throw new InvalidOperationException();

        const DbmsName databaseName = DbmsName.POSTGRESQL;
        var builder = new ConfigurationBuilder();
        var configuration = 
            builder.AddInMemoryCollection(new Dictionary<string, string?> 
                {
                    [$"CONNECTIONSTRING__{databaseName.ToString().ToUpperInvariant()}"] = _postgresService.Migration.ConnectionString
                }).Build();

        _unitOfWorkFactory = new UnitOfWorkFactory(configuration, databaseName);
        _postgresService.Migration.RebuildMigrationProvider( typeof(_1_CreateVOTables).Assembly );
        _postgresService.Migration.ExecuteMigration(1);
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDownAsync()
    {
        await _postgresService.Database.DisposeAsync();
    }

    [SetUp]
    public void SetUp()
    {
        var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork();
        _repository = new CurrencyRepository(unitOfWork, _compiler);
        _repository.UnitOfWork.Begin();
    }

    [TearDown]
    public void TearDown()
    {
        _repository.UnitOfWork.Dispose();
    }
    
    [NUnit.Framework.Ignore(reason: "This is just an example")]
    [Test]
    public void ExecuteCommand()
    {
        TestContext.WriteLine(_postgresService.ConnectionString);
        var compiler = new PostgresCompiler();
        var query = compiler.Compile(new Query("Items").Select("*"));
        TestContext.WriteLine(query.ToString());
        var command = _repository.UnitOfWork.Connection.CreateCommand();
        command.CommandText = query.Sql;
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            TestContext.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
        }
    }
    
    [NUnit.Framework.Ignore(reason: "Not Implemented")]
    [Test(Description = "Repository should insert currency row without failling")]
    public void InsertCurrencyTest()
    {
        var query = _compiler.Compile(new Query("Items").Select("*"));
        TestContext.WriteLine(query.ToString());
        // _repository.Insert();
    }
    
    [Test(Description = "Repository should return all rows")]
    public void GetAllTest()
    {
        var result = _repository.GetAll();
        var json = JsonConvert.SerializeObject(result);
        TestContext.WriteLine(json);
    }
}
