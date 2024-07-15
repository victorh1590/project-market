using FluentMigrator;

namespace ProjectMarket.Server.Infra.Migrations;

[Migration(1)]
public class _1_CreateVOTables(IConfiguration configuration) : Migration {
    public override void Up()
	{
        Create.Table("AdvertisementStatus")
            .WithColumn("AdvertisementStatusName").AsString(64).PrimaryKey("pk_advertisement_status").NotNullable();

        Create.Table("Currency")
            .WithColumn("CurrencyName").AsString(64).PrimaryKey("pk_currency").NotNullable()
            .WithColumn("Prefix").AsString(8).NotNullable();

        Create.Table("JobRequirement")
            .WithColumn("JobRequirementName").AsString(64).PrimaryKey("pk_job_requirement").NotNullable();

        Create.Table("KnowledgeArea")
            .WithColumn("KnowledgeAreaName").AsString(64).PrimaryKey("pk_knowledge_area").NotNullable();

        Create.Table("PaymentFrequency")
            .WithColumn("PaymentFrequencyName").AsString(32).PrimaryKey("pk_payment_frequency").NotNullable()
            .WithColumn("Suffix").AsString(32).Unique().NotNullable();

        if(configuration.GetValue<bool>("Database:UseSeedData")) {
            DeleteAllRows();

            Insert.IntoTable("AdvertisementStatus").Row(new { AdvertisementStatusName = "Open" });
            Insert.IntoTable("AdvertisementStatus").Row(new { AdvertisementStatusName = "Paused" });
            Insert.IntoTable("AdvertisementStatus").Row(new { AdvertisementStatusName = "Closed" });

            Insert.IntoTable("Currency").Row(new { CurrencyName = "Dollar", Prefix = "$" });
            Insert.IntoTable("Currency").Row(new { CurrencyName = "Euro", Prefix = "€" });
            Insert.IntoTable("Currency").Row(new { CurrencyName = "Yen", Prefix = "¥" });

            Insert.IntoTable("JobRequirement").Row(new { JobRequirementName = "Python" });
            Insert.IntoTable("JobRequirement").Row(new { JobRequirementName = "C#" });
            Insert.IntoTable("JobRequirement").Row(new { JobRequirementName = "Java" });
            Insert.IntoTable("JobRequirement").Row(new { JobRequirementName = "Go" });
            Insert.IntoTable("JobRequirement").Row(new { JobRequirementName = "Javascript" });
            Insert.IntoTable("JobRequirement").Row(new { JobRequirementName = "PHP" });
            Insert.IntoTable("JobRequirement").Row(new { JobRequirementName = "Ruby" });
            Insert.IntoTable("JobRequirement").Row(new { JobRequirementName = "C++" });

            Insert.IntoTable("KnowledgeArea").Row(new { KnowledgeAreaName = "Software Development" });
            Insert.IntoTable("KnowledgeArea").Row(new { KnowledgeAreaName = "AI" });
            Insert.IntoTable("KnowledgeArea").Row(new { KnowledgeAreaName = "Data Science" });
            Insert.IntoTable("KnowledgeArea").Row(new { KnowledgeAreaName = "DevOps" });
            Insert.IntoTable("KnowledgeArea").Row(new { KnowledgeAreaName = "UI/UX" });

            Insert.IntoTable("PaymentFrequency").Row(new { PaymentFrequencyName = "Hourly", Suffix = "per hour" });
            Insert.IntoTable("PaymentFrequency").Row(new { PaymentFrequencyName = "Daily", Suffix = "per day" });
            Insert.IntoTable("PaymentFrequency").Row(new { PaymentFrequencyName = "Once", Suffix = "when project is done" });
        }
	}

	public override void Down()
	{
        DeleteAllRows();

        Delete.Table("AdvertisementStatus").IfExists();
        Delete.Table("Currency").IfExists();
        Delete.Table("JobRequirement").IfExists();
        Delete.Table("KnowledgeArea").IfExists();
        Delete.Table("PaymentFrequency").IfExists();
	}

    public void DeleteAllRows() {
        Delete.FromTable("AdvertisementStatus").AllRows();
        Delete.FromTable("Currency").AllRows();
        Delete.FromTable("JobRequirement").AllRows();
        Delete.FromTable("KnowledgeArea").AllRows();
        Delete.FromTable("PaymentFrequency").AllRows();
    }
}