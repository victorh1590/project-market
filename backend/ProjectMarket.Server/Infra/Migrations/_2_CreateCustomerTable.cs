using FluentMigrator;
using FluentMigrator.SqlServer;

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

            DeleteAllRows();

            Insert.IntoTable("Costumer").Row(new { 
                Name = "Adam", Email = "example@example.com", Password = "$2a$04$vo0GaDyEPfOb9f6gqviWh.UZLnabjN/cUEeBV5j21mLXSlngv4LyS", RegistrationDate = DateTime.Now 
            }).WithIdentityInsert();
            Insert.IntoTable("Costumer").Row(new { 
                Name = "Alice Johnson", Email = "alice.johnson@example.com", Password = "password1", RegistrationDate = DateTime.Now 
            }).WithIdentityInsert();
            Insert.IntoTable("Costumer").Row(new { 
                Name = "Bob Smith", Email = "bob.smith@example.com", Password = "password2", RegistrationDate = DateTime.Now 
            }).WithIdentityInsert();
            Insert.IntoTable("Costumer").Row(new { 
                Name = "Charlie Brown", Email = "charlie.brown@example.com", Password = "password3", RegistrationDate = DateTime.Now 
            }).WithIdentityInsert();
            Insert.IntoTable("Costumer").Row(new { 
                Name = "David Wilson", Email = "david.wilson@example.com", Password = "password4", RegistrationDate = DateTime.Now 
            }).WithIdentityInsert();
            Insert.IntoTable("Costumer").Row(new { 
                Name = "Emma Davis", Email = "emma.davis@example.com", Password = "password5", RegistrationDate = DateTime.Now
            }).WithIdentityInsert();
            Insert.IntoTable("Costumer").Row(new { 
                Name = "Frank Miller", Email = "frank.miller@example.com", Password = "password6", RegistrationDate = DateTime.Now
            }).WithIdentityInsert();
            Insert.IntoTable("Costumer").Row(new {
                Name = "Grace Lee", Email = "grace.lee@example.com", Password = "password7", RegistrationDate = DateTime.Now 
            }).WithIdentityInsert();         
            Insert.IntoTable("Costumer").Row(new {
                Name = "Henry Thompson", Email = "henry.thompson@example.com", Password = "password8", RegistrationDate = DateTime.Now 
            }).WithIdentityInsert();
            Insert.IntoTable("Costumer").Row(new {
                Name = "Ivy Martinez", Email = "ivy.martinez@example.com", Password = "password9", RegistrationDate = DateTime.Now 
            }).WithIdentityInsert();
            Insert.IntoTable("Costumer").Row(new {
                Name = "Jack White", Email = "jack.white@example.com", Password = "password10", RegistrationDate = DateTime.Now 
            }).WithIdentityInsert();

        }
	}

	public override void Down()
	{
        DeleteAllRows();

        Delete.Table("Costumer").IfExists();
	}
    
    public void DeleteAllRows() {
        Delete.FromTable("Costumer").AllRows();
    }
}