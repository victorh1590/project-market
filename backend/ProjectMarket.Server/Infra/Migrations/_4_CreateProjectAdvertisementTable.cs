using FluentMigrator;
using FluentMigrator.SqlServer;
using ProjectMarket.Server.Data.Model.Entity;
using ProjectMarket.Server.Infra.Repository;

namespace ProjectMarket.Server.Infra.Migrations;

[Migration(4)]
public class _4_CreateProjectAdvertisementTable(IConfiguration? configuration = null) : Migration {
    public override void Up()
	{
        Create.Table("ProjectAdvertisement")
            .WithColumn("ProjectAdvertisementId").AsInt32().Unique().PrimaryKey("pk_project_advertisement").Identity().NotNullable()
            .WithColumn("Title").AsString(128).NotNullable()
            .WithColumn("Description").AsString(512).Nullable()
            .WithColumn("OpenedOn").AsDateTime().NotNullable()
            .WithColumn("Deadline").AsDateTime().Nullable()
            .WithColumn("PaymentOfferId").AsInt32().NotNullable()
            .WithColumn("CustomerId").AsInt32().NotNullable()
            .WithColumn("AdvertisementStatusName").AsString(64).NotNullable();

        Create.ForeignKey("fk_project_advertisement_payment_offer")
            .FromTable("ProjectAdvertisement").ForeignColumn("PaymentOfferId")
            .ToTable("PaymentOffer").PrimaryColumn("PaymentOfferId");

        Create.ForeignKey("fk_project_advertisement_customer")
            .FromTable("ProjectAdvertisement").ForeignColumn("CustomerId")
            .ToTable("Customer").PrimaryColumn("CustomerId");

        Create.ForeignKey("fk_project_advertisement_advertisement_status")
            .FromTable("ProjectAdvertisement").ForeignColumn("AdvertisementStatusName")
            .ToTable("AdvertisementStatus").PrimaryColumn("AdvertisementStatusName");

        // Project Advertisement x Job Requirement
        Create.Table("ProjectAdvertisementJobRequirement")
            .WithColumn("ProjectAdvertisementId").AsInt32().NotNullable()
            .WithColumn("JobRequirementName").AsString(64).NotNullable();

        Create.PrimaryKey("pk_project_advertisement_job_requirement")
            .OnTable("ProjectAdvertisementJobRequirement")
            .Columns("ProjectAdvertisementId", "JobRequirementName");

        Create.ForeignKey("fk_project_advertisement_job_requirement")
            .FromTable("ProjectAdvertisementJobRequirement").ForeignColumn("ProjectAdvertisementId")
            .ToTable("ProjectAdvertisement").PrimaryColumn("ProjectAdvertisementId");

        Create.ForeignKey("fk_job_requirement_fk_project_advertisement")
            .FromTable("ProjectAdvertisementJobRequirement").ForeignColumn("JobRequirementName")
            .ToTable("JobRequirement").PrimaryColumn("JobRequirementName");

        // Project Advertisement X Knowledge Area
        Create.Table("ProjectAdvertisementKnowledgeArea")
            .WithColumn("ProjectAdvertisementId").AsInt32().NotNullable()
            .WithColumn("KnowledgeAreaName").AsString(64).NotNullable();

        Create.PrimaryKey("pk_project_advertisement_knowledge_area")
            .OnTable("ProjectAdvertisementKnowledgeArea")
            .Columns("ProjectAdvertisementId", "KnowledgeAreaName");

        Create.ForeignKey("fk_project_advertisement_knowledge_area")
            .FromTable("ProjectAdvertisementKnowledgeArea").ForeignColumn("ProjectAdvertisementId")
            .ToTable("ProjectAdvertisement").PrimaryColumn("ProjectAdvertisementId");

        Create.ForeignKey("fk_knowledge_area_project_advertisement")
            .FromTable("ProjectAdvertisementKnowledgeArea").ForeignColumn("KnowledgeAreaName")
            .ToTable("KnowledgeArea").PrimaryColumn("KnowledgeAreaName");
            
        if(configuration?.GetValue<bool>("Database:UseSeedData") ?? false) {
            DeleteAllRows();

            int projectadvertisementId = 0;

            // Insert 1.
            var projectAdvertisementId1 = Insert.IntoTable("ProjectAdvertisement").WithIdentityInsert()
            .Row(new {
                ProjectAdvertisementId = ++projectadvertisementId,
                Title = "Some AI Project",
                Description = "Something Something AI",
                OpenedOn = DateTime.Now,
                Deadline = DateTime.Now.AddMonths(2),
                PaymentOfferId = 1,
                CustomerId = 1,
                AdvertisementStatusName = "Open",
            });

            Insert.IntoTable("ProjectAdvertisementKnowledgeArea").Row(new {
                ProjectAdvertisementId = projectadvertisementId,
                KnowledgeAreaName = "AI"
            });

            Insert.IntoTable("ProjectAdvertisementJobRequirement").Row(new {
                ProjectAdvertisementId = projectadvertisementId,
                JobRequirementName = "Python"
            });

            // Insert 2.
            var projectAdvertisementId2 = Insert.IntoTable("ProjectAdvertisement").WithIdentityInsert()
            .Row(new {
                ProjectAdvertisementId = ++projectadvertisementId,
                Title = "Some Website",
                Description = "Website Description",
                OpenedOn = DateTime.Now.AddMonths(-2),
                Deadline = DateTime.Now.AddMonths(2),
                PaymentOfferId = 4,
                CustomerId = 4,
                AdvertisementStatusName = "Paused",
            });
                
            Insert.IntoTable("ProjectAdvertisementKnowledgeArea").Row(new {
                ProjectAdvertisementId = projectadvertisementId,
                KnowledgeAreaName = "Software Development"
            });

            Insert.IntoTable("ProjectAdvertisementKnowledgeArea").Row(new {
                ProjectAdvertisementId = projectadvertisementId,
                KnowledgeAreaName = "UI/UX"
            });

            Insert.IntoTable("ProjectAdvertisementJobRequirement").Row(new {
                ProjectAdvertisementId = projectadvertisementId,
                JobRequirementName = "Javascript"
            });

            // Insert 3.
            var projectAdvertisementId3 = Insert.IntoTable("ProjectAdvertisement").WithIdentityInsert()
            .Row(new {
                ProjectAdvertisementId = ++projectadvertisementId,
                Title = "Website Layout Creation",
                Description = "Create a layout for a website...",
                OpenedOn = DateTime.Now.AddMonths(-4),
                Deadline = DateTime.Now.AddMonths(-3),
                PaymentOfferId = 2,
                CustomerId = 2,
                AdvertisementStatusName = "Closed",
            });

            Insert.IntoTable("ProjectAdvertisementKnowledgeArea").Row(new {
                ProjectAdvertisementId = projectadvertisementId,
                KnowledgeAreaName = "UI/UX"
            });

            // Insert 4.
            var projectAdvertisementId4 = Insert.IntoTable("ProjectAdvertisement").WithIdentityInsert()
            .Row(new {
                ProjectAdvertisementId = ++projectadvertisementId,
                Title = "Analysing Data",
                Description = "Data analysis of something.",
                OpenedOn = DateTime.Now,
                PaymentOfferId = 3,
                CustomerId = 5,
                AdvertisementStatusName = "Open",
            });

            Insert.IntoTable("ProjectAdvertisementKnowledgeArea").Row(new {
                ProjectAdvertisementId = projectadvertisementId,
                KnowledgeAreaName = "Data Science"
            });

            Insert.IntoTable("ProjectAdvertisementKnowledgeArea").Row(new {
                ProjectAdvertisementId = projectadvertisementId,
                KnowledgeAreaName = "AI"
            });

            Insert.IntoTable("ProjectAdvertisementJobRequirement").Row(new {
                ProjectAdvertisementId = projectadvertisementId,
                JobRequirementName = "Python"
            });
        }
    }

	public override void Down()
	{  
        DeleteAllRows();

        Delete.ForeignKey("fk_project_advertisement_payment_offer").OnTable("ProjectAdvertisement");
        Delete.ForeignKey("fk_project_advertisement_customer").OnTable("ProjectAdvertisement");
        Delete.ForeignKey("fk_project_advertisement_status").OnTable("ProjectAdvertisement");
        Delete.Table("ProjectAdvertisement").IfExists();

        Delete.ForeignKey("fk_project_advertisement_job_requirement").OnTable("ProjectAdvertisementJobRequirement");
        Delete.ForeignKey("fk_job_requirement_fk_project_advertisement").OnTable("ProjectAdvertisementJobRequirement");        
        Delete.Table("ProjectAdvertisementJobRequirement").IfExists();

        Delete.ForeignKey("fk_project_advertisement_knowledge_area").OnTable("ProjectAdvertisementKnowledgeArea");
        Delete.ForeignKey("fk_knowledge_area_fk_project_advertisement").OnTable("ProjectAdvertisementKnowledgeArea");        
        Delete.Table("ProjectAdvertisementKnowledgeArea").IfExists();
	}

    public void DeleteAllRows() {
        Delete.FromTable("ProjectAdvertisementJobRequirement").AllRows();
        Delete.FromTable("ProjectAdvertisementKnowledgeArea").AllRows();

        Delete.FromTable("ProjectAdvertisement").AllRows();
    }
}