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

[TestFixture]
public class PaymentFrequencyRepositoryTests
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
        
        string scriptSuffix = "_SeedData.sql";
        DeployChanges.To
            .PostgresqlDatabase(_postgresService.ConnectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), 
                s => s.Contains(GetType().Name + scriptSuffix, StringComparison.OrdinalIgnoreCase))
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
        var expectedAllObj = new List<PaymentFrequencyVo>
        {
            new() { PaymentFrequencyName = "Monthly", Suffix = "a month" },
            new() { PaymentFrequencyName = "Hourly", Suffix = "per hour" },
            new() { PaymentFrequencyName = "Daily", Suffix = "per day" },
            new() { PaymentFrequencyName = "Once", Suffix = "when project is done" }
        };
        var resultAllObj = _repository.GetAll();

        Assert.That(resultAllObj, Is.EqualTo(expectedAllObj));
    }
    
    [Order(2)]
    [Test(Description = "Repository should insert specified rows")]
    public void InsertTest()
    {
        PaymentFrequencyVo toInsert = new() { PaymentFrequencyName = "Per Task", Suffix = "when task is completed" };
        var expectedAllObj = new List<PaymentFrequencyVo>
        {
            new() { PaymentFrequencyName = "Monthly", Suffix = "a month" },
            new() { PaymentFrequencyName = "Hourly", Suffix = "per hour" },
            new() { PaymentFrequencyName = "Daily", Suffix = "per day" },
            new() { PaymentFrequencyName = "Once", Suffix = "when project is done" },
            new() { PaymentFrequencyName = "Per Task", Suffix = "when task is completed" }
        };
        var resultObj = _repository.Insert(toInsert);
        _repository.UnitOfWork.Commit();
        var resultAllObj = _repository.GetAll();

        Assert.Multiple(() =>
        {
            Assert.That(resultObj, Is.EqualTo(toInsert));
            Assert.That(resultAllObj, Is.EqualTo(expectedAllObj).AsCollection);
        });
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
        var resultAllObj = _repository.GetAll().AsList();

        Assert.Multiple(() =>
        {
            Assert.That(resultObj, Is.EqualTo(true));
            Assert.That(resultAllObj, Is.EqualTo(expectedAllObj).AsCollection);
        });
    }
        
        [Order(4)]
        [Test(Description = "Repository should return specified row")]
        public void GetPaymentFrequencyByNameTest()
        {
            const string toSearch = "Once";
            var expectedObj = new PaymentFrequencyVo() { PaymentFrequencyName = "Once", Suffix = "when project is done" };
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
            new() { PaymentFrequencyName = "Once", Suffix = "when project is done" },
            new() { PaymentFrequencyName = "Per Task", Suffix = "when task is completed" },
            new() { PaymentFrequencyName = "Monthly", Suffix = "each month" }
        };
        var resultObj = _repository.Update(toUpdate, update);
        _repository.UnitOfWork.Commit();
        TestContext.WriteLine($"Update Returned: {resultObj}");
        var resultAllObj = _repository.GetAll().AsList();
        
        Assert.Multiple(() =>
        {
            Assert.That(resultObj, Is.EqualTo(true));
            Assert.That(resultAllObj, Is.EqualTo(expectedAllObj).AsCollection);
        });
    }
    
    [Order(6)]
    [Test(Description = "Repository should throw Exception when row doesn't exist")]
    public void GetPaymentFrequencyByNameFailWithExceptionTest()
    {
        const string toSearch = "Flexible";
        Assert.That(() => _repository.GetPaymentFrequencyByName(toSearch), Throws.ArgumentException);
    } 
}