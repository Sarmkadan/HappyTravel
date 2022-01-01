using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class MarkupPoliciesAndCurrencies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "MarkupPolicies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CurrencyRates",
                columns: table => new
                {
                    SourceCurrency = table.Column<int>(nullable: false),
                    TargetCurrency = table.Column<int>(nullable: false),
                    ValidTo = table.Column<DateTime>(nullable: false),
                    Rate = table.Column<decimal>(nullable: false),
                    ValidFrom = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRates", x => new { x.SourceCurrency, x.TargetCurrency, x.ValidTo });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyRates");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "MarkupPolicies");
        }
    }
}
