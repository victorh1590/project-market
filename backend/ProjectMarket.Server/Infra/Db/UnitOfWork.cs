using Npgsql;
using System.Data;

namespace ProjectMarket.Server.Infra.Db;

public sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    public IDbConnection Connection { get; }
    private IDbTransaction? _transaction;

    public UnitOfWork(IConfiguration configuration) {
        Connection = new NpgsqlConnection(configuration.GetConnectionString("postgresql"));
        Connection.Open();
    }
    public void Begin() {
        _transaction = Connection.BeginTransaction();
    }

    public void Commit()
    {
        try
        {
            _transaction?.Commit();
        }
        catch
        {
            _transaction?.Rollback();
            throw;
        }
    }

    public void Rollback()
    {
        _transaction?.Rollback();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        Connection.Dispose();
    }
}