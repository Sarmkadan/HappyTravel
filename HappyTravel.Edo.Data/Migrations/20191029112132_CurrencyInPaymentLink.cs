using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class CurrencyInPaymentLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Currency", table: "PaymentLinks");
            
            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "PaymentLinks",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Currency", table: "PaymentLinks");
            
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "PaymentLinks",
                nullable: false);
        }
    }
}
