using System;
using HappyTravel.Geography;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Locations.Google
{
    public readonly struct Geometry : IEquatable<Geometry>
    {
        [JsonConstructor]
        public Geometry(GeoPoint location, Viewport viewport)
        {
            Location = location;
            Viewport = viewport;
        }


        public GeoPoint Location { get; }
        public Viewport Viewport { get; }


        public override bool Equals(object obj) => obj is Geometry geometry && Equals(geometry);


        public bool Equals(Geometry other) => (Location, Viewport) == (other.Location, other.Viewport);


        public override int GetHashCode() => (Location, Viewport).GetHashCode();


        public static bool operator ==(Geometry left, Geometry right) => left.Equals(right);


        public static bool operator !=(Geometry left, Geometry right) => !left.Equals(right);
    }
}