using FluentMigrator;

namespace SupportCentral.Server.Infra.Migrations;

[Migration(2)]
public class AddTicketStatusAndProblemTypeTables : Migration {
    public override void Up()
	{
        Create.Table("ProblemType")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey("pk_problem_type").Identity()
            .WithColumn("Type").AsString(64).Unique().NotNullable();

		Create.Table("TicketStatus")
			.WithColumn("Id").AsInt32().NotNullable().PrimaryKey("pk_ticket_status").Identity()
            .WithColumn("Status").AsString(64).Unique().NotNullable();
	}

	public override void Down()
	{
        Delete.Table("TicketStatus");
        Delete.Table("ProblemType");
	}
}