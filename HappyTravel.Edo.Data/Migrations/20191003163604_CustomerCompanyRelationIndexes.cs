using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class CustomerCompanyRelationIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Relations",
                table: "CustomerCompanyRelations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerCompanyRelations",
                table: "CustomerCompanyRelations",
                columns: new[] { "CustomerId", "CompanyId", "Type" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerCompanyRelations",
                table: "CustomerCompanyRelations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Relations",
                table: "CustomerCompanyRelations",
                columns: new[] { "CustomerId", "CompanyId" });
        }
    }
}
