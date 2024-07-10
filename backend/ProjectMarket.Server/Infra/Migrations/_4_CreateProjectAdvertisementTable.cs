using FluentMigrator;

namespace ProjectMarket.Server.Infra.Migrations;

[Migration(4)]
public class _4_CreateProjectAdvertisementTable : Migration {
    public override void Up()
	{
        Create.Table("ProjectAdvertisement")
            .WithColumn("Id").AsInt32().NotNullable().Unique().PrimaryKey("pk_ticket").Identity()
            .WithColumn("Title").AsString(128).NotNullable()
            .WithColumn("Description").AsString(512).Nullable()
            .WithColumn("OpenedOn").AsDateTime().NotNullable()
            .WithColumn("Deadline").AsDateTime().Nullable()
            .WithColumn("PaymentOfferId").AsInt32().NotNullable()
            .WithColumn("CostumerId").AsInt32().NotNullable()
            .WithColumn("StatusId").AsInt32().NotNullable()
            .WithColumn("SubjectId").AsInt32().NotNullable()
            .WithColumn("RequirementId").AsInt32().Nullable();

        Create.ForeignKey("fk_project_advertisement_payment_offer")
            .FromTable("ProjectAdvertisement").ForeignColumn("PaymentOfferId")
            .ToTable("PaymentOffer").PrimaryColumn("Id");

        Create.ForeignKey("fk_project_advertisement_costumer")
            .FromTable("ProjectAdvertisement").ForeignColumn("CostumerId")
            .ToTable("Costumer").PrimaryColumn("Id");

        Create.ForeignKey("fk_project_advertisement_status")
            .FromTable("ProjectAdvertisement").ForeignColumn("StatusId")
            .ToTable("Status").PrimaryColumn("Id");

        Create.ForeignKey("fk_project_advertisement_subject")
            .FromTable("ProjectAdvertisement").ForeignColumn("SubjectId")
            .ToTable("Subject").PrimaryColumn("Id");

        Create.ForeignKey("fk_project_advertisement_requirements")
            .FromTable("ProjectAdvertisement").ForeignColumn("RequirementsId")
            .ToTable("Requirements").PrimaryColumn("Id");
	}

	public override void Down()
	{  
        Delete.ForeignKey("fk_project_advertisement_payment_offer").OnTable("ProjectAdvertisement");
        Delete.ForeignKey("fk_project_advertisement_costumer").OnTable("ProjectAdvertisement");
        Delete.ForeignKey("fk_project_advertisement_status").OnTable("ProjectAdvertisement");
        Delete.ForeignKey("fk_project_advertisement_subject").OnTable("ProjectAdvertisement");
        Delete.ForeignKey("fk_project_advertisement_requirements").OnTable("ProjectAdvertisement");
        Delete.Table("PaymentOffer");
	}
}