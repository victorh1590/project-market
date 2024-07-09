using FluentMigrator;
using SupportCentral.Server.Data.Model;

namespace SupportCentral.Server.Infra.Migrations;

[Migration(3)]
public class AddTicketTable : Migration {
    public override void Up()
	{
        Create.Table("Ticket")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey("pk_ticket").Identity()
            .WithColumn("Title").AsString(128).NotNullable()
            .WithColumn("Description").AsString(512).Nullable()
            .WithColumn("ToSector").AsInt32().ForeignKey("fk_ticket_sector", "Sector", "Id").NotNullable()
            .WithColumn("User").AsInt32().ForeignKey("fk_ticket_user", "User", "Id").NotNullable()
            .WithColumn("Attendant").AsInt32().ForeignKey("fk_ticket_user_attendant", "User", "Id").Nullable()
            .WithColumn("Type").AsInt32().ForeignKey("fk_ticket_problem_type", "ProblemType", "Id").NotNullable()
            .WithColumn("Status").AsInt32().ForeignKey("fk_ticket_ticket_status", "TicketStatus", "Id").NotNullable()
            .WithColumn("Paused").AsBoolean().NotNullable()
            .WithColumn("OpenedOn").AsDateTime().NotNullable()
            .WithColumn("ClosedOn").AsDateTime().NotNullable();
	}

	public override void Down()
	{
        Delete.Table("Ticket");
	}
}