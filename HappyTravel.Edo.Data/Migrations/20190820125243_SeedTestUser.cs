using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class SeedTestUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Address", "City", "CountryCode", "Fax", "Name", "Phone", "PostalCode", "PreferredCurrency", "PreferredPaymentMethod", "State", "Website" },
                values: new object[] { -1, "Address", "City", "IT", "Fax", "Test company", "Phone", "400055", 0, 1, 0, "https://happytravel.com" });

            migrationBuilder.InsertData(
                table: "CustomerCompanyRelations",
                columns: new[] { "CustomerId", "CompanyId", "Type" },
                values: new object[] { -1, -1, 1 });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "FirstName", "IdentityHash", "LastName", "Position", "Title" },
                values: new object[] { -1, "test@happytravel.com", "FirstName", "postman", "LastName", "Position", "Mr." });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "CustomerCompanyRelations",
                keyColumns: new[] { "CustomerId", "CompanyId" },
                keyValues: new object[] { -1, -1 });

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: -1);
        }
    }
}
