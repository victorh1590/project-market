using System.Data;

namespace SupportCentral.Server.Infra.Db;
internal interface IUseDbSession
{
    public IDbConnection Connection { get; }
    public void Dispose();
}