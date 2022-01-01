using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HappyTravel.Edo.Common.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SignatureTypes
    {
        Unknown = 0,
        Request = 1,
        Response = 2,
    }
}
