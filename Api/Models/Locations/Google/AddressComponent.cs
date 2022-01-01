using System.Collections.Generic;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Locations.Google
{
    public readonly struct AddressComponent
    {
        [JsonConstructor]
        public AddressComponent(string name, List<string> types)
        {
            Name = name;
            Types = types;
        }


        [JsonProperty("long_name")]
        public string Name { get; }

        public List<string> Types { get; }
    }
}