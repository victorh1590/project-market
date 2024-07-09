using FluentMigrator;

namespace ProjectMarket.Server.Infra.Migrations;

[Migration(1)]
public class AddSectorAndUserTables : Migration {
    public override void Up()
	{
        Create.Table("Sector")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey("pk_sector").Identity()
            .WithColumn("Name").AsString(64).Unique().NotNullable();

		Create.Table("User")
			.WithColumn("Id").AsInt32().NotNullable().PrimaryKey("pk_user").Identity()
            .WithColumn("Name").AsString(64).Unique().NotNullable()
            .WithColumn("Email").AsString(128).NotNullable()
            .WithColumn("Password").AsString(72).NotNullable()
            .WithColumn("Sector").AsInt32().ForeignKey("fk_user_sector", "Sector", "Id").Nullable();
	}

	public override void Down()
	{
        Delete.Table("User");
        Delete.Table("Sector");
	}
}