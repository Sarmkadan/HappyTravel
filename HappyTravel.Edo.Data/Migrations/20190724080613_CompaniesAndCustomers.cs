using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class CompaniesAndCustomers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    CountryCode = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Fax = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    PreferredCurrency = table.Column<int>(nullable: false),
                    PreferredPaymentMethod = table.Column<int>(nullable: false),
                    Website = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Position = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    TokenHash = table.Column<string>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerCompanyRelations",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relations", x => new { x.CustomerId, x.CompanyId });
                });
            
            migrationBuilder.CreateIndex("token_btree_idx", "Customers", "TokenHash");
            migrationBuilder.CreateIndex("email_btree_idx", "Customers", "Email");
        }

        protected override void Down(MigrationBuilder builder)
        {
            builder.DropTable("CustomerCompanyRelations");
            builder.DropTable("Customers");
            builder.DropTable("Companies");
        }
    }
}
