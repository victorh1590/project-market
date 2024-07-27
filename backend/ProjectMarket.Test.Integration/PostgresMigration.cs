using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using ProjectMarket.Test.Integration.Migrations;

namespace ProjectMarket.Test.Integration;
// "CONNECTIONSTRING__POSTGRESQL"
public class PostgresMigration : IMigration
{
    public String ConnectionString { get; init; }
    private ServiceProvider ServiceProvider { get; set; }
    public Assembly InitialConfigurationAssembly { get; set; }

    public PostgresMigration(String connectionString)
    {
        ConnectionString = connectionString;
        ServiceProvider = CreateServices();
        InitialConfigurationAssembly = typeof(InitialConfigurationDb).Assembly;
    }

    public void RebuildMigrationProvider(List<Assembly>? assemblies)
    {
        ServiceProvider = CreateServices(assemblies);
    }
    
    public void ExecuteMigration(int number)
    {
        using (var serviceProvider = CreateServices())
        using (var scope = serviceProvider.CreateScope())
        {
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.ListMigrations();
            runner.MigrateUp(number);
        }
    }

    public void ExecuteAllMigrations()
    {
        using (var serviceProvider = CreateServices())
        using (var scope = serviceProvider.CreateScope())
        {
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.ListMigrations();
            runner.MigrateUp();
        }
    }

    private ServiceProvider CreateServices(List<Assembly>? assemblies = null)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb =>
            {
                rb.AddPostgres().WithGlobalConnectionString(ConnectionString)
                    .ScanIn(InitialConfigurationAssembly).For.Migrations();
                if (assemblies == null) return;
                foreach (var assembly in assemblies)
                {
                    rb.ScanIn(assembly).For.Migrations();
                }
            })
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }
}