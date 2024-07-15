using FluentMigrator;
using FluentMigrator.SqlServer;

namespace ProjectMarket.Server.Infra.Migrations;

[Migration(2)]
public class _2_CreateCustomerTable(IConfiguration configuration) : Migration {

    public override void Up()
	{
        int bcryptHashSize = 72;

        Create.Table("Costumer")
            .WithColumn("CostumerId").AsInt32().PrimaryKey("pk_costumer").Identity().NotNullable()
            .WithColumn("Name").AsString(64).Unique().NotNullable()
            .WithColumn("Email").AsString(128).NotNullable()
            .WithColumn("Password").AsString(bcryptHashSize).NotNullable()
            .WithColumn("RegistrationDate").AsDateTime().NotNullable();

        if(configuration.GetValue<bool>("Database:UseSeedData")) {

            DeleteAllRows();

            Insert.IntoTable("Costumer").Row(new { 
                Name = "Adam", Email = "example@example.com", 
                Password = "$2a$04$vo0GaDyEPfOb9f6gqviWh.UZLnabjN/cUEeBV5j21mLXSlngv4LyS", 
                RegistrationDate = DateTime.Now 
            });
            Insert.IntoTable("Costumer").Row(new { 
                Name = "Alice Johnson", Email = "alice.johnson@example.com", 
                Password = "$2a$04$ythI9xOhEmXaHsCiJ.Jh0ugqBohSpFlkjJJlozkiBcoDqC1SmQS1.", 
                RegistrationDate = DateTime.Now 
            });
            Insert.IntoTable("Costumer").Row(new { 
                Name = "Bob Smith", Email = "bob.smith@example.com", 
                Password = "$2a$04$iGLiwDHKP81cPawqX.vEh.y7XK0u1qwhz8Z1uIkLyLAoKCCUL7pOW", 
                RegistrationDate = DateTime.Now 
            });
            Insert.IntoTable("Costumer").Row(new { 
                Name = "Charlie Brown", Email = "charlie.brown@example.com", 
                Password = "$2a$04$u9oWbsDKeAyV8a902.57iuMRtlDQOOtxyPfYWSz4RTdZ8.faFvyxW", 
                RegistrationDate = DateTime.Now 
            });
            Insert.IntoTable("Costumer").Row(new { 
                Name = "David Wilson", Email = "david.wilson@example.com", 
                Password = "$2a$04$7Tch3wNL3rDZqbZo7tO9/u7iNZmz1Pqb52nmNcm4grzyz.7kvHQiS", 
                RegistrationDate = DateTime.Now 
            });
            Insert.IntoTable("Costumer").Row(new { 
                Name = "Emma Davis", Email = "emma.davis@example.com", 
                Password = "$2a$04$ChRprKplMqJAKRD15N0vUuw.HK0LkxNLcrIMv88967J9N8tgDszba", 
                RegistrationDate = DateTime.Now
            });
            Insert.IntoTable("Costumer").Row(new { 
                Name = "Frank Miller", Email = "frank.miller@example.com", 
                Password = "$2a$04$Rb9XEaVTLhNlY75mpaVPaetbhY/UwcTUN.Jt/5FN6BNCex0RSOb9G", 
                RegistrationDate = DateTime.Now
            });
            Insert.IntoTable("Costumer").Row(new {
                Name = "Grace Lee", Email = "grace.lee@example.com", 
                Password = "$2a$04$UNXL4rXerkg7YQnDJRJYPuPWVYDmyiSVifRnb8LvOaCMujhYn1naC", 
                RegistrationDate = DateTime.Now 
            });         
            Insert.IntoTable("Costumer").Row(new {
                Name = "Henry Thompson", Email = "henry.thompson@example.com", 
                Password = "$2a$04$QBBN1Wo8U46A/7OJIASFCutH64iE3s42nEE411qRXicxa8jYQllYS", 
                RegistrationDate = DateTime.Now 
            });
            Insert.IntoTable("Costumer").Row(new {
                Name = "Ivy Martinez", Email = "ivy.martinez@example.com",
                Password = "$2a$04$.a6d2TTE121rHPju9A16fumBMfjSqoMXsdRL7PL3Ye5IeWr2pR0x.", 
                RegistrationDate = DateTime.Now 
            });
            Insert.IntoTable("Costumer").Row(new {
                Name = "Jack White", Email = "jack.white@example.com", 
                Password = "$2a$04$WVPOSbzu6xBjz1dDvdTHEO8RvMJUsAPnHpHxT8i.Ud8Kvj12gAbjW", 
                RegistrationDate = DateTime.Now 
            });
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