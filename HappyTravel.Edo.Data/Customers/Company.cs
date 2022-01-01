using System;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.EdoContracts.General.Enums;

namespace HappyTravel.Edo.Data.Customers
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CountryCode { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string PostalCode { get; set; }
        public Currencies PreferredCurrency { get; set; }
        public PaymentMethods PreferredPaymentMethod { get; set; }
        public string Website { get; set; }
        public CompanyStates State { get; set; }
        public DateTime Created { get; set; }
        public string VerificationReason { get; set; }
        public DateTime? Verified { get; set; }
        public DateTime Updated { get; set; }
    }
}