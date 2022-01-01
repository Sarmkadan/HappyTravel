using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HappyTravel.Edo.Common.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentTokenTypes
    {
        Unknown = 0,
        OneTime = 1,
        Stored = 2
    }
}
