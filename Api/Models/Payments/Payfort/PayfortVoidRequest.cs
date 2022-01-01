using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Payments.Payfort
{
    public readonly struct PayfortVoidRequest
    {
        [JsonConstructor]
        public PayfortVoidRequest(string accessCode, string merchantIdentifier, string merchantReference, string language,
            string fortId, string signature)
        {
            AccessCode = accessCode;
            MerchantIdentifier = merchantIdentifier;
            MerchantReference = merchantReference;
            Language = language;
            FortId = fortId;
            Command = "VOID_AUTHORIZATION";
            Signature = signature;
        }


        public PayfortVoidRequest(PayfortVoidRequest request, string signature) : this(
            signature: signature,
            accessCode: request.AccessCode,
            merchantIdentifier: request.MerchantIdentifier,
            merchantReference: request.MerchantReference,
            language: request.Language,
            fortId: request.FortId)
        { }


        public string Command { get; }
        public string AccessCode { get; }
        public string MerchantIdentifier { get; }
        public string MerchantReference { get; }
        public string Language { get; }
        public string FortId { get; }
        public string Signature { get; }
    }
}