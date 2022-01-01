using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class FixEmptyFullTextSearchError : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var alterSearchLocationsFunction =
                "CREATE OR REPLACE FUNCTION search_locations(search_text text, location_type integer, take_limit integer DEFAULT 10) returns SETOF \"Locations\"     LANGUAGE plpgsql AS $$ DECLARE     parsed_ts_query varchar := plainto_tsquery(search_text)::varchar;     search_ts_query tsquery;  BEGIN   \n  -- If parsed query is empty, we cannot execute fulltext search  \n   IF parsed_ts_query <> '' THEN         search_ts_query := to_tsquery(parsed_ts_query || ':*');         RETURN QUERY SELECT loc.*                      FROM public.\"Locations\" as loc                      WHERE get_location_tsvector(\"Name\", \"Locality\", \"Country\") @@ search_ts_query                        AND \"Type\" = location_type                      ORDER BY ts_rank(get_location_tsvector(\"Name\", \"Locality\", \"Country\"), search_ts_query) DESC                      LIMIT (take_limit);     ELSE         RETURN QUERY SELECT * FROM public.\"Locations\" LIMIT 0;     END IF; END; $$;  ALTER FUNCTION search_locations(text, integer, integer) OWNER TO postgres;  ";
            migrationBuilder.Sql(alterSearchLocationsFunction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var alterSearchLocationsFunction =
                "create or replace function search_locations(search_text text, location_type integer, take_limit integer DEFAULT 10) returns SETOF \"Locations\"     language plpgsql as $$ DECLARE    search_ts_query tsquery := to_tsquery(plainto_tsquery(search_text)::varchar || ':*');     BEGIN  RETURN QUERY SELECT loc.* FROM public.\"Locations\" as loc WHERE get_location_tsvector(\"Name\", \"Locality\", \"Country\") @@ search_ts_query   and \"Type\" = location_type ORDER BY ts_rank(get_location_tsvector(\"Name\", \"Locality\", \"Country\"), search_ts_query) desc LIMIT (take_limit); END; $$;  alter function search_locations(text, integer, integer) owner to postgres; ";
            migrationBuilder.Sql(alterSearchLocationsFunction);
        }
    }
}
