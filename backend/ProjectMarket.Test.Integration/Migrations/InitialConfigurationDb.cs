using FluentMigrator;

namespace ProjectMarket.Test.Integration.Migrations;

[Migration(1)]
public class InitialConfigurationDb : Migration
{
    public override void Up()
    {
        Execute.Sql("ALTER DATABASE postgres SET search_path TO postgres, public;");
        Execute.Sql("ALTER ROLE ALL SET search_path TO postgres, public;");
    }

    public override void Down()
    {
    }
}