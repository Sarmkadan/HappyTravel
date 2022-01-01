using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class AppliedMarkupsAndSupplierPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarkupLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ServiceType = table.Column<int>(nullable: false),
                    Policies = table.Column<string>(type: "jsonb", nullable: false),
                    ReferenceCode = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkupLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplierOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DataProvider = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ReferenceCode = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierOrders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarkupLog_ReferenceCode",
                table: "MarkupLog",
                column: "ReferenceCode");

            migrationBuilder.CreateIndex(
                name: "IX_MarkupLog_ServiceType",
                table: "MarkupLog",
                column: "ServiceType");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOrders_DataProvider",
                table: "SupplierOrders",
                column: "DataProvider");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOrders_ReferenceCode",
                table: "SupplierOrders",
                column: "ReferenceCode");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOrders_Type",
                table: "SupplierOrders",
                column: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarkupLog");

            migrationBuilder.DropTable(
                name: "SupplierOrders");
        }
    }
}
