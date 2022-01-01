using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HappyTravel.Edo.Common.Enums.Markup
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MarkupPolicyScopeType
    {
        NotSpecified = 0,
        Global = 1,
        Company = 2,
        Branch = 3,
        Customer = 4,
        EndClient = 5
    }
}