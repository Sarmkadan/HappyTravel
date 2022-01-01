using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class FuncAndIdxForLocationSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION get_location_tsvector(locality jsonb, name jsonb, country jsonb)
                                    RETURNS tsvector AS $$
                                    BEGIN
                                        RETURN setweight(to_tsvector('simple', locality), 'A') || ' ' || setweight(to_tsvector('simple', name), 'B')  || ' ' || setweight(to_tsvector('simple', country), 'C');
                                    END
                                    $$ LANGUAGE 'plpgsql' IMMUTABLE;");

            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS \"IDX_GetLocationTsVector\" ON \"Locations\" USING gin(get_location_tsvector(\"Locality\", \"Name\", \"Country\"));");
        }
        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX \"IDX_GetLocationTsVector\";");
            migrationBuilder.Sql("DROP FUNCTION get_location_tsvector;");
        }
    }
}
