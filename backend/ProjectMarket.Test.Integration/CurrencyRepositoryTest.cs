using System.Reflection;
using Dapper;
using DbUp;
using FluentValidation;
using Newtonsoft.Json;
using NUnit.Framework;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Data.Validators;
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
    private readonly PostgresCompiler _compiler = new();
    private CurrencyRepository _repository;

    [OneTimeSetUp]
    public async Task OneTimeSetUpAsync()
    {
        _postgresService = await PostgresServiceFactory.CreateServiceAsync() ?? throw new InvalidOperationException();
        _unitOfWorkFactory = new UnitOfWorkFactory(_postgresService.Configuration, _postgresService.DbmsName);
        _postgresService.Migration.RebuildMigrationProvider( typeof(_1_CreateVOTables).Assembly );
        _postgresService.Migration.ExecuteMigration(1);
        
        DeployChanges.To
            .PostgresqlDatabase(_postgresService.ConnectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build()
            .PerformUpgrade();
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
    
    [Order(1)]
    [Test(Description = "Repository should return all rows")]
    public void GetAllTest()
    {        
        var expectedObj = new List<CurrencyVo>
        {
            new() { CurrencyName = "Dollar", Prefix = "$"},
            new() { CurrencyName = "Euro", Prefix = "€" },
            new() { CurrencyName = "Yen", Prefix = "¥" }
        };
        var expectedJson = JsonConvert.SerializeObject(expectedObj, Formatting.Indented);
        
        var resultObj = _repository.GetAll();
        var resultJson = JsonConvert.SerializeObject(resultObj, Formatting.Indented);
        TestContext.WriteLine(resultJson);

        Assert.That(resultJson, Is.EqualTo(expectedJson));
    }
    
    [Order(2)]
    [Test(Description = "Repository should insert specified rows")]
    public void InsertTest()
    {
        CurrencyVo toInsert = new() { CurrencyName = "Rupee", Prefix = "₹" };
        var expectedJson = JsonConvert.SerializeObject(toInsert, Formatting.Indented);
        var expectedAllObj = new List<CurrencyVo>
        {
            new() { CurrencyName = "Dollar", Prefix = "$"},
            new() { CurrencyName = "Euro", Prefix = "€" },
            new() { CurrencyName = "Yen", Prefix = "¥" },
            new() { CurrencyName = "Rupee", Prefix = "₹"}
        };
        var expectedAllJson = JsonConvert.SerializeObject(expectedAllObj, Formatting.Indented);

        var resultObj = _repository.Insert(toInsert);
        _repository.UnitOfWork.Commit();
        
        var resultJson = JsonConvert.SerializeObject(resultObj, Formatting.Indented);
        TestContext.WriteLine(resultJson);

        Assert.That(resultJson, Is.EqualTo(expectedJson));

        var resultAllObj = _repository.GetAll();
        var resultAllJson = JsonConvert.SerializeObject(resultAllObj, Formatting.Indented);

        Assert.That(resultAllJson, Is.EqualTo(expectedAllJson));
    }
    
    [Order(3)]
    [Test(Description = "Repository should delete specified rows")]
    public void DeleteTest()
    {
        CurrencyVo toRemove = new() { CurrencyName = "Euro", Prefix = "€" };

        var expectedAllObj = new List<CurrencyVo>
        {
            new() { CurrencyName = "Dollar", Prefix = "$"},
            new() { CurrencyName = "Yen", Prefix = "¥" },
            new() { CurrencyName = "Rupee", Prefix = "₹"}
        };
        var resultObj = _repository.Delete(toRemove.CurrencyName);
        _repository.UnitOfWork.Commit();

        TestContext.WriteLine($"Delete returned: {resultObj}");
        Assert.That(resultObj, Is.EqualTo(true));

        var resultAllObj = _repository.GetAll().AsList();
        Assert.That(resultAllObj, Is.EqualTo(expectedAllObj).AsCollection);
    }
    
    [Order(4)]
    [Test(Description = "Repository should return specified row")]
    public void GetCurrencyByNameTest()
    {
        const string toSearch = "Rupee";
        var expectedObj = new CurrencyVo() { CurrencyName = "Rupee", Prefix = "₹" };

        var resultObj = _repository.GetCurrencyByName(toSearch);

        var resultJson = JsonConvert.SerializeObject(resultObj, Formatting.Indented);
        TestContext.WriteLine($"Get By Name Returned: {resultJson}");
        Assert.That(resultObj, Is.EqualTo(expectedObj));
    }
    
    [Order(5)]
    [Test(Description = "Repository should update specified row")]
    public void UpdateTest()
    {
        const string toUpdate = "Rupee";
        var update = new CurrencyVo() { CurrencyName = "Yuan", Prefix = "\u00a5" };
        
        var expectedAllObj = new List<CurrencyVo>
        {
            new() { CurrencyName = "Dollar", Prefix = "$"},
            new() { CurrencyName = "Yen", Prefix = "¥" },
            new() { CurrencyName = "Yuan", Prefix = "\u00a5" }
        };

        var resultObj = _repository.Update(toUpdate, update);
        _repository.UnitOfWork.Commit();

        TestContext.WriteLine($"Update Returned: {resultObj}");
        Assert.That(resultObj, Is.EqualTo(true));
        
        var resultAllObj = _repository.GetAll().AsList();
        Assert.That(resultAllObj, Is.EqualTo(expectedAllObj).AsCollection);
    }
    
    [Order(6)]
    [Test(Description = "Repository should return Exception when row don't exist")]
    public void GetCurrencyByNameFailWithExceptionTest()
    {
        const string toSearch = "Pounds";
        Assert.That(() => _repository.GetCurrencyByName(toSearch), Throws.ArgumentException);
    }
}
