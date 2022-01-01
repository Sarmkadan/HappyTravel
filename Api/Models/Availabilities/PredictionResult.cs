using HappyTravel.EdoContracts.GeoData.Enums;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Availabilities
{
    public readonly struct PredictionResult
    {
        [JsonConstructor]
        public PredictionResult(string id, string sessionId, PredictionSources source, LocationTypes type)
        {
            Id = id;
            SessionId = sessionId;
            Source = source;
            Type = type;
        }


        /// <summary>
        ///     The entity ID from a prediction query.
        /// </summary>
        public string Id { get; }

        /// <summary>
        ///     Prediction session ID.
        /// </summary>
        public string SessionId { get; }

        /// <summary>
        ///     Prediction source.
        /// </summary>
        public PredictionSources Source { get; }

        /// <summary>
        ///     A type Of a predicted entity.
        /// </summary>
        public LocationTypes Type { get; }
    }
}