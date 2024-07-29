using ProjectMarket.Server.Infra.Db;
using ProjectMarket.Server.Infra.Repository;
using SqlKata.Compilers;

namespace ProjectMarket.Server.Infra.DependencyInjection;

using System.Reflection;
using FluentMigrator.Runner;

public static class ServiceCollectionExtensions
{
    public static void AddMigrations(this IServiceCollection services, IConfiguration configuration)
    {
        Assembly[] assemblies = GetAssembliesContainingNamespace("ProjectMarket.Server.Infra.Migrations");
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(configuration["CONNECTIONSTRING__POSTGRESQL"])
                .ScanIn(assemblies).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());
        // .BuildServiceProvider(false);
    }

    private static Assembly[] GetAssembliesContainingNamespace(string targetNamespace)
    {
        List<Assembly> matchingAssemblies = [];

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                // Load types from the assembly
                var types = assembly.GetTypes();
                // Check if any type in the assembly belongs to the target namespace
                if (types.Any(t => t.Namespace == targetNamespace))
                {
                    matchingAssemblies.Add(assembly);
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine(ex?.Message);
                // Handle exceptions loading types from assembly if needed
                if(ex?.LoaderExceptions != null) {
                    foreach (var innerEx in ex.LoaderExceptions.Where(innerEx => innerEx != null).ToArray())
                    {
                        Console.WriteLine(innerEx?.Message);
                    }
                }
            }
        }
        return [.. matchingAssemblies];
    }
    
    public static void AddUnitOfWork(this IServiceCollection services, DbmsName dbmsName)
    {
        services.AddSingleton<UnitOfWorkFactory>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            return new UnitOfWorkFactory(configuration, dbmsName);
        });
        services.AddScoped<IUnitOfWork>(provider =>
        {
            var factory = provider.GetRequiredService<UnitOfWorkFactory>();
            return factory.CreateUnitOfWork();
        });
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<AdvertisementStatusRepository>();
        services.AddScoped<CurrencyRepository>();
        services.AddScoped<CustomerRepository>();
        services.AddScoped<JobRequirementRepository>();
        services.AddScoped<KnowledgeAreaRepository>();
        services.AddScoped<PaymentFrequencyRepository>();
        services.AddScoped<PaymentOfferRepository>();
        services.AddScoped<ProjectAdvertisementRepository>();
    }
}