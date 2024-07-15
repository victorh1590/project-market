namespace ProjectMarket.Server.Infra.DependencyInjection;

using System.Reflection;
using FluentMigrator.Runner;

public static class MigrationsServiceCollectionExtensions
{
    
    public static IServiceCollection AddMigrations(this IServiceCollection services, IConfiguration configuration)
    {
        Assembly[] assemblies = GetAssembliesContainingNamespace("ProjectMarket.Server.Infra.Migrations");
        if (services != null)
        {
            services
                    // Add common FluentMigrator services
                    .AddFluentMigratorCore()
                    .ConfigureRunner(rb => rb
                        // Add Postgres support to FluentMigrator
                        .AddPostgres()
                        // Set the connection string
                        .WithGlobalConnectionString(configuration["CONNECTIONSTRING__POSTGRESQL"])
                        // Define the assembly containing the migrations
                        .ScanIn(assemblies).For.Migrations())
                    // Enable logging to console in the FluentMigrator way
                    .AddLogging(lb => lb.AddFluentMigratorConsole());
            // Build the service provider
            // .BuildServiceProvider(false);

            return services;
        }

        throw new ArgumentNullException(nameof(services));
    }

    static Assembly[] GetAssembliesContainingNamespace(string targetNamespace)
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
                // // Handle exceptions loading types from assembly if needed
                // foreach (Exception innerEx in ex.LoaderExceptions)
                // {
                // }
            }
        }

        return [.. matchingAssemblies];

    }
}