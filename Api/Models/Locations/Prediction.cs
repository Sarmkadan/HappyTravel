using HappyTravel.EdoContracts.GeoData.Enums;

namespace HappyTravel.Edo.Api.Models.Locations
{
    public readonly struct Prediction
    {
        public Prediction(string id, string countryCode, PredictionSources source, LocationTypes type, string value)
        {
            Id = id;
            CountryCode = countryCode;
            Source = source;
            Type = type;
            Value = value;
        }


        /// <summary>
        ///     The ID of a prediction.
        /// </summary>
        public string Id { get; }

        /// <summary>
        ///     The country code of a prediction.
        /// </summary>
        public string CountryCode { get; }

        /// <summary>
        ///     The source of a prediction. <see cref="PredictionSources" />
        /// </summary>
        public PredictionSources Source { get; }

        /// <summary>
        ///     Type of a predicted location. <see cref="LocationTypes" />
        /// </summary>
        public LocationTypes Type { get; }

        /// <summary>
        ///     The combined value from prediction country, locality, and name values to display for a user.
        /// </summary>
        public string Value { get; }
    }
}