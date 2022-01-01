using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class AddServiceAccountServiceWorker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlRegions = @"insert into public.""ServiceAccounts"" (""ClientId"")
    VALUES ('serviceWorker' );";
            migrationBuilder.Sql(sqlRegions);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sqlRegions = @"delete from public.""ServiceAccounts""
    where ""ClientId"" = 'serviceWorker';";
            migrationBuilder.Sql(sqlRegions);
        }
    }
}
