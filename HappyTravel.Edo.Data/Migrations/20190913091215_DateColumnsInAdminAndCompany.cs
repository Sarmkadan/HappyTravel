using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class DateColumnsInAdminAndCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Companies",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Administrators",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Administrators",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            
            migrationBuilder.Sql(
                "UPDATE public.\"Companies\" SET \"Updated\" = '2019-09-01';");
            
            migrationBuilder.Sql(
                "UPDATE public.\"Administrators\" SET \"Created\" = '2019-09-01'; UPDATE public.\"Administrators\" SET \"Updated\" = '2019-09-01';");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Administrators");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Administrators");
        }
    }
}
