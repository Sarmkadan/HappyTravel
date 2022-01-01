using System.Collections.Generic;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Locations.Google
{
    public readonly struct Place
    {
        [JsonConstructor]
        public Place(string id, string name, Geometry geometry, List<AddressComponent> components)
        {
            Id = id;
            Components = components;
            Geometry = geometry;
            Name = name;
        }


        [JsonProperty("place_id")]
        public string Id { get; }

        [JsonProperty("address_components")]
        public List<AddressComponent> Components { get; }

        public Geometry Geometry { get; }
        public string Name { get; }


        public override bool Equals(object obj) => obj is Place other && Equals(other);


        public bool Equals(Place other) => (Id, Components, Geometry, Name) == (other.Id, other.Components, other.Geometry, other.Name);


        public override int GetHashCode() => (Id, Components, Geometry, Name).GetHashCode();
    }
}