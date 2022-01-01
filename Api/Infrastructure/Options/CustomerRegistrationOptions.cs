using System.Collections.Generic;

namespace HappyTravel.Edo.Api.Infrastructure.Options
{
    public class CustomerRegistrationNotificationOptions
    {
        public string MasterCustomerMailTemplateId { get; set; }
        public string RegularCustomerMailTemplateId { get; set; }
        public List<string> AdministratorsEmails { get; set; }
    }
}