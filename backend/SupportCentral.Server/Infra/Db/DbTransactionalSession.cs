using Npgsql;
using System.Data;

namespace SupportCentral.Server.Infra.Db;

public sealed class DbTransactionalSession : IDisposable, IUseDbSession
{
    private IDbTransaction Transaction { get; set; }
    private readonly IConfiguration _configuration;
    public IDbConnection Connection { get; }

    public DbTransactionalSession(IConfiguration configuration) 
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        Connection = new NpgsqlConnection(_configuration.GetConnectionString("postgresql"));
        Connection.Open();
        Transaction = Connection.BeginTransaction();
    }
    public void Commit() => Transaction.Commit();
    public void Rollback() => Transaction.Rollback();
    public void Dispose()
    {
        Transaction.Rollback();
        Transaction.Dispose();
        Connection.Dispose();
    }
}