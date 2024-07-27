using FluentMigrator;

namespace ProjectMarket.Test.Integration.Migrations;

[Migration(2)]
public class _2_CreateInitialTable : Migration
{
    public override void Up()
    {
        Create.Table("Items")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Name").AsString(255).NotNullable();

        Insert.IntoTable("Items").Row(new { Id = 1, Name = "Something" });
    }

    public override void Down()
    {
        Delete.Table("Items");
    }
}