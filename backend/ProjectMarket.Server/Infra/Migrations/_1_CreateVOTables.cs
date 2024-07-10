using FluentMigrator;

namespace ProjectMarket.Server.Infra.Migrations;

[Migration(1)]
public class _1_CreateVOTables : Migration {
    public override void Up()
	{
        Create.Table("AdvertisementStatus")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey("pk_advertisement_status").Identity()
            .WithColumn("Status").AsString(64).Unique().NotNullable();

        Create.Table("Currency")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey("pk_currency").Identity()
            .WithColumn("Name").AsString(64).Unique().NotNullable()
            .WithColumn("Prefix").AsString(8).NotNullable();

        Create.Table("JobRequirement")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey("pk_job_requirement").Identity()
            .WithColumn("Requirement").AsString(64).Unique().NotNullable();

        Create.Table("KnowledgeArea")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey("pk_knowledge_area").Identity()
            .WithColumn("Name").AsString(64).Unique().NotNullable();

        Create.Table("PaymentFrequency")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey("pk_knowledge_area").Identity()
            .WithColumn("Description").AsString(32).Unique().NotNullable()
            .WithColumn("Suffix").AsString(32).Unique().NotNullable();
	}

	public override void Down()
	{
        Delete.Table("AdvertisementStatus");
        Delete.Table("Currency");
        Delete.Table("JobRequirement");
        Delete.Table("KnowledgeArea");
	}
}