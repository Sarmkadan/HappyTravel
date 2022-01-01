using HappyTravel.Edo.Common.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class CompanyPermissionsRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permissions",
                table: "CustomerCompanyRelations");
            
            var defaultPermissions = InCompanyPermissions.AccommodationBooking |
                InCompanyPermissions.AccommodationAvailabilitySearch;

            migrationBuilder.AddColumn<int>(
                name: "InCompanyPermissions",
                table: "CustomerCompanyRelations",
                nullable: false,
                defaultValue: (int)defaultPermissions);
            
            migrationBuilder.Sql($"UPDATE \"CustomerCompanyRelations\" SET \"InCompanyPermissions\"={(int) InCompanyPermissions.All} WHERE \"Type\" = 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InCompanyPermissions",
                table: "CustomerCompanyRelations");

            migrationBuilder.AddColumn<int>(
                name: "Permissions",
                table: "CustomerCompanyRelations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CustomerCompanyRelations",
                keyColumns: new[] { "CustomerId", "CompanyId", "Type" },
                keyValues: new object[] { -1, -1, 1 },
                column: "Permissions",
                value: 62);
        }
    }
}
