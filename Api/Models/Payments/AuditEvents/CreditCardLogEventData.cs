using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Payments.AuditEvents
{
    public readonly struct CreditCardLogEventData
    {
        [JsonConstructor]
        public CreditCardLogEventData(string reason, string externalCode, string message, string merchantReference)
        {
            Reason = reason;
            ExternalCode = externalCode;
            Message = message;
            MerchantReference = merchantReference;
        }


        public string Reason { get; }
        public string ExternalCode { get; }
        public string Message { get; }
        public string MerchantReference { get; }
    }
}