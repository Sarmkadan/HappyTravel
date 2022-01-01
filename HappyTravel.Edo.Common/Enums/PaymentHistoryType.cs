using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HappyTravel.Edo.Common.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentHistoryType
    {
        None = 0,
        Add = 1,
        Charge = 2,
        Authorize = 3,
        Capture = 4,
        Void = 5
    }
}