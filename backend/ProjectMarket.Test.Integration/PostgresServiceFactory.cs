namespace ProjectMarket.Test.Integration;

public static class PostgresServiceFactory
{
    public static async Task<PostgresService?> CreateServiceAsync()
    {
        IPostgresDbResource postrgresDbResource = new PostgresDbResource();
        await postrgresDbResource.InitializeAsync();
        String connectionString = postrgresDbResource.PostgreSqlContainer.GetConnectionString();
        IMigration postgresMigration = new PostgresMigration(connectionString);
        return new PostgresService(postrgresDbResource, postgresMigration);
    }
    
    public static PostgresService CreateService()
    {
        IPostgresDbResource postrgresDbResource = new PostgresDbResource();
        postrgresDbResource.InitializeAsync().Wait();
        String connectionString = postrgresDbResource.PostgreSqlContainer.GetConnectionString();
        IMigration postgresMigration = new PostgresMigration(connectionString);
        return new PostgresService(postrgresDbResource, postgresMigration);
    }
}