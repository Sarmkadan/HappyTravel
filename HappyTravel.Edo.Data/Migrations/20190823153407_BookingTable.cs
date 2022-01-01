using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class BookingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    AgentReference = table.Column<string>(nullable: true),
                    ReferenceCode = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    BookingDate = table.Column<DateTime>(nullable: false),
                    Nationality = table.Column<string>(nullable: true),
                    Residency = table.Column<string>(nullable: true),
                    ItineraryNumber = table.Column<long>(nullable: false),
                    MainPassengerName = table.Column<string>(nullable: false),
                    ServiceType = table.Column<int>(nullable: false),
                    PaymentMethod = table.Column<int>(nullable: false),
                    BookingDetails = table.Column<string>(type: "jsonb", nullable: true),
                    ServiceDetails = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CompanyId",
                table: "Bookings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CustomerId",
                table: "Bookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ItineraryNumber",
                table: "Bookings",
                column: "ItineraryNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MainPassengerName",
                table: "Bookings",
                column: "MainPassengerName");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ReferenceCode",
                table: "Bookings",
                column: "ReferenceCode");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ServiceType",
                table: "Bookings",
                column: "ServiceType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}
