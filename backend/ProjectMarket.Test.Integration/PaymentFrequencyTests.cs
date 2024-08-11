using System.Reflection;
using Dapper;
using DbUp;
using Newtonsoft.Json;
using NUnit.Framework;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Db;
using ProjectMarket.Server.Infra.Migrations;
using ProjectMarket.Server.Infra.Repository;
using ProjectMarket.Test.Integration.Database;
using SqlKata.Compilers;

namespace ProjectMarket.Test.Integration;

public class PaymentFrequencyTests
{
    private PostgresService _postgresService;
    private UnitOfWorkFactory _unitOfWorkFactory;
    private readonly PostgresCompiler _compiler = new();
    private PaymentFrequencyRepository _repository;

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
        _repository = new PaymentFrequencyRepository(unitOfWork, _compiler);
        _repository.UnitOfWork.Begin();
    }

    [TearDown]
    public void TearDown()
    {
        _repository.UnitOfWork.Dispose();
    }
    
    [Order(1)]
    [Test(Description = "Repository should return all rows")]
    public void GetAllTest()
    {        
        var expectedObj = new List<PaymentFrequencyVo>
        {
            new() { PaymentFrequencyName = "Monthly", Suffix = "a month" },
            new() { PaymentFrequencyName = "Hourly", Suffix = "per hour" },
            new() { PaymentFrequencyName = "Daily", Suffix = "per day" },
            new() { PaymentFrequencyName = "Once", Suffix = "when project is done" }
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
        PaymentFrequencyVo toInsert = new() { PaymentFrequencyName = "Per Task", Suffix = "when task is completed" };
        var expectedJson = JsonConvert.SerializeObject(toInsert, Formatting.Indented);
        var expectedAllObj = new List<PaymentFrequencyVo>
        {
            new() { PaymentFrequencyName = "Monthly", Suffix = "a month" },
            new() { PaymentFrequencyName = "Hourly", Suffix = "per hour" },
            new() { PaymentFrequencyName = "Daily", Suffix = "per day" },
            new() { PaymentFrequencyName = "Once", Suffix = "when project is done" }
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
        PaymentFrequencyVo toRemove = new() { PaymentFrequencyName = "Monthly", Suffix = "a month" };

        var expectedAllObj = new List<PaymentFrequencyVo>
        {
            new() { PaymentFrequencyName = "Hourly", Suffix = "per hour" },
            new() { PaymentFrequencyName = "Daily", Suffix = "per day" },
            new() { PaymentFrequencyName = "Once", Suffix = "when project is done" },
            new() { PaymentFrequencyName = "Per Task", Suffix = "when task is completed" }
        };
        var resultObj = _repository.Delete(toRemove.PaymentFrequencyName);
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
        const string toSearch = "Once";
        var expectedObj = new PaymentFrequencyVo() { PaymentFrequencyName = "Once" };

        var resultObj = _repository.GetPaymentFrequencyByName(toSearch);

        var resultJson = JsonConvert.SerializeObject(resultObj, Formatting.Indented);
        TestContext.WriteLine($"Get By Name Returned: {resultJson}");
        Assert.That(resultObj, Is.EqualTo(expectedObj));
    }
    
    [Order(5)]
    [Test(Description = "Repository should update specified row")]
    public void UpdateTest()
    {
        const string toUpdate = "Daily";
        var update = new PaymentFrequencyVo() { PaymentFrequencyName = "Monthly", Suffix = "each month" };
        
        var expectedAllObj = new List<PaymentFrequencyVo>
        {
            new() { PaymentFrequencyName = "Hourly", Suffix = "per hour" },
            new() { PaymentFrequencyName = "Monthly", Suffix = "each month" },
            new() { PaymentFrequencyName = "Once", Suffix = "when project is done" },
            new() { PaymentFrequencyName = "Per Task", Suffix = "when task is completed" }
        };

        var resultObj = _repository.Update(toUpdate, update);
        _repository.UnitOfWork.Commit();

        TestContext.WriteLine($"Update Returned: {resultObj}");
        Assert.That(resultObj, Is.EqualTo(true));
        
        var resultAllObj = _repository.GetAll().AsList();
        var resultAllJson = JsonConvert.SerializeObject(resultAllObj, Formatting.Indented);
        TestContext.WriteLine(resultAllJson);
        Assert.That(resultAllObj, Is.EqualTo(expectedAllObj).AsCollection);
    }
    
    [Order(6)]
    [Test(Description = "Repository should throw Exception when row doesn't exist")]
    public void GetCurrencyByNameFailWithExceptionTest()
    {
        const string toSearch = "Flexible";
        Assert.That(() => _repository.GetPaymentFrequencyByName(toSearch), Throws.ArgumentException);
    } 
}