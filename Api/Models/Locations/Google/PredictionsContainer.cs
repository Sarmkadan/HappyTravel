using System.Collections.Generic;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Locations.Google
{
    public class PredictionsContainer : GoogleResponse
    {
        [JsonProperty("predictions")]
        public List<Prediction> Predictions { get; set; }
    }
}