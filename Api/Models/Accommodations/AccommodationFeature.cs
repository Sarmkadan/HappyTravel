using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Accommodations
{
    public readonly struct AccommodationFeature
    {
        [JsonConstructor]
        public AccommodationFeature(AccommodationFeatureTypes type, string value)
        {
            Type = type;
            Value = value;
        }


        public AccommodationFeatureTypes Type { get; }
        public string Value { get; }
    }
}