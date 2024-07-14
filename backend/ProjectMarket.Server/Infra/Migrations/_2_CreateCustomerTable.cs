using FluentMigrator;

namespace ProjectMarket.Server.Infra.Migrations;

[Migration(2)]
public class _2_CreateCustomerTable(IConfiguration configuration) : Migration {

    public override void Up()
	{
        int bcryptHashSize = 72;

        Create.Table("Costumer")
            .WithColumn("CostumerId").AsInt32().NotNullable().PrimaryKey("pk_costumer").Identity()
            .WithColumn("Name").AsString(64).Unique().NotNullable()
            .WithColumn("Email").AsString(128).NotNullable()
            .WithColumn("Password").AsString(bcryptHashSize).NotNullable()
            .WithColumn("RegistrationDate").AsDateTime().NotNullable();

        if(configuration.GetValue<bool>("Database:UseSeedData")) {
            Insert.IntoTable("Costumer").Row(new { Name = "Adam", Email = "example@example.com", Password = "", RegistrationDate = DateTime.Now });
        }
	}

	public override void Down()
	{
        Delete.Table("Costumer").IfExists();
	}
}