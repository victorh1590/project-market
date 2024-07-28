using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using ProjectMarket.Test.Integration.Database.Interfaces;
using ProjectMarket.Test.Integration.Migrations;

namespace ProjectMarket.Test.Integration.Database
{
    public class PostgresMigration : IMigration
    {
        public string ConnectionString { get; init; }
        private ServiceProvider ServiceProvider { get; set; }
        public Assembly InitialConfigurationAssembly { get; set; }

        public PostgresMigration(string connectionString)
        {
            ConnectionString = connectionString;
            InitialConfigurationAssembly = typeof(InitialConfigurationDb).Assembly;
            ServiceProvider = CreateServices([InitialConfigurationAssembly]);
        }

        public void RebuildMigrationProvider(Assembly assembly)
        {
            ServiceProvider.Dispose();
            ServiceProvider = CreateServices([InitialConfigurationAssembly, assembly]);
        }

        public void ExecuteMigration(int number)
        {
            using var scope = ServiceProvider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.ListMigrations();
            runner.MigrateUp(number);
        }

        public void ExecuteAllMigrations()
        {
            using var scope = ServiceProvider.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.ListMigrations();
            runner.MigrateUp();
        }

        private ServiceProvider CreateServices(List<Assembly> assemblies)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                {
                    rb.AddPostgres().WithGlobalConnectionString(ConnectionString);
                    foreach (var assembly in assemblies)
                    {
                        rb.ScanIn(assembly).For.Migrations();
                    }
                })
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }
    }
}
