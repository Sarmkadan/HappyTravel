using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Payments
{
    /// <summary>
    ///     Settings for payfort tokenization
    /// </summary>
    public readonly struct TokenizationSettings
    {
        [JsonConstructor]
        public TokenizationSettings(string accessCode, string merchantIdentifier, string tokenizationUrl)
        {
            AccessCode = accessCode;
            MerchantIdentifier = merchantIdentifier;
            TokenizationUrl = tokenizationUrl;
        }


        /// <summary>
        ///     Access Code
        /// </summary>
        public string AccessCode { get; }

        /// <summary>
        ///     Merchant Identifier
        /// </summary>
        public string MerchantIdentifier { get; }

        /// <summary>
        ///     Payfort tokenization url
        /// </summary>
        public string TokenizationUrl { get; }
    }
}