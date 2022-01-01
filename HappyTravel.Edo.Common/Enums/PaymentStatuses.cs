using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HappyTravel.Edo.Common.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PaymentStatuses
    {
        Created = 0,
        Authorized = 1,
        Secure3d  = 2,
        Failed  = 3,
        Captured = 4,
        Refunded = 5,
        Voided = 6
    }
}
