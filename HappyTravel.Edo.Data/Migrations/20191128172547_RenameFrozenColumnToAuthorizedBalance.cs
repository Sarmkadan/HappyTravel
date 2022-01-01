using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class RenameFrozenColumnToAuthorizedBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Frozen",
                table: "PaymentAccounts",
                newName: "AuthorizedBalance");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorizedBalance",
                table: "PaymentAccounts",
                newName: "Frozen");
        }
    }
}
