using FluentMigrator;

namespace ProjectMarket.Server.Infra.Migrations;

[Migration(4)]
public class _4_CreateProjectAdvertisementTable : Migration {
    public override void Up()
	{
        Create.Table("ProjectAdvertisement")
            .WithColumn("ProjectAdvertisementId").AsInt32().NotNullable().Unique().PrimaryKey("pk_project_advertisement").Identity()
            .WithColumn("Title").AsString(128).NotNullable()
            .WithColumn("Description").AsString(512).Nullable()
            .WithColumn("OpenedOn").AsDateTime().NotNullable()
            .WithColumn("Deadline").AsDateTime().Nullable()
            .WithColumn("PaymentOfferId").AsInt32().NotNullable()
            .WithColumn("CostumerId").AsInt32().NotNullable()
            .WithColumn("StatusId").AsInt32().NotNullable()
            .WithColumn("SubjectId").AsInt32().NotNullable()
            .WithColumn("ProjectAdvertisementJobRequirementId").AsInt32().Nullable();

        Create.ForeignKey("fk_project_advertisement_payment_offer")
            .FromTable("ProjectAdvertisement").ForeignColumn("PaymentOfferId")
            .ToTable("PaymentOffer").PrimaryColumn("PaymentOfferId");

        Create.ForeignKey("fk_project_advertisement_costumer")
            .FromTable("ProjectAdvertisement").ForeignColumn("CostumerId")
            .ToTable("Costumer").PrimaryColumn("CostumerId");

        Create.ForeignKey("fk_project_advertisement_advertisement_status")
            .FromTable("ProjectAdvertisement").ForeignColumn("StatusId")
            .ToTable("AdvertisementStatus").PrimaryColumn("StatusId");

        Create.ForeignKey("fk_project_advertisement_knowledge_area")
            .FromTable("ProjectAdvertisement").ForeignColumn("SubjectId")
            .ToTable("KnowledgeArea").PrimaryColumn("SubjectId");

        // Project Advertisement x Job Requirement
        Create.Table("ProjectAdvertisementJobRequirement")
            .WithColumn("ProjectAdvertisementJobRequirementId")
                .AsInt32().NotNullable().Unique()
                .PrimaryKey("pk_project_advertisement_job_requirement").Identity()
            .WithColumn("ProjectAdvertisementId").AsInt32().NotNullable()
            .WithColumn("JobRequirementId").AsInt32().NotNullable();

        Create.ForeignKey("fk_project_advertisement_job_requirement")
            .FromTable("ProjectAdvertisementJobRequirement").ForeignColumn("ProjectAdvertisementId")
            .ToTable("ProjectAdvertisement").PrimaryColumn("ProjectAdvertisementId");

        Create.ForeignKey("fk_job_requirement_fk_project_advertisement")
            .FromTable("ProjectAdvertisementJobRequirement").ForeignColumn("JobRequirementId")
            .ToTable("JobRequirement").PrimaryColumn("JobRequirementId");
    }

	public override void Down()
	{  
        Delete.ForeignKey("fk_project_advertisement_payment_offer").OnTable("ProjectAdvertisement");
        Delete.ForeignKey("fk_project_advertisement_costumer").OnTable("ProjectAdvertisement");
        Delete.ForeignKey("fk_project_advertisement_status").OnTable("ProjectAdvertisement");
        Delete.ForeignKey("fk_project_advertisement_subject").OnTable("ProjectAdvertisement");
        Delete.Table("ProjectAdvertisement").IfExists();

        Delete.ForeignKey("fk_project_advertisement_job_requirement").OnTable("ProjectAdvertisementJobRequirement");
        Delete.ForeignKey("fk_job_requirement_fk_project_advertisement").OnTable("ProjectAdvertisementJobRequirement");        
        Delete.Table("ProjectAdvertisementJobRequirement").IfExists();
	}
}