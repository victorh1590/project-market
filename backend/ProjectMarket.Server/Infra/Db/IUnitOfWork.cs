using System.Data;

namespace ProjectMarket.Server.Infra.Db;

public interface IUnitOfWork : IDisposable
{
    public IDbConnection Connection { get; }
    public void Begin();
    public void Commit();
    public void Rollback();
}
