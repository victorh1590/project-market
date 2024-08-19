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
public class ProjectAdvertisementRepositoryTests
{
    private PostgresService _postgresService;
    private UnitOfWorkFactory _unitOfWorkFactory;
    private readonly PostgresCompiler _compiler = new();
    private ProjectAdvertisementRepository _repository;
    
    // Vo's and Objects used for tests.
    private PaymentFrequencyVo Monthly { get; } = new ("Monthly", "a month");
    private PaymentFrequencyVo Hourly { get; } = new ("Hourly", "per hour");
    private PaymentFrequencyVo Daily { get; } = new ("Daily", "per day");
    private PaymentFrequencyVo Once { get; } = new ("Once", "when project is done");

    private CurrencyVo Dollar { get; } = new ("Dollar", "$");
    private CurrencyVo Euro { get; } = new ("Euro", "€");
    private CurrencyVo Yen { get; } = new ("Yen", "¥");
    
    private AdvertisementStatusVo Open { get; } = new ("Open");
    private AdvertisementStatusVo Paused { get; } = new ("Paused");
    private AdvertisementStatusVo Closed { get; } = new ("Closed");

    private PaymentOffer PaymentOffer1 { get; init; }
    private PaymentOffer PaymentOffer2 { get; init; }
    private PaymentOffer PaymentOffer3 { get; init; }
    private PaymentOffer PaymentOffer4 { get; init; }
    private PaymentOffer PaymentOffer5 { get; init; }

    private Customer Customer1 { get; } = new(1, "Adam Adam", "adam.adam@example.com", "$2a$04$vo0GaDyEPfOb9f6gqviWh.UZLnabjN/cUEeBV5j21mLXSlngv4LyS"u8.ToArray(), new DateTime(2024, 2, 14, 10, 32, 45));
    private Customer Customer2 { get; } = new(2, "Alice Johnson", "alice.johnson@example.com", "$2a$04$ythI9xOhEmXaHsCiJ.Jh0ugqBohSpFlkjJJlozkiBcoDqC1SmQS1."u8.ToArray(), new DateTime(2024, 3, 5, 14, 21, 12));
    private Customer Customer3 { get; } = new(3, "Bob Smith", "bob.smith@example.com", "$2a$04$iGLiwDHKP81cPawqX.vEh.y7XK0u1qwhz8Z1uIkLyLAoKCCUL7pOW"u8.ToArray(), new DateTime(2024, 4, 21, 9, 45, 23));
    private Customer Customer4 { get; } = new(4, "Charlie Brown", "charlie.brown@example.com", "$2a$04$u9oWbsDKeAyV8a902.57iuMRtlDQOOtxyPfYWSz4RTdZ8.faFvyxW"u8.ToArray(), new DateTime(2024, 5, 30, 17, 54, 9));
    private Customer Customer5 { get; } = new(5, "David Wilson", "david.wilson@example.com", "$2a$04$7Tch3wNL3rDZqbZo7tO9/u7iNZmz1Pqb52nmNcm4grzyz.7kvHQiS"u8.ToArray(), new DateTime(2024, 6, 18, 8, 12, 34));
    private Customer Customer6 { get; } = new(6, "Emma Davis", "emma.davis@example.com", "$2a$04$ChRprKplMqJAKRD15N0vUuw.HK0LkxNLcrIMv88967J9N8tgDszba"u8.ToArray(), new DateTime(2024, 7, 10, 13, 46, 51));
    private Customer Customer7 { get; } = new(7, "Frank Miller", "frank.miller@example.com", "$2a$04$Rb9XEaVTLhNlY75mpaVPaetbhY/UwcTUN.Jt/5FN6BNCex0RSOb9G"u8.ToArray(), new DateTime(2024, 1, 25, 16, 23, 18));
    private Customer Customer8 { get; } = new(8, "Grace Lee", "grace.lee@example.com", "$2a$04$UNXL4rXerkg7YQnDJRJYPuPWVYDmyiSVifRnb8LvOaCMujhYn1naC"u8.ToArray(), new DateTime(2024, 8, 3, 19, 37, 29));
    private Customer Customer9 { get; } = new(9, "Henry Thompson", "henry.thompson@example.com", "$2a$04$QBBN1Wo8U46A/7OJIASFCutH64iE3s42nEE411qRXicxa8jYQllYS"u8.ToArray(), new DateTime(2024, 2, 27, 11, 54, 48));

    // JobRequirementVo objects
    private JobRequirementVo Python { get; } = new ("Python");
    private JobRequirementVo CSharp { get; } = new ("C#");
    private JobRequirementVo Go { get; } = new ("Go");
    private JobRequirementVo PowerBI { get; } = new ("Power BI");

    // KnowledgeAreaVo objects
    private KnowledgeAreaVo WebDevelopment { get; } = new ("Web Development");
    private KnowledgeAreaVo DataAnalysis { get; } = new ("Data Analysis");
    private KnowledgeAreaVo AI { get; } = new ("AI");
    private KnowledgeAreaVo SystemDevelopment { get; } = new ("System Development");

    // Lists of KnowledgeAreaVo
    private List<KnowledgeAreaVo> Subjects1 { get; init; }
    private List<KnowledgeAreaVo> Subjects2 { get; init; }
    private List<KnowledgeAreaVo> Subjects3 { get; init; }
    private List<KnowledgeAreaVo> Subjects4 { get; init; }
    private List<KnowledgeAreaVo> Subjects5 { get; init; }
    private List<KnowledgeAreaVo> Subjects6 { get; init; }
    private List<KnowledgeAreaVo> Subjects7 { get; init; }

    // Lists of JobRequirementVo
    private List<JobRequirementVo> Requirements1 { get; init; }
    private List<JobRequirementVo> Requirements2 { get; init; }
    private List<JobRequirementVo> Requirements3 { get; init; }
    private List<JobRequirementVo> Requirements4 { get; init; }
    private List<JobRequirementVo> Requirements5 { get; init; }
    private List<JobRequirementVo> Requirements6 { get; init; }
    private List<JobRequirementVo> Requirements7 { get; init; }

    public ProjectAdvertisementRepositoryTests()
    {
        PaymentOffer1 = new PaymentOffer(1, 2000, Monthly, Dollar);
        PaymentOffer2 = new PaymentOffer(2, 25, Hourly, Euro);
        PaymentOffer3 = new PaymentOffer(3, 30000, Daily, Yen);
        PaymentOffer4 = new PaymentOffer(4, 15000, Once, Dollar);
        PaymentOffer5 = new PaymentOffer(5, 3000, Monthly, Euro);  
        
        Subjects1 = [AI, DataAnalysis];
        Subjects2 = [SystemDevelopment, WebDevelopment];
        Subjects3 = [WebDevelopment, DataAnalysis];
        Subjects4 = [SystemDevelopment, DataAnalysis];
        Subjects5 = [AI, SystemDevelopment];
        Subjects6 = [AI, WebDevelopment];
        Subjects7 = [SystemDevelopment, AI];
        
        Requirements1 = [Python, PowerBI];
        Requirements2 = [CSharp, Go];
        Requirements3 = [Python, PowerBI];
        Requirements4 = [Go, Python];
        Requirements5 = [CSharp, Python];
        Requirements6 = [Go, Python];
        Requirements7 = [CSharp, Go];
    }
    
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
                    s.Contains(nameof(JobRequirementRepositoryTests) + scriptSuffix, StringComparison.OrdinalIgnoreCase) ||
                    s.Contains(nameof(KnowledgeAreaRepositoryTests) + scriptSuffix, StringComparison.OrdinalIgnoreCase) ||
                    s.Contains(nameof(CurrencyRepositoryTests) + scriptSuffix, StringComparison.OrdinalIgnoreCase) ||
                    s.Contains(nameof(PaymentFrequencyRepositoryTests) + scriptSuffix, StringComparison.OrdinalIgnoreCase) ||
                    s.Contains(nameof(AdvertisementStatusRepositoryTests) + scriptSuffix, StringComparison.OrdinalIgnoreCase) ||
                    s.Contains(nameof(CustomerRepositoryTests) + scriptSuffix, StringComparison.OrdinalIgnoreCase) ||
                    s.Contains(nameof(PaymentOfferRepositoryTests) + scriptSuffix, StringComparison.OrdinalIgnoreCase) ||
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
        _repository = new ProjectAdvertisementRepository(unitOfWork, _compiler);
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
        var expectedAllObj = new List<ProjectAdvertisement>
        {
            new(1, "AI Predictive Model", "Developing an AI model for financial forecasting.", 
                new DateTime(2024, 8, 1, 8, 30, 0), 
                new DateTime(2024, 9, 15, 17, 0, 0), 
                PaymentOffer1, Customer1, Open, Subjects1, Requirements1
            ),
            new(
                2, "E-Commerce Backend", "Building a scalable backend system.", 
                new DateTime(2024, 8, 5, 9, 0, 0), 
                new DateTime(2024, 9, 1, 18, 0, 0), 
                PaymentOffer4, Customer2, Paused, Subjects2, Requirements2
            ),
            new(
                3, "Web Development Campaign", null, 
                new DateTime(2024, 8, 10, 10, 15, 0), 
                new DateTime(2024, 9, 10, 16, 30, 0), 
                PaymentOffer5, Customer3, Open, Subjects3, Requirements3
            ),
            new(
                4, "Real-Time Data Pipeline", "Creating a real-time data pipeline for analytics.", 
                new DateTime(2024, 8, 12, 8, 45, 0), 
                new DateTime(2024, 9, 20, 17, 45, 0), 
                PaymentOffer3, Customer4, Closed, Subjects4, Requirements4
            ),
            new(
                5, "AI Mobile Application", "Designing an AI-driven mobile app.", 
                new DateTime(2024, 8, 15, 11, 0, 0), 
                new DateTime(2024, 9, 25, 15, 0, 0), 
                PaymentOffer2, Customer5, Open, Subjects5, Requirements5
            )
        };
        var resultAllObj = _repository.GetAll();
        
        Assert.That(resultAllObj, Is.EqualTo(expectedAllObj).AsCollection);
    }
    
    [Order(2)]
    [Test(Description = "Repository should insert specified rows")]
    public void InsertTest()
    {
        ProjectAdvertisement toInsert = new(
            6, "Machine Learning API", "Developing an API for machine learning model integration.", 
            new DateTime(2024, 8, 18, 9, 30, 0), 
            new DateTime(2024, 9, 30, 14, 45, 0), 
            PaymentOffer1, Customer6, Paused, Subjects6, Requirements6
        );
        var expectedAllObj = new List<ProjectAdvertisement>
        {
            new(1, "AI Predictive Model", "Developing an AI model for financial forecasting.", 
                new DateTime(2024, 8, 1, 8, 30, 0), 
                new DateTime(2024, 9, 15, 17, 0, 0), 
                PaymentOffer1, Customer1, Open, Subjects1, Requirements1
            ),
            new(
                2, "E-Commerce Backend", "Building a scalable backend system.", 
                new DateTime(2024, 8, 5, 9, 0, 0), 
                new DateTime(2024, 9, 1, 18, 0, 0), 
                PaymentOffer4, Customer2, Paused, Subjects2, Requirements2
            ),
            new(
                3, "Web Development Campaign", null, 
                new DateTime(2024, 8, 10, 10, 15, 0), 
                new DateTime(2024, 9, 10, 16, 30, 0), 
                PaymentOffer5, Customer3, Open, Subjects3, Requirements3
            ),
            new(
                4, "Real-Time Data Pipeline", "Creating a real-time data pipeline for analytics.", 
                new DateTime(2024, 8, 12, 8, 45, 0), 
                new DateTime(2024, 9, 20, 17, 45, 0), 
                PaymentOffer3, Customer4, Closed, Subjects4, Requirements4
            ),
            new(
                5, "AI Mobile Application", "Designing an AI-driven mobile app.", 
                new DateTime(2024, 8, 15, 11, 0, 0), 
                new DateTime(2024, 9, 25, 15, 0, 0), 
                PaymentOffer2, Customer5, Open, Subjects5, Requirements5
            ),
            new(
                6, "Machine Learning API", "Developing an API for machine learning model integration.", 
                new DateTime(2024, 8, 18, 9, 30, 0), 
                new DateTime(2024, 9, 30, 14, 45, 0), 
                PaymentOffer1, Customer6, Paused, Subjects6, Requirements6
            )
        };
        var resultObj = _repository.Insert(toInsert);
        _repository.UnitOfWork.Commit();
        var resultAllObj = _repository.GetAll();

        // Custom Comparer without considering the ProjectAdvertisementId, because it doesn't exist before the insert happens.
        var comparer = new Func<ProjectAdvertisement, ProjectAdvertisement, bool>(
            (expected, result)
                => expected.Title == result.Title &&
                   expected.Description == result.Description &&
                   expected.OpenedOn == result.OpenedOn &&
                   Nullable.Equals(expected.Deadline, result.Deadline) &&
                   expected.PaymentOffer.Equals(result.PaymentOffer) &&
                   expected.Customer.Equals(result.Customer) &&
                   expected.Status == result.Status &&
                   expected.Subjects.Equals(result.Subjects) &&
                   (expected.Requirements?.Equals(result.Requirements) ?? result.Requirements == null));
                   
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
        const int toRemove = 2;
        var expectedAllObj = new List<ProjectAdvertisement>
        {
            new(1, "AI Predictive Model", "Developing an AI model for financial forecasting.", 
                new DateTime(2024, 8, 1, 8, 30, 0), 
                new DateTime(2024, 9, 15, 17, 0, 0), 
                PaymentOffer1, Customer1, Open, Subjects1, Requirements1
            ),
            new(
                3, "Web Development Campaign", null, 
                new DateTime(2024, 8, 10, 10, 15, 0), 
                new DateTime(2024, 9, 10, 16, 30, 0), 
                PaymentOffer5, Customer3, Open, Subjects3, Requirements3
            ),
            new(
                4, "Real-Time Data Pipeline", "Creating a real-time data pipeline for analytics.", 
                new DateTime(2024, 8, 12, 8, 45, 0), 
                new DateTime(2024, 9, 20, 17, 45, 0), 
                PaymentOffer3, Customer4, Closed, Subjects4, Requirements4
            ),
            new(
                5, "AI Mobile Application", "Designing an AI-driven mobile app.", 
                new DateTime(2024, 8, 15, 11, 0, 0), 
                new DateTime(2024, 9, 25, 15, 0, 0), 
                PaymentOffer2, Customer5, Open, Subjects5, Requirements5
            ),
            new(
                6, "Machine Learning API", "Developing an API for machine learning model integration.", 
                new DateTime(2024, 8, 18, 9, 30, 0), 
                new DateTime(2024, 9, 30, 14, 45, 0), 
                PaymentOffer1, Customer6, Paused, Subjects6, Requirements6
            )
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
    public void GetProjectAdvertisementByIdTest()
    {
        const int toSearch = 5;
        var expectedObj = new ProjectAdvertisement(
            5, "AI Mobile Application", "Designing an AI-driven mobile app.",
            new DateTime(2024, 8, 15, 11, 0, 0),
            new DateTime(2024, 9, 25, 15, 0, 0),
            PaymentOffer2, Customer5, Open, Subjects5, Requirements5
        );
        var resultObj = _repository.GetProjectAdvertisementById(toSearch);
        var resultJson = JsonConvert.SerializeObject(resultObj, Formatting.Indented);
        TestContext.WriteLine($"Get By Name Returned: {resultJson}");
        
        Assert.That(resultObj, Is.EqualTo(expectedObj));
    }
    
    [Order(5)]
    [Test(Description = "Repository should update specified row")]
    public void UpdateTest()
    {
        var toUpdate = new ProjectAdvertisement(
            1, "System Security Audit", "Conducting a security audit for a new system.",
            new DateTime(2024, 8, 20, 10, 0, 0),
            new DateTime(2024, 10, 5, 12, 0, 0),
            PaymentOffer4, Customer7, Closed, Subjects7, Requirements7
        );
        var resultObj = _repository.Update(toUpdate);
        var expectedAllObj = new List<ProjectAdvertisement>
        {
            new(
                3, "Web Development Campaign", null, 
                new DateTime(2024, 8, 10, 10, 15, 0), 
                new DateTime(2024, 9, 10, 16, 30, 0), 
                PaymentOffer5, Customer3, Open, Subjects3, Requirements3
            ),
            new(
                4, "Real-Time Data Pipeline", "Creating a real-time data pipeline for analytics.", 
                new DateTime(2024, 8, 12, 8, 45, 0), 
                new DateTime(2024, 9, 20, 17, 45, 0), 
                PaymentOffer3, Customer4, Closed, Subjects4, Requirements4
            ),
            new(
                5, "AI Mobile Application", "Designing an AI-driven mobile app.", 
                new DateTime(2024, 8, 15, 11, 0, 0), 
                new DateTime(2024, 9, 25, 15, 0, 0), 
                PaymentOffer2, Customer5, Open, Subjects5, Requirements5
            ),
            new(
                6, "Machine Learning API", "Developing an API for machine learning model integration.", 
                new DateTime(2024, 8, 18, 9, 30, 0), 
                new DateTime(2024, 9, 30, 14, 45, 0), 
                PaymentOffer1, Customer6, Paused, Subjects6, Requirements6
            ),
            new(
                1, "System Security Audit", "Conducting a security audit for a new system.",
                new DateTime(2024, 8, 20, 10, 0, 0),
                new DateTime(2024, 10, 5, 12, 0, 0),
                PaymentOffer4, Customer7, Closed, Subjects7, Requirements7
            )
        };
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
    public void GetProjectAdvertisementByIdFailWithExceptionTest()
    {
        const int toSearch = 8;
        Assert.That(() => _repository.GetProjectAdvertisementById(toSearch), Throws.ArgumentException);
    }
}