using System.Collections.Generic;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.EdoContracts.GeoData.Enums;
using HappyTravel.Geography;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Locations
{
    public readonly struct Location
    {
        [JsonConstructor]
        public Location(string name, string locality, string country, GeoPoint coordinates, int distance, PredictionSources source, LocationTypes type,
            List<DataProviders> dataProviders)
        {
            Name = name;
            Locality = locality;
            Country = country;
            Coordinates = coordinates;
            Distance = distance;
            Source = source;
            Type = type;
            DataProviders = dataProviders ?? new List<DataProviders>();
        }


        public Location(GeoPoint coordinates, int distance, LocationTypes type = LocationTypes.Unknown)
            : this(string.Empty, string.Empty, string.Empty, coordinates, distance, PredictionSources.NotSpecified, type, null)
        { }


        public GeoPoint Coordinates { get; }
        public string Country { get; }
        public int Distance { get; }
        public string Locality { get; }
        public string Name { get; }
        public PredictionSources Source { get; }
        public LocationTypes Type { get; }
        public List<DataProviders> DataProviders { get; }


        public override bool Equals(object obj) => obj is Location other && Equals(other);


        public bool Equals(Location other)
            => (Coordinates, Coordinates, Country, Distance, Locality, Name, Source, Type) == (other.Coordinates,
                other.Coordinates, other.Country, other.Distance, other.Locality, other.Name,
                other.Source, other.Type);


        public override int GetHashCode() => (Coordinates, Country, Distance, Locality, Name, Source, Type).GetHashCode();
    }
}