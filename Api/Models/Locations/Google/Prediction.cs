using System.Collections.Generic;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Locations.Google
{
    public readonly struct Prediction
    {
        [JsonConstructor]
        public Prediction(string id, string description, List<Term> terms, List<string> types)
        {
            Id = id;
            Description = description;
            Terms = terms;
            Types = types;
        }


        [JsonProperty("place_id")]
        public string Id { get; }

        [JsonProperty("description")]
        public string Description { get; }

        [JsonProperty("terms")]
        public List<Term> Terms { get; }

        [JsonProperty("types")]
        public List<string> Types { get; }
    }
}