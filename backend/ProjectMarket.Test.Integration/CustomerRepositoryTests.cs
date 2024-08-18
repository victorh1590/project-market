using System.Reflection;
using Dapper;
using DbUp;
using Newtonsoft.Json;
using NUnit.Framework;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Infra.Db;
using ProjectMarket.Server.Infra.Migrations;
using ProjectMarket.Server.Infra.Repository;
using ProjectMarket.Test.Integration.Database;
using SqlKata.Compilers;

namespace ProjectMarket.Test.Integration;

[TestFixture]
public class CustomerRepositoryTests
{
    private PostgresService _postgresService;
    private UnitOfWorkFactory _unitOfWorkFactory;
    private readonly PostgresCompiler _compiler = new();
    private CustomerRepository _repository;

    [OneTimeSetUp]
    public async Task OneTimeSetUpAsync()
    {
        _postgresService = await PostgresServiceFactory.CreateServiceAsync() ?? throw new InvalidOperationException();
        _unitOfWorkFactory = new UnitOfWorkFactory(_postgresService.Configuration, _postgresService.DbmsName);
        _postgresService.Migration.RebuildMigrationProvider( typeof(_1_CreateVOTables).Assembly );
        _postgresService.Migration.ExecuteMigration(2);
        
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
        _repository = new CustomerRepository(unitOfWork, _compiler);
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
        var expectedAllObj = new List<Customer>
        {
            new(1, "Adam Adam", "adam.adam@example.com", "$2a$04$vo0GaDyEPfOb9f6gqviWh.UZLnabjN/cUEeBV5j21mLXSlngv4LyS"u8.ToArray(), new DateTime(2024, 2, 14, 10, 32, 45)),
            new(2, "Alice Johnson", "alice.johnson@example.com", "$2a$04$ythI9xOhEmXaHsCiJ.Jh0ugqBohSpFlkjJJlozkiBcoDqC1SmQS1."u8.ToArray(), new DateTime(2024, 3, 5, 14, 21, 12)),
            new(3, "Bob Smith", "bob.smith@example.com", "$2a$04$iGLiwDHKP81cPawqX.vEh.y7XK0u1qwhz8Z1uIkLyLAoKCCUL7pOW"u8.ToArray(), new DateTime(2024, 4, 21, 9, 45, 23)),
            new(4, "Charlie Brown", "charlie.brown@example.com", "$2a$04$u9oWbsDKeAyV8a902.57iuMRtlDQOOtxyPfYWSz4RTdZ8.faFvyxW"u8.ToArray(), new DateTime(2024, 5, 30, 17, 54, 9)),
            new(5, "David Wilson", "david.wilson@example.com", "$2a$04$7Tch3wNL3rDZqbZo7tO9/u7iNZmz1Pqb52nmNcm4grzyz.7kvHQiS"u8.ToArray(), new DateTime(2024, 6, 18, 8, 12, 34)),
            new(6, "Emma Davis", "emma.davis@example.com", "$2a$04$ChRprKplMqJAKRD15N0vUuw.HK0LkxNLcrIMv88967J9N8tgDszba"u8.ToArray(), new DateTime(2024, 7, 10, 13, 46, 51)),
            new(7, "Frank Miller", "frank.miller@example.com", "$2a$04$Rb9XEaVTLhNlY75mpaVPaetbhY/UwcTUN.Jt/5FN6BNCex0RSOb9G"u8.ToArray(), new DateTime(2024, 1, 25, 16, 23, 18)),
            new(8, "Grace Lee", "grace.lee@example.com", "$2a$04$UNXL4rXerkg7YQnDJRJYPuPWVYDmyiSVifRnb8LvOaCMujhYn1naC"u8.ToArray(), new DateTime(2024, 8, 3, 19, 37, 29)),
            new(9, "Henry Thompson", "henry.thompson@example.com", "$2a$04$QBBN1Wo8U46A/7OJIASFCutH64iE3s42nEE411qRXicxa8jYQllYS"u8.ToArray(), new DateTime(2024, 2, 27, 11, 54, 48)),
        };
        var resultAllObj = _repository.GetAll();
        
        Assert.That(resultAllObj, Is.EqualTo(expectedAllObj).AsCollection);
    }
    
    [Order(2)]
    [Test(Description = "Repository should insert specified rows")]
    public void InsertTest()
    {
        Customer toInsert = new(
            null, 
            "Jack White", 
            "jack.white@example.com", 
            "$2a$04$WVPOSbzu6xBjz1dDvdTHEO8RvMJUsAPnHpHxT8i.Ud8Kvj12gAbjW"u8.ToArray(), 
            new DateTime(2024,05,14,7,38,56));
        var expectedAllObj = new List<Customer>
        {
            new(1, "Adam Adam", "adam.adam@example.com", "$2a$04$vo0GaDyEPfOb9f6gqviWh.UZLnabjN/cUEeBV5j21mLXSlngv4LyS"u8.ToArray(), new DateTime(2024, 2, 14, 10, 32, 45)),
            new(2, "Alice Johnson", "alice.johnson@example.com", "$2a$04$ythI9xOhEmXaHsCiJ.Jh0ugqBohSpFlkjJJlozkiBcoDqC1SmQS1."u8.ToArray(), new DateTime(2024, 3, 5, 14, 21, 12)),
            new(3, "Bob Smith", "bob.smith@example.com", "$2a$04$iGLiwDHKP81cPawqX.vEh.y7XK0u1qwhz8Z1uIkLyLAoKCCUL7pOW"u8.ToArray(), new DateTime(2024, 4, 21, 9, 45, 23)),
            new(4, "Charlie Brown", "charlie.brown@example.com", "$2a$04$u9oWbsDKeAyV8a902.57iuMRtlDQOOtxyPfYWSz4RTdZ8.faFvyxW"u8.ToArray(), new DateTime(2024, 5, 30, 17, 54, 9)),
            new(5, "David Wilson", "david.wilson@example.com", "$2a$04$7Tch3wNL3rDZqbZo7tO9/u7iNZmz1Pqb52nmNcm4grzyz.7kvHQiS"u8.ToArray(), new DateTime(2024, 6, 18, 8, 12, 34)),
            new(6, "Emma Davis", "emma.davis@example.com", "$2a$04$ChRprKplMqJAKRD15N0vUuw.HK0LkxNLcrIMv88967J9N8tgDszba"u8.ToArray(), new DateTime(2024, 7, 10, 13, 46, 51)),
            new(7, "Frank Miller", "frank.miller@example.com", "$2a$04$Rb9XEaVTLhNlY75mpaVPaetbhY/UwcTUN.Jt/5FN6BNCex0RSOb9G"u8.ToArray(), new DateTime(2024, 1, 25, 16, 23, 18)),
            new(8, "Grace Lee", "grace.lee@example.com", "$2a$04$UNXL4rXerkg7YQnDJRJYPuPWVYDmyiSVifRnb8LvOaCMujhYn1naC"u8.ToArray(), new DateTime(2024, 8, 3, 19, 37, 29)),
            new(9, "Henry Thompson", "henry.thompson@example.com", "$2a$04$QBBN1Wo8U46A/7OJIASFCutH64iE3s42nEE411qRXicxa8jYQllYS"u8.ToArray(), new DateTime(2024, 2, 27, 11, 54, 48)),
            new(10, "Jack White", "jack.white@example.com", "$2a$04$WVPOSbzu6xBjz1dDvdTHEO8RvMJUsAPnHpHxT8i.Ud8Kvj12gAbjW"u8.ToArray(), new DateTime(2024,05,14,7,38,56))
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
        const int toRemove = 3;
        var expectedAllObj = new List<Customer>
        {
            new(1, "Adam Adam", "adam.adam@example.com", "$2a$04$vo0GaDyEPfOb9f6gqviWh.UZLnabjN/cUEeBV5j21mLXSlngv4LyS"u8.ToArray(), new DateTime(2024, 2, 14, 10, 32, 45)),
            new(2, "Alice Johnson", "alice.johnson@example.com", "$2a$04$ythI9xOhEmXaHsCiJ.Jh0ugqBohSpFlkjJJlozkiBcoDqC1SmQS1."u8.ToArray(), new DateTime(2024, 3, 5, 14, 21, 12)),
            new(4, "Charlie Brown", "charlie.brown@example.com", "$2a$04$u9oWbsDKeAyV8a902.57iuMRtlDQOOtxyPfYWSz4RTdZ8.faFvyxW"u8.ToArray(), new DateTime(2024, 5, 30, 17, 54, 9)),
            new(5, "David Wilson", "david.wilson@example.com", "$2a$04$7Tch3wNL3rDZqbZo7tO9/u7iNZmz1Pqb52nmNcm4grzyz.7kvHQiS"u8.ToArray(), new DateTime(2024, 6, 18, 8, 12, 34)),
            new(6, "Emma Davis", "emma.davis@example.com", "$2a$04$ChRprKplMqJAKRD15N0vUuw.HK0LkxNLcrIMv88967J9N8tgDszba"u8.ToArray(), new DateTime(2024, 7, 10, 13, 46, 51)),
            new(7, "Frank Miller", "frank.miller@example.com", "$2a$04$Rb9XEaVTLhNlY75mpaVPaetbhY/UwcTUN.Jt/5FN6BNCex0RSOb9G"u8.ToArray(), new DateTime(2024, 1, 25, 16, 23, 18)),
            new(8, "Grace Lee", "grace.lee@example.com", "$2a$04$UNXL4rXerkg7YQnDJRJYPuPWVYDmyiSVifRnb8LvOaCMujhYn1naC"u8.ToArray(), new DateTime(2024, 8, 3, 19, 37, 29)),
            new(9, "Henry Thompson", "henry.thompson@example.com", "$2a$04$QBBN1Wo8U46A/7OJIASFCutH64iE3s42nEE411qRXicxa8jYQllYS"u8.ToArray(), new DateTime(2024, 2, 27, 11, 54, 48)),
            new(10, "Jack White", "jack.white@example.com", "$2a$04$WVPOSbzu6xBjz1dDvdTHEO8RvMJUsAPnHpHxT8i.Ud8Kvj12gAbjW"u8.ToArray(), new DateTime(2024,05,14,7,38,56))
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
    
    [Ignore("fixing other tests")]
    [Order(4)]
    [Test(Description = "Repository should return specified row")]
    public void GetCustomerByIdTest()
    {
        const int toSearch = 6;
        var expectedObj = new Customer(
            6, 
            "Emma Davis", 
            "emma.davis@example.com", 
            "$2a$04$ChRprKplMqJAKRD15N0vUuw.HK0LkxNLcrIMv88967J9N8tgDszba"u8.ToArray(), 
            new DateTime(2024, 7, 10, 13, 46, 51));
        var resultObj = _repository.GetCustomerById(toSearch);
        var resultJson = JsonConvert.SerializeObject(resultObj, Formatting.Indented);
        TestContext.WriteLine($"Get By Name Returned: {resultJson}");
        
        Assert.That(resultObj, Is.EqualTo(expectedObj));
    }
    
    [Ignore("fixing other tests")]
    [Order(5)]
    [Test(Description = "Repository should update specified row")]
    public void UpdateTest()
    {
        var toUpdate = new Customer(
            8,
            "Ivy Martinez",
            "ivy.martinez@example.com",
            "$2a$04$.a6d2TTE121rHPju9A16fumBMfjSqoMXsdRL7PL3Ye5IeWr2pR0x."u8.ToArray(),
            new DateTime(2024, 7, 22, 15, 29, 7));
        var expectedAllObj = new List<Customer>
        {
            new(1, "Adam Adam", "adam.adam@example.com", "$2a$04$vo0GaDyEPfOb9f6gqviWh.UZLnabjN/cUEeBV5j21mLXSlngv4LyS"u8.ToArray(), new DateTime(2024, 2, 14, 10, 32, 45)),
            new(2, "Alice Johnson", "alice.johnson@example.com", "$2a$04$ythI9xOhEmXaHsCiJ.Jh0ugqBohSpFlkjJJlozkiBcoDqC1SmQS1."u8.ToArray(), new DateTime(2024, 3, 5, 14, 21, 12)),
            new(4, "Charlie Brown", "charlie.brown@example.com", "$2a$04$u9oWbsDKeAyV8a902.57iuMRtlDQOOtxyPfYWSz4RTdZ8.faFvyxW"u8.ToArray(), new DateTime(2024, 5, 30, 17, 54, 9)),
            new(5, "David Wilson", "david.wilson@example.com", "$2a$04$7Tch3wNL3rDZqbZo7tO9/u7iNZmz1Pqb52nmNcm4grzyz.7kvHQiS"u8.ToArray(), new DateTime(2024, 6, 18, 8, 12, 34)),
            new(6, "Emma Davis", "emma.davis@example.com", "$2a$04$ChRprKplMqJAKRD15N0vUuw.HK0LkxNLcrIMv88967J9N8tgDszba"u8.ToArray(), new DateTime(2024, 7, 10, 13, 46, 51)),
            new(7, "Frank Miller", "frank.miller@example.com", "$2a$04$Rb9XEaVTLhNlY75mpaVPaetbhY/UwcTUN.Jt/5FN6BNCex0RSOb9G"u8.ToArray(), new DateTime(2024, 1, 25, 16, 23, 18)),
            new(8, "Ivy Martinez", "ivy.martinez@example.com", "$2a$04$.a6d2TTE121rHPju9A16fumBMfjSqoMXsdRL7PL3Ye5IeWr2pR0x."u8.ToArray(), new DateTime(2024, 7, 22, 15, 29, 7)),
            new(9, "Henry Thompson", "henry.thompson@example.com", "$2a$04$QBBN1Wo8U46A/7OJIASFCutH64iE3s42nEE411qRXicxa8jYQllYS"u8.ToArray(), new DateTime(2024, 2, 27, 11, 54, 48)),
            new(10, "Jack White", "jack.white@example.com", "$2a$04$WVPOSbzu6xBjz1dDvdTHEO8RvMJUsAPnHpHxT8i.Ud8Kvj12gAbjW"u8.ToArray(), new DateTime(2024,05,14,7,38,56))
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
    
    [Ignore("fixing other tests")]
    [Order(6)]
    [Test(Description = "Repository should throw Exception when row doesn't exist")]
    public void GetCustomerByIdFailWithExceptionTest()
    {
        const int toSearch = 15;
        Assert.That(() => _repository.GetCustomerById(toSearch), Throws.ArgumentException);
    }
}