using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HappyTravel.Edo.Common.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CreditCardEventType
    {
        None = 0,
        Authorize = 1,
        Capture = 2,
        Void = 3
    }
}
