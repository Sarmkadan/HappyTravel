using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Management
{
    public readonly struct CompanyVerificationRequest
    {
        [JsonConstructor]
        public CompanyVerificationRequest(string reason)
        {
            Reason = reason;
        }


        /// <summary>
        ///     Verify reason.
        /// </summary>
        public string Reason { get; }
    }
}