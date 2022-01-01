using System;
using HappyTravel.Geography;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Locations.Google
{
    public readonly struct Viewport : IEquatable<Viewport>
    {
        [JsonConstructor]
        public Viewport(GeoPoint northEast, GeoPoint southWest)
        {
            NorthEast = northEast;
            SouthWest = southWest;
        }


        [JsonProperty("northeast")]
        public GeoPoint NorthEast { get; }

        [JsonProperty("southwest")]
        public GeoPoint SouthWest { get; }


        public override bool Equals(object obj) => obj is Viewport viewport && Equals(viewport);


        public bool Equals(Viewport other) => (NorthEast, SouthWest) == (other.NorthEast, other.SouthWest);


        public override int GetHashCode() => (NorthEast, SouthWest).GetHashCode();


        public static bool operator ==(Viewport left, Viewport right) => left.Equals(right);


        public static bool operator !=(Viewport left, Viewport right) => !left.Equals(right);
    }
}