using Npgsql;
using System.Data;

namespace ProjectMarket.Server.Infra.Db;

public sealed class UnitOfWork : IUnitOfWork
{
    public IDbConnection Connection { get; }
    private IDbTransaction? _transaction;

    public UnitOfWork(IConfiguration configuration, IDbConnection connection)
    {
        Connection = connection;
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