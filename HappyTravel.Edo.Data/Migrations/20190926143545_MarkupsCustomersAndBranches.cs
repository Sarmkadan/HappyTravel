using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class MarkupsCustomersAndBranches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "CustomerCompanyRelations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CompanyId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarkupPolicies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Description = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    BranchId = table.Column<int>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    ScopeType = table.Column<int>(nullable: false),
                    Target = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    TemplateId = table.Column<int>(nullable: false),
                    TemplateSettings = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkupPolicies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Administrators",
                columns: new[] { "Id", "Created", "Email", "FirstName", "IdentityHash", "LastName", "Position", "Updated" },
                values: new object[] { -1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "testAdmin@happytravel.com", "FirstName", "postman", "LastName", "Position", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "Id", "CompanyId", "Created", "Modified", "Title" },
                values: new object[] { -1, -1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test branch" });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: -1,
                column: "PreferredCurrency",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CustomerCompanyRelations",
                keyColumns: new[] { "CustomerId", "CompanyId" },
                keyValues: new object[] { -1, -1 },
                column: "BranchId",
                value: -1);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CompanyId",
                table: "Branches",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkupPolicies_BranchId",
                table: "MarkupPolicies",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkupPolicies_CompanyId",
                table: "MarkupPolicies",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkupPolicies_CustomerId",
                table: "MarkupPolicies",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkupPolicies_ScopeType",
                table: "MarkupPolicies",
                column: "ScopeType");

            migrationBuilder.CreateIndex(
                name: "IX_MarkupPolicies_Target",
                table: "MarkupPolicies",
                column: "Target");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "MarkupPolicies");

            migrationBuilder.DeleteData(
                table: "Administrators",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "CustomerCompanyRelations");

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: -1,
                column: "PreferredCurrency",
                value: 0);
        }
    }
}
