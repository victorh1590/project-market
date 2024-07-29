using ProjectMarket.Server.Infra.Db;

namespace ProjectMarket.Server.Infra.DependencyInjection;

public static class DbmsNameExtensions
{
    private static readonly String ConnectionStringPattern = "CONNECTIONSTRING__";
    public static String GetConnectionStringName(this DbmsName dbmsName) => ConnectionStringPattern + dbmsName.ToString().ToUpperInvariant();
}