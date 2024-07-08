using Npgsql;
using System.Data;

namespace SupportCentral.Server.Infra.Db;

public sealed class DbSession : IDisposable, IUseDbSession
{
    private readonly IConfiguration _configuration;
    public IDbConnection Connection { get; }
    public DbSession(IConfiguration configuration) 
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        Connection = new NpgsqlConnection(_configuration.GetConnectionString("postgresql"));
        Connection.Open();
    }
    public void Dispose() => Connection.Dispose();

}