using HappyTravel.Edo.Common.Enums;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Payments.Payfort
{
    public readonly struct CreditCardVoidResult
    {
        [JsonConstructor]
        public CreditCardVoidResult(string externalCode, string message, string merchantReference)
        {
            ExternalCode = externalCode;
            Message = message;
            MerchantReference = merchantReference;
        }

        public string ExternalCode { get; }
        public string Message { get; }
        public string MerchantReference { get; }
    }
}