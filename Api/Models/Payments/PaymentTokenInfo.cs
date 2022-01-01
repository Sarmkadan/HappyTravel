using HappyTravel.Edo.Common.Enums;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Payments
{
    /// <summary>
    ///     Payment token info
    /// </summary>
    public readonly struct PaymentTokenInfo
    {
        [JsonConstructor]
        public PaymentTokenInfo(string code, PaymentTokenTypes type)
        {
            Code = code;
            Type = type;
        }


        /// <summary>
        ///     Payment token
        /// </summary>
        public string Code { get; }

        /// <summary>
        ///     Payment token type
        /// </summary>
        public PaymentTokenTypes Type { get; }
    }
}