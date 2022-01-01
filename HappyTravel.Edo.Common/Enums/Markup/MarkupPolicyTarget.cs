using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HappyTravel.Edo.Common.Enums.Markup
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MarkupPolicyTarget
    {
        NotSpecified = 0,
        AccommodationAvailability = 1
    }
}