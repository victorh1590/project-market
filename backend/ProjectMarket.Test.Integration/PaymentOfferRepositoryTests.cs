using System.Reflection;
using Dapper;
using DbUp;
using Newtonsoft.Json;
using NUnit.Framework;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Db;
using ProjectMarket.Server.Infra.Migrations;
using ProjectMarket.Server.Infra.Repository;
using ProjectMarket.Test.Integration.Database;
using SqlKata.Compilers;

namespace ProjectMarket.Test.Integration;

[TestFixture]
public class PaymentOfferRepositoryTests
{
    private PostgresService _postgresService;
    private UnitOfWorkFactory _unitOfWorkFactory;
    private readonly PostgresCompiler _compiler = new();
    private PaymentOfferRepository _repository;
    
    [OneTimeSetUp]
    public async Task OneTimeSetUpAsync()
    {
        _postgresService = await PostgresServiceFactory.CreateServiceAsync() ?? throw new InvalidOperationException();
        _unitOfWorkFactory = new UnitOfWorkFactory(_postgresService.Configuration, _postgresService.DbmsName);
        _postgresService.Migration.RebuildMigrationProvider( typeof(_1_CreateVOTables).Assembly );
        _postgresService.Migration.ExecuteMigration(3);
        
        const string scriptSuffix = "_SeedData.sql";
        DeployChanges.To
            .PostgresqlDatabase(_postgresService.ConnectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), 
                s => 
                    s.Contains(nameof(CurrencyRepositoryTests) + scriptSuffix, StringComparison.OrdinalIgnoreCase) ||
                    s.Contains(nameof(PaymentFrequencyRepositoryTests) + scriptSuffix, StringComparison.OrdinalIgnoreCase) ||
                    s.Contains(GetType().Name + scriptSuffix, StringComparison.OrdinalIgnoreCase))
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
        _repository = new PaymentOfferRepository(unitOfWork, _compiler);
        _repository.UnitOfWork.Begin();
    }

    [TearDown]
    public void TearDown()
    {
        _repository.UnitOfWork.Dispose();
    }
    
    // Vo's used for tests.
   private PaymentFrequencyVo Monthly { get; } = new ("Monthly", "a month");
   private PaymentFrequencyVo Hourly { get; } = new ("Hourly", "per hour");
   private PaymentFrequencyVo Daily { get; } = new ("Daily", "per day");
   private PaymentFrequencyVo Once { get; } = new ("Once", "when project is done");
   private CurrencyVo Dollar { get; } = new ("Dollar", "$");
   private CurrencyVo Euro { get; } = new ("Euro", "€");
   private CurrencyVo Yen { get; } = new ("Yen", "¥");
    
    [Order(1)]
    [Test(Description = "Repository should return all rows")]
    public void GetAllTest()
    {        
        var expectedAllObj = new List<PaymentOffer>
        {
            new(1, 2000, Monthly, Dollar),
            new(2, 25, Hourly, Euro),
            new(3, 30000, Daily, Yen),
            new(4, 15000, Once, Dollar),
            new(5, 3000, Monthly, Euro)
        };
        var resultAllObj = _repository.GetAll();
        
        Assert.That(resultAllObj, Is.EqualTo(expectedAllObj).AsCollection);
    }
    
    [Order(2)]
    [Test(Description = "Repository should insert specified rows")]
    public void InsertTest()
    {
        PaymentOffer toInsert = new(null, 4000, Hourly, Yen);
        var expectedAllObj = new List<PaymentOffer>
        {
            new(1, 2000, Monthly, Dollar),
            new(2, 25, Hourly, Euro),
            new(3, 30000, Daily, Yen),
            new(4, 15000, Once, Dollar),
            new(5, 3000, Monthly, Euro),
            new(6, 4000, Hourly, Yen)
        };
        var resultObj = _repository.Insert(toInsert);
        _repository.UnitOfWork.Commit();
        var resultAllObj = _repository.GetAll();

        // Custom Comparer without considering the PaymentOfferId, because it doesn't exist before the insert happens.
        var comparer = new Func<PaymentOffer, PaymentOffer, bool>(
            (expected, result) 
                => expected.Value == result.Value &&
                   expected.PaymentFrequency == result.PaymentFrequency &&
                   expected.Currency == result.Currency);
        
        Assert.Multiple(() =>
        {
            Assert.That(comparer(toInsert, resultObj), Is.True);
            Assert.That(resultAllObj, Is.EqualTo(expectedAllObj).AsCollection);
        });
    }
    
    [Order(3)]
    [Test(Description = "Repository should delete specified rows")]
    public void DeleteTest()
    {
        const int toRemove = 4;
        var expectedAllObj = new List<PaymentOffer>
        {
            new(1, 2000, Monthly, Dollar),
            new(2, 25, Hourly, Euro),
            new(3, 30000, Daily, Yen),
            new(5, 3000, Monthly, Euro),
            new(6, 4000, Hourly, Yen)
        };
        var resultObj = _repository.Delete(toRemove);
        _repository.UnitOfWork.Commit();
        var resultAllObj = _repository.GetAll().AsList();
        TestContext.WriteLine($"Delete returned: {resultObj}");
        
        Assert.Multiple(() =>
        {
            Assert.That(resultObj, Is.EqualTo(true));
            Assert.That(resultAllObj, Is.EqualTo(expectedAllObj).AsCollection);
        });
    }
    
    [Order(4)]
    [Test(Description = "Repository should return specified row")]
    public void GetPaymentOfferByIdTest()
    {
        const int toSearch = 5;
        var expectedObj = new PaymentOffer(5, 3000, Monthly, Euro);
        var resultObj = _repository.GetPaymentOfferById(toSearch);
        var resultJson = JsonConvert.SerializeObject(resultObj, Formatting.Indented);
        TestContext.WriteLine($"Get By Name Returned: {resultJson}");
        
        Assert.That(resultObj, Is.EqualTo(expectedObj));
    }
    
    [Order(5)]
    [Test(Description = "Repository should update specified row")]
    public void UpdateTest()
    {
        var toUpdate = new PaymentOffer(3, 250, Daily, Dollar);
        var expectedAllObj = new List<PaymentOffer>
        {
            new(1, 2000, Monthly, Dollar),
            new(2, 25, Hourly, Euro),
            new(3, 250, Daily, Dollar),
            new(5, 3000, Monthly, Euro),
            new(6, 4000, Hourly, Yen)
        };
        var resultObj = _repository.Update(toUpdate);
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
    public void GetPaymentOfferByIdFailWithExceptionTest()
    {
        const int toSearch = 10;
        Assert.That(() => _repository.GetPaymentOfferById(toSearch), Throws.ArgumentException);
    }
}