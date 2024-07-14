using FluentMigrator;

namespace ProjectMarket.Server.Infra.Migrations;

[Migration(1)]
public class _1_CreateVOTables : Migration {
    public override void Up()
	{
        Create.Table("AdvertisementStatus")
            .WithColumn("AdvertisementStatusId").AsInt32().NotNullable().PrimaryKey("pk_advertisement_status").Identity()
            .WithColumn("Status").AsString(64).Unique().NotNullable();

        Create.Table("Currency")
            .WithColumn("CurrencyId").AsInt32().NotNullable().PrimaryKey("pk_currency").Identity()
            .WithColumn("Name").AsString(64).Unique().NotNullable()
            .WithColumn("Prefix").AsString(8).NotNullable();

        Create.Table("JobRequirement")
            .WithColumn("JobRequirementId").AsInt32().NotNullable().PrimaryKey("pk_job_requirement").Identity()
            .WithColumn("Requirement").AsString(64).Unique().NotNullable();

        Create.Table("KnowledgeArea")
            .WithColumn("KnowledgeAreaId").AsInt32().NotNullable().PrimaryKey("pk_knowledge_area").Identity()
            .WithColumn("Name").AsString(64).Unique().NotNullable();

        Create.Table("PaymentFrequency")
            .WithColumn("PaymentFrequencyId").AsInt32().NotNullable().PrimaryKey("pk_payment_frequency").Identity()
            .WithColumn("Description").AsString(32).Unique().NotNullable()
            .WithColumn("Suffix").AsString(32).Unique().NotNullable();
	}

	public override void Down()
	{
        Delete.Table("AdvertisementStatus").IfExists();
        Delete.Table("Currency").IfExists();
        Delete.Table("JobRequirement").IfExists();
        Delete.Table("KnowledgeArea").IfExists();
        Delete.Table("PaymentFrequency").IfExists();
	}
}