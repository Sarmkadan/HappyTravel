using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class CustomerInvitationsToUserInvitations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerInvitations");

            migrationBuilder.CreateTable(
                name: "UserInvitations",
                columns: table => new
                {
                    CodeHash = table.Column<string>(nullable: false),
                    Data = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    IsAccepted = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInvitations", x => x.CodeHash);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInvitations");

            migrationBuilder.CreateTable(
                name: "CustomerInvitations",
                columns: table => new
                {
                    CodeHash = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    IsAccepted = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInvitations", x => x.CodeHash);
                });
        }
    }
}
