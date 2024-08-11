﻿using System.Reflection;
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
public class KnowledgeAreaRepositoryTests
{
    private PostgresService _postgresService;
    private UnitOfWorkFactory _unitOfWorkFactory;
    private readonly PostgresCompiler _compiler = new();
    private KnowledgeAreaRepository _repository;

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
        _repository = new KnowledgeAreaRepository(unitOfWork, _compiler);
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
        var expectedObj = new List<KnowledgeAreaVo>
        {
            new() { KnowledgeAreaName = "Web Development" },
            new() { KnowledgeAreaName = "Data Analysis" },
            new() { KnowledgeAreaName = "AI" },
            new() { KnowledgeAreaName = "System Development" }
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
        KnowledgeAreaVo toInsert = new() { KnowledgeAreaName = "Mobile Development" };
        var expectedJson = JsonConvert.SerializeObject(toInsert, Formatting.Indented);
        var expectedAllObj = new List<KnowledgeAreaVo>
        {
            new() { KnowledgeAreaName = "Web Development" },
            new() { KnowledgeAreaName = "Data Analysis" },
            new() { KnowledgeAreaName = "AI" },
            new() { KnowledgeAreaName = "System Development" }
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
        KnowledgeAreaVo toRemove = new() { KnowledgeAreaName = "AI" };

        var expectedAllObj = new List<KnowledgeAreaVo>
        {
            new() { KnowledgeAreaName = "Web Development" },
            new() { KnowledgeAreaName = "Data Analysis" },
            new() { KnowledgeAreaName = "System Development" },
            new() { KnowledgeAreaName = "Mobile Development" }
        };
        var resultObj = _repository.Delete(toRemove.KnowledgeAreaName);
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
        const string toSearch = "Data Analysis";
        var expectedObj = new KnowledgeAreaVo() { KnowledgeAreaName = "Data Analysis" };

        var resultObj = _repository.GetKnowledgeAreaByName(toSearch);

        var resultJson = JsonConvert.SerializeObject(resultObj, Formatting.Indented);
        TestContext.WriteLine($"Get By Name Returned: {resultJson}");
        Assert.That(resultObj, Is.EqualTo(expectedObj));
    }
    
    [Order(5)]
    [Test(Description = "Repository should update specified row")]
    public void UpdateTest()
    {
        const string toUpdate = "System Development";
        var update = new KnowledgeAreaVo() { KnowledgeAreaName = "Microcontrollers" };
        
        var expectedAllObj = new List<KnowledgeAreaVo>
        {
            new() { KnowledgeAreaName = "Web Development" },
            new() { KnowledgeAreaName = "Data Analysis" },
            new() { KnowledgeAreaName = "Microcontrollers" },
            new() { KnowledgeAreaName = "Mobile Development" }
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
        const string toSearch = "Game Development";
        Assert.That(() => _repository.GetKnowledgeAreaByName(toSearch), Throws.ArgumentException);
    }
}