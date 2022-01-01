using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class PaymentLinkPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "PaymentLinks");

            migrationBuilder.RenameColumn(
                name: "Facility",
                table: "PaymentLinks",
                newName: "ReferenceCode");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPaymentDate",
                table: "PaymentLinks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastPaymentResponse",
                table: "PaymentLinks",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceType",
                table: "PaymentLinks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPaymentDate",
                table: "PaymentLinks");

            migrationBuilder.DropColumn(
                name: "LastPaymentResponse",
                table: "PaymentLinks");

            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "PaymentLinks");

            migrationBuilder.RenameColumn(
                name: "ReferenceCode",
                table: "PaymentLinks",
                newName: "Facility");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "PaymentLinks",
                nullable: false,
                defaultValue: false);
        }
    }
}
