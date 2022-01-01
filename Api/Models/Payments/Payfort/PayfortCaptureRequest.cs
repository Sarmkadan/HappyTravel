using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Payments.Payfort
{
    public readonly struct PayfortCaptureRequest
    {
        [JsonConstructor]
        public PayfortCaptureRequest(string accessCode, string merchantIdentifier, string merchantReference, string amount, string currency, string language,
            string fortId, string signature)
        {
            AccessCode = accessCode;
            MerchantIdentifier = merchantIdentifier;
            MerchantReference = merchantReference;
            Amount = amount;
            Currency = currency;
            Language = language;
            FortId = fortId;
            Command = "CAPTURE";
            Signature = signature;
        }


        public PayfortCaptureRequest(PayfortCaptureRequest request, string signature) : this(
            signature: signature,
            accessCode: request.AccessCode,
            merchantIdentifier: request.MerchantIdentifier,
            merchantReference: request.MerchantReference,
            amount: request.Amount,
            currency: request.Currency,
            language: request.Language,
            fortId: request.FortId)
        { }


        public string Command { get; }
        public string AccessCode { get; }
        public string MerchantIdentifier { get; }
        public string MerchantReference { get; }
        public string Amount { get; }
        public string Currency { get; }
        public string Language { get; }
        public string FortId { get; }
        public string Signature { get; }
    }
}