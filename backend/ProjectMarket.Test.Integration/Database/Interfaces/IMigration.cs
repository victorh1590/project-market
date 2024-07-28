using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace ProjectMarket.Test.Integration;

public interface IMigration
{
    public String ConnectionString { get; init; }
    Assembly InitialConfigurationAssembly { get; set; }
    void RebuildMigrationProvider(List<Assembly>? assemblies);
    void ExecuteMigration(int number);
    void ExecuteAllMigrations();
}