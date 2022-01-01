using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class AddCustomerInvitationRights : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var updateUserRights = "UPDATE edo.public.\"CustomerCompanyRelations\" SET \"InCompanyPermissions\" = 56 WHERE \"Type\" = 0";
            migrationBuilder.Sql(updateUserRights);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var updateUserRights = "UPDATE edo.public.\"CustomerCompanyRelations\" SET \"InCompanyPermissions\" = 48 WHERE \"Type\" = 0";
            migrationBuilder.Sql(updateUserRights);
        }
    }
}
