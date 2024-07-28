using System.Reflection;

namespace ProjectMarket.Test.Integration.Database.Interfaces;

public interface IMigration
{
    public String ConnectionString { get; }
    Assembly InitialConfigurationAssembly { get; set; }
    void RebuildMigrationProvider(Assembly assembly);
    void ExecuteMigration(int number);
    void ExecuteAllMigrations();
}