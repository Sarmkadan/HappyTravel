using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class LocationsSearchLogicChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var dropOldIdx = "drop index \"IDX_GetLocationTsVector\"";
            migrationBuilder.Sql(dropOldIdx);

            var dropOldGetLocationTsVector = "drop function get_location_tsvector";
            migrationBuilder.Sql(dropOldGetLocationTsVector);

            var createNewGetLocationTsVector =
                "create function get_location_tsvector(name jsonb, locality jsonb, country jsonb) returns tsvector immutable language plpgsql as $$ BEGIN RETURN setweight(to_tsvector('simple', name), 'A') || ' ' || setweight(to_tsvector('simple', locality), 'B')  || ' ' || setweight(to_tsvector('simple', country), 'C'); END $$;";
            migrationBuilder.Sql(createNewGetLocationTsVector);
            
            var createNewIndex = "create index \"IDX_GetLocationTsVector\"    on \"Locations\" using gin(get_location_tsvector(\"Name\",\"Locality\", \"Country\"));";
            migrationBuilder.Sql(createNewIndex);

            var createSearchLocationFunction =
                "create function search_locations(search_text text, location_type integer, take_limit integer DEFAULT 10) returns SETOF \"Locations\"     language plpgsql as $$ DECLARE    search_ts_query tsquery := plainto_tsquery(search_text); BEGIN  RETURN QUERY SELECT loc.* FROM public.\"Locations\" as loc WHERE get_location_tsvector(\"Name\", \"Locality\", \"Country\") @@ search_ts_query   and \"Type\" = location_type ORDER BY ts_rank(get_location_tsvector(\"Name\", \"Locality\", \"Country\"), search_ts_query) desc LIMIT (take_limit); END; $$;  alter function search_locations(text, integer, integer) owner to postgres;  ";
            migrationBuilder.Sql(createSearchLocationFunction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropSearchLocation = "drop function search_locations";
            migrationBuilder.Sql(dropSearchLocation);
            
            var dropOldIdx = "drop index \"IDX_GetLocationTsVector\"";
            migrationBuilder.Sql(dropOldIdx);

            var dropOldGetLocationTsVector = "drop function get_location_tsvector";
            migrationBuilder.Sql(dropOldGetLocationTsVector);

            var createNewGetLocationTsVector =
                "create function get_location_tsvector(locality jsonb, name jsonb, country jsonb) returns tsvector immutable language plpgsql as $$ BEGIN RETURN setweight(to_tsvector('simple', locality), 'A') || ' ' || setweight(to_tsvector('simple', name), 'B')  || ' ' || setweight(to_tsvector('simple', country), 'C'); END $$;";
            migrationBuilder.Sql(createNewGetLocationTsVector);
        }
    }
}
