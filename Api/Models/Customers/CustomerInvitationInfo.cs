using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Customers
{
    public readonly struct CustomerInvitationInfo
    {
        [JsonConstructor]
        public CustomerInvitationInfo(CustomerRegistrationInfo registrationInfo, int companyId, string email)
        {
            RegistrationInfo = registrationInfo;
            CompanyId = companyId;
            Email = email;
        }


        /// <summary>
        ///     Regular customer personal information.
        /// </summary>
        [Required]
        public CustomerRegistrationInfo RegistrationInfo { get; }

        /// <summary>
        ///     Related company id.
        /// </summary>
        [Required]
        public int CompanyId { get; }

        /// <summary>
        ///     E-mail for invitation.
        /// </summary>
        [Required]
        public string Email { get; }
    }
}