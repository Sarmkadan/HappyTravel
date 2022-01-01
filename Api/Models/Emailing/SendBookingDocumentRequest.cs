using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Emailing
{
    public readonly struct SendBookingDocumentRequest
    {
        [JsonConstructor]
        public SendBookingDocumentRequest(string email)
        {
            Email = email;
        }


        [Required]
        public string Email { get; }
    }
}