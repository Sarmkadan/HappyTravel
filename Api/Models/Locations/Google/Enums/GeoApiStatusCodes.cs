using System.Runtime.Serialization;

namespace HappyTravel.Edo.Api.Models.Locations.Google.Enums
{
    public enum GeoApiStatusCodes
    {
        [EnumMember(Value = "INVALID_REQUEST")]
        InvalidRequest,

        [EnumMember(Value = "OK")]
        Ok,

        [EnumMember(Value = "OVER_QUERY_LIMIT")]
        OverQueryLimit,

        [EnumMember(Value = "REQUEST_DENIED")]
        RequestDenied,

        [EnumMember(Value = "UNKNOWN_ERROR")]
        UnknownError,

        [EnumMember(Value = "ZERO_RESULTS")]
        ZeroResults
    }
}