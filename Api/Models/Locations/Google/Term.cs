using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Locations.Google
{
    public readonly struct Term
    {
        [JsonConstructor]
        public Term(string value)
        {
            Value = value;
        }


        [JsonProperty("value")]
        public string Value { get; }
    }
}