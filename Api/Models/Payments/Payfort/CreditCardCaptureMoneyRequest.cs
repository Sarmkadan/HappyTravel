using HappyTravel.EdoContracts.General.Enums;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Payments.Payfort
{
    public readonly struct CreditCardCaptureMoneyRequest
    {
        [JsonConstructor]
        public CreditCardCaptureMoneyRequest(decimal amount, Currencies currency, string externalId, string merchantReference, string languageCode)
        {
            Amount = amount;
            Currency = currency;
            ExternalId = externalId;
            MerchantReference = merchantReference;
            LanguageCode = languageCode;
        }


        public decimal Amount { get; }
        public Currencies Currency { get; }
        public string ExternalId { get; }
        public string MerchantReference { get; }
        public string LanguageCode { get; }
    }
}