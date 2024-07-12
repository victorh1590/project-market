using FluentMigrator;

namespace ProjectMarket.Server.Infra.Migrations;

[Migration(3)]
public class _3_CreatePaymentOfferTable : Migration {
    public override void Up()
	{
        // MaxValue = 1_000_000.00
        int valueMaxSize = 7;
        int valuePrecision = 2;

        Create.Table("PaymentOffer")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey("pk_payment_offer").Identity()
            .WithColumn("Value").AsDecimal(valueMaxSize,valuePrecision).NotNullable() 
            .WithColumn("PaymentFrequencyId").AsInt32().NotNullable()
            .WithColumn("CurrencyId").AsInt32().NotNullable();

        Create.ForeignKey("fk_payment_offer_payment_frequency")
            .FromTable("PaymentOffer").ForeignColumn("PaymentFrequencyId")
            .ToTable("PaymentFrequency").PrimaryColumn("Id");

        Create.ForeignKey("fk_payment_offer_currency")
            .FromTable("PaymentOffer").ForeignColumn("CurrencyId")
            .ToTable("Currency").PrimaryColumn("Id");
	}

	public override void Down()
	{   
        Delete.ForeignKey("fk_payment_offer_payment_frequency").OnTable("PaymentOffer");
        Delete.ForeignKey("fk_payment_offer_currency").OnTable("PaymentOffer");
        Delete.Table("PaymentOffer").IfExists();
	}
}