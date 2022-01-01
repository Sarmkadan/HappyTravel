using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Edo.Data.Migrations
{
    public partial class LocationsWeightCalculationChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var alterGetLocationTsVector =
                "create or replace function get_location_tsvector(name jsonb, locality jsonb, country jsonb) returns tsvector     immutable     language plpgsql as $$ DECLARE         country_vector tsvector;         locality_vektor tsvector;         name_vektor tsvector; BEGIN     country_vector = to_tsvector('simple', country);     locality_vektor = to_tsvector('simple', locality);      IF locality_vektor = '' THEN         locality_vektor = country_vector;     END IF;      name_vektor = to_tsvector('simple', name);     IF name_vektor = '' THEN         name_vektor = locality_vektor;     END IF;     RETURN setweight(name_vektor, 'A') || setweight(locality_vektor, 'B')  || setweight(country_vector, 'C'); END $$;  alter function get_location_tsvector(jsonb, jsonb, jsonb) owner to postgres;";

            migrationBuilder.Sql(alterGetLocationTsVector);

            var alterSearchLocation =
                "create or replace function search_locations(search_text text, location_type integer, take_limit integer DEFAULT 10) returns SETOF \"Locations\"     language plpgsql as $$ DECLARE    search_ts_query tsquery := to_tsquery(plainto_tsquery(search_text)::varchar || ':*');     BEGIN  RETURN QUERY SELECT loc.* FROM public.\"Locations\" as loc WHERE get_location_tsvector(\"Name\", \"Locality\", \"Country\") @@ search_ts_query   and \"Type\" = location_type ORDER BY ts_rank(get_location_tsvector(\"Name\", \"Locality\", \"Country\"), search_ts_query) desc LIMIT (take_limit); END; $$;  alter function search_locations(text, integer, integer) owner to postgres; ";
            migrationBuilder.Sql(alterSearchLocation);

            var reindexSql = "REINDEX INDEX \"IDX_GetLocationTsVector\"";
            migrationBuilder.Sql(reindexSql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var alterGetLocationTsVector =
                "create or replace function get_location_tsvector(name jsonb, locality jsonb, country jsonb) returns tsvector     immutable     language plpgsql as $$ BEGIN     RETURN setweight(to_tsvector('simple', name), 'A') || ' ' || setweight(to_tsvector('simple', locality), 'B')  || ' ' || setweight(to_tsvector('simple', country), 'C'); END $$;  alter function get_location_tsvector(jsonb, jsonb, jsonb) owner to postgres;  ";

            var reindexSql = "REINDEX INDEX \"IDX_GetLocationTsVector\"";
            migrationBuilder.Sql(reindexSql);
        }
    }
}
