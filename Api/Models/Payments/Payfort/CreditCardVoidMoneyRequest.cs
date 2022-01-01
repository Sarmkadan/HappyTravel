using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Payments.Payfort
{
    public readonly struct CreditCardVoidMoneyRequest
    {
        [JsonConstructor]
        public CreditCardVoidMoneyRequest(string externalId, string merchantReference, string languageCode)
        {
            ExternalId = externalId;
            MerchantReference = merchantReference;
            LanguageCode = languageCode;
        }


        public string ExternalId { get; }
        public string MerchantReference { get; }
        public string LanguageCode { get; }
    }
}