using Npgsql;
using System.Data;

public sealed class DbSession : IDisposable
{
    private Guid _id;
    public IDbConnection Connection { get; }
    public IDbTransaction? Transaction { get; private set; }
    private readonly IConfiguration _configuration;

    public DbSession(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _id = Guid.NewGuid();
        Connection = new NpgsqlConnection(_configuration.GetConnectionString("postgresql"));
        Connection.Open();
    }

    public IDbTransaction BeginTransaction()
    {
        Transaction ??= Connection.BeginTransaction();
        return Transaction;
    }

    public void Commit()
    {
        Transaction?.Commit();
        Transaction = null;
    }

    public void Rollback()
    {
        Transaction?.Rollback();
        Transaction = null;
    }

    public void Dispose()
    {
        if (Transaction != null)
        {
            Transaction.Rollback();
            Transaction.Dispose();
        }
        Connection.Dispose();
    }
}
