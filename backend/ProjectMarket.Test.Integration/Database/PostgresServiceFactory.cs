namespace ProjectMarket.Test.Integration.Database;

public class PostgresServiceFactory
{
    public static async Task<PostgresService?> CreateServiceAsync()
    {
        IPostgresDbResource postrgresDbResource = new PostgresDbResource();
        await postrgresDbResource.StartAsync();
        String connectionString = postrgresDbResource.PostgreSqlContainer.GetConnectionString();
        IMigration postgresMigration = new PostgresMigration(connectionString);
        return new PostgresService(postrgresDbResource, postgresMigration);
    }
    
    public static PostgresService CreateService()
    {
        IPostgresDbResource postrgresDbResource = new PostgresDbResource();
        postrgresDbResource.StartAsync().AsTask().Wait();
        String connectionString = postrgresDbResource.PostgreSqlContainer.GetConnectionString();
        IMigration postgresMigration = new PostgresMigration(connectionString);
        return new PostgresService(postrgresDbResource, postgresMigration);
    }
}