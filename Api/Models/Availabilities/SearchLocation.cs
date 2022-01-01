using System.ComponentModel.DataAnnotations;
using HappyTravel.Geography;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Availabilities
{
    public readonly struct SearchLocation
    {
        [JsonConstructor]
        public SearchLocation(PredictionResult predictionResult, GeoPoint coordinates, [Range(0, 40000)] int distanceInMeters)
        {
            Coordinates = coordinates;
            DistanceInMeters = distanceInMeters;
            PredictionResult = predictionResult;
        }


        /// <summary>
        ///     Central point of a search.
        /// </summary>
        public GeoPoint Coordinates { get; }

        /// <summary>
        ///     Search distance in meters.
        /// </summary>
        public int DistanceInMeters { get; }

        /// <summary>
        ///     The result of a prediction query.
        /// </summary>
        public PredictionResult PredictionResult { get; }
    }
}