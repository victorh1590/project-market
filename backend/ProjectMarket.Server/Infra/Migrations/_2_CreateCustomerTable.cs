using FluentMigrator;

namespace ProjectMarket.Server.Infra.Migrations;

[Migration(2)]
public class _2_CreateCustomerTable : Migration {
    public override void Up()
	{
        int bcryptHashSize = 72;

        Create.Table("Costumer")
            .WithColumn("CostumerId").AsInt32().NotNullable().PrimaryKey("pk_costumer").Identity()
            .WithColumn("Name").AsString(64).Unique().NotNullable()
            .WithColumn("Email").AsString(128).NotNullable()
            .WithColumn("Password").AsString(bcryptHashSize).NotNullable()
            .WithColumn("RegistrationDate").AsDateTime().NotNullable();
	}

	public override void Down()
	{
        Delete.Table("Costumer").IfExists();
	}
}