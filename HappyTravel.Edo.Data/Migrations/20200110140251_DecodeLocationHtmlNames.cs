using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class DecodeLocationHtmlNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var decodeHotelNamesSql =
                "UPDATE \"Locations\"\nSET \"Name\" = replace(replace(replace(replace(replace(\"Name\"::varchar, '&amp;', '&'), '&#241;', 'ñ'), '&quot;', '\\\"'),'&NBSP;', ''), '&AMP;', '&')::jsonb\nwhere \"Name\"::varchar like '%&%' and \"Name\"::varchar like '%;%';";
            migrationBuilder.Sql(decodeHotelNamesSql);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
