using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Payments.Payfort
{
    public class PayfortVoidResponse : ISignedResponse
    {
        [JsonConstructor]
        public PayfortVoidResponse(string signature, string fortId, string authorizationCode, string responseMessage, string responseCode, string status,
            string merchantReference)
        {
            Signature = signature;
            FortId = fortId;
            AuthorizationCode = authorizationCode;
            ResponseMessage = responseMessage;
            ResponseCode = responseCode;
            Status = status;
            MerchantReference = merchantReference;
        }


        public string Signature { get; }
        public string FortId { get; }
        public string AuthorizationCode { get; }
        public string ResponseMessage { get; }
        public string ResponseCode { get; }
        public string Status { get; }
        public string MerchantReference { get; }
    }
}