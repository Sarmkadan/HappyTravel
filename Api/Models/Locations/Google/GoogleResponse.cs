using HappyTravel.Edo.Api.Models.Locations.Google.Enums;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Locations.Google
{
    public class GoogleResponse
    {
        [JsonProperty("status")]
        public GeoApiStatusCodes Status { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
    }
}