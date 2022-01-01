using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Locations.Google
{
    public class PlaceContainer : GoogleResponse
    {
        [JsonProperty("result")]
        public Place Place { get; set; }
    }
}