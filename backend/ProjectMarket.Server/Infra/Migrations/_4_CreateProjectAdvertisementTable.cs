using FluentMigrator;
using FluentMigrator.SqlServer;
using ProjectMarket.Server.Data.Model.Entity;

namespace ProjectMarket.Server.Infra.Migrations;

[Migration(4)]
public class _4_CreateProjectAdvertisementTable(IConfiguration configuration) : Migration {
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
            .WithColumn("StatusId").AsInt32().NotNullable();

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
            .WithColumn("ProjectAdvertisementId").AsInt32().NotNullable()
            .WithColumn("JobRequirementName").AsString(64).NotNullable();

        Create.PrimaryKey("pk_project_advertisement_job_requirement")
            .OnTable("ProjectAdvertisementJobRequirement")
            .Columns("ProjectAdvertisementId", "JobRequirementName");

        Create.ForeignKey("pk_project_advertisement_job_requirement")
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

        Create.ForeignKey("pk_project_advertisement_knowledge_area")
            .FromTable("ProjectAdvertisementKnowledgeArea").ForeignColumn("ProjectAdvertisementId")
            .ToTable("ProjectAdvertisement").PrimaryColumn("ProjectAdvertisementId");

        Create.ForeignKey("fk_knowledge_area_fk_project_advertisement")
            .FromTable("ProjectAdvertisementKnowledgeArea").ForeignColumn("KnowledgeAreaName")
            .ToTable("KnowledgeArea").PrimaryColumn("KnowledgeAreaName");
            
        if(configuration.GetValue<bool>("Database:UseSeedData")) {
            DeleteAllRows();

            // Insert 1.
            var projectAdvertisementId1 = Insert.IntoTable("ProjectAdvertisement").Row(new {
                Title = "Some AI Project",
                Description = "Something Something AI",
                OpenedOn = DateTime.Now,
                Deadline = DateTime.Now.AddMonths(2),
                PaymentOfferId = 1,
                Costumer = 1,
                Status = "Open",
            }).WithIdentityInsert();

            Insert.IntoTable("ProjectAdvertisementKnowledgeArea").Row(new {
                ProjectAdvertisement = projectAdvertisementId1,
                JobRequirement = "AI"
            });

            Insert.IntoTable("ProjectAdvertisementJobRequirement").Row(new {
                ProjectAdvertisement = projectAdvertisementId1,
                JobRequirement = "Python"
            });

            // Insert 2.
            var projectAdvertisementId2 = Insert.IntoTable("ProjectAdvertisement").Row(new {
                Title = "Some Website",
                Description = "Website Description",
                OpenedOn = DateTime.Now.AddMonths(-2),
                Deadline = DateTime.Now.AddMonths(2),
                PaymentOfferId = 4,
                Costumer = 4,
                Status = "Paused",
            }).WithIdentityInsert();
            
            Insert.IntoTable("ProjectAdvertisementKnowledgeArea").Row(new {
                ProjectAdvertisement = projectAdvertisementId2,
                JobRequirement = "Software Development"
            });

            Insert.IntoTable("ProjectAdvertisementKnowledgeArea").Row(new {
                ProjectAdvertisement = projectAdvertisementId2,
                JobRequirement = "UI/UX"
            });

            Insert.IntoTable("ProjectAdvertisementJobRequirement").Row(new {
                ProjectAdvertisement = projectAdvertisementId2,
                JobRequirement = "Javascript"
            });

            // Insert 3.
            var projectAdvertisementId3 = Insert.IntoTable("ProjectAdvertisement").Row(new {
                Title = "Website Layout Creation",
                Description = "Create a layout for a website...",
                OpenedOn = DateTime.Now.AddMonths(-4),
                Deadline = DateTime.Now.AddMonths(-3),
                PaymentOfferId = 2,
                Costumer = 2,
                Status = "Closed",
            }).WithIdentityInsert();

            Insert.IntoTable("ProjectAdvertisementKnowledgeArea").Row(new {
                ProjectAdvertisement = projectAdvertisementId3,
                JobRequirement = "UI/UX"
            });

            // Insert 4.
            var projectAdvertisementId4 = Insert.IntoTable("ProjectAdvertisement").Row(new {
                Title = "Analysing Data",
                Description = "Data analysis of something.",
                OpenedOn = DateTime.Now,
                PaymentOfferId = 3,
                Costumer = 5,
                Status = "Open",
                Subject = "Data Science",
                Requirement = "Python"
            }).WithIdentityInsert();

            Insert.IntoTable("ProjectAdvertisementKnowledgeArea").Row(new {
                ProjectAdvertisement = projectAdvertisementId4,
                JobRequirement = "Data Science"
            });

            Insert.IntoTable("ProjectAdvertisementKnowledgeArea").Row(new {
                ProjectAdvertisement = projectAdvertisementId4,
                JobRequirement = "AI"
            });

            Insert.IntoTable("ProjectAdvertisementJobRequirement").Row(new {
                ProjectAdvertisement = projectAdvertisementId4,
                JobRequirement = "Python"
            });
        }
    }

	public override void Down()
	{  
        DeleteAllRows();

        Delete.ForeignKey("fk_project_advertisement_payment_offer").OnTable("ProjectAdvertisement");
        Delete.ForeignKey("fk_project_advertisement_costumer").OnTable("ProjectAdvertisement");
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